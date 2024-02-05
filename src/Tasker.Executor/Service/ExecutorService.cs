using System.Reflection;
using Tasker.Executor.Jobs;
using Tasker.Executor.Jobs.Annotations;
using Tasker.Executor.Model;
using Tasker.Executor.Service.Interfaces;
using Tasker.Logging.Interfaces;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler.Enums;
using JobHistoryStatus = Tasker.Scheduler.Enums.JobHistoryStatus;

namespace Tasker.Executor.Service;

public class ExecutorService : IExecutorService
{
    private readonly IBaseRepository<Job> _jobRepository;
    private readonly IBaseRepository<JobHistory> _jobHistoryRepository;
    private readonly IEnumerable<JobDefinition> _jobs;
    private readonly ITaskerLogger<ExecutorService> _logger;

    public ExecutorService(
        ITaskerLogger<ExecutorService> logger,
        IBaseRepository<Job> jobRepository,
        IBaseRepository<JobHistory> jobHistoryRepository)
    {
        _logger = logger;
        _jobRepository = jobRepository;
        _jobHistoryRepository = jobHistoryRepository;
        _jobs = GetJobs();
    }

    public async Task ExecuteJob(string jobName)
    {
        try
        {
            var job = _jobs.FirstOrDefault(j => j.JobName == jobName);

            if (job == null)
            {
                _logger.LogError($"Job not found: {jobName}");
                throw new Exception($"Job not found: {jobName}");
            }

            await UpdateJobStatus(jobName, (int)JobStatus.InProgress, true, true);

            _logger.LogInformation($"Executing job: {jobName}");

            job.JobInstance.Execute();

            await UpdateJobStatus(jobName, (int)JobStatus.Completed, true);
            await InsertJobHistory(
                jobName,
                (int)JobHistoryStatus.ExecutedSuccessfully,
                "Job executed successfully",
                true
            );
        }
        catch (Exception e)
        {
            await UpdateJobStatus(jobName, (int)JobStatus.Failed, false);
            await InsertJobHistory(
                jobName,
                (int)JobHistoryStatus.Failed,
                "Error while executing job",
                false,
                e.StackTrace,
                e.Message
            );

            _logger.LogCritical($"Error while executing job: {jobName}", e);
        }
    }

    private async Task InsertJobHistory(
        string jobName,
        int jobHistoryStatusId,
        string message,
        bool success,
        string? stackTrace = null,
        string? exceptionMessage = null)
    {
        try
        {
            var job = (await _jobRepository.GetWhere(x => x.Name == jobName)).FirstOrDefault();

            if (job == null)
            {
                _logger.LogError($"Job not found: {jobName}");
                throw new Exception($"Job not found: {jobName}");
            }

            await _jobHistoryRepository.Add(new JobHistory
            {
                Id = default,
                Message = message,
                JobId = job.Id,
                JobStatus = jobHistoryStatusId,
                Success = success,
                StackTrace = stackTrace,
                ExceptionMessage = exceptionMessage,
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical("Error to insert job history", e);
        }
    }

    private async Task UpdateJobStatus(string jobName, int statusId, bool success, bool inProgress = false)
    {
        try
        {
            var job = (await _jobRepository.GetWhere(j => j.Name == jobName)).FirstOrDefault();

            if (job == null)
            {
                _logger.LogError($"Job not found: {jobName}");
                throw new Exception($"Job not found: {jobName}");
            }

            job.StatusId = statusId;
            job.InProgress = inProgress;
            job.LastExecution = DateTime.Now;
            job.CurrentlyRetries = !success ? job.CurrentlyRetries + 1 : 0;

            await _jobRepository.Update(job);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Error to update job", e);
        }
    }

    private IEnumerable<JobDefinition> GetJobs()
    {
        var jobsToExecute = new List<JobDefinition>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var jobClasses = assembly.GetTypes()
                .Where(type => Attribute.IsDefined(type, typeof(TaskerJob)));

            foreach (var jobClass in jobClasses)
            {
                try
                {
                    var taskerJobAttribute = jobClass.GetCustomAttribute<TaskerJob>();

                    if (Activator.CreateInstance(jobClass) is not BaseJob jobInstance) continue;

                    if (taskerJobAttribute?.JobName != null)
                        jobsToExecute.Add(new JobDefinition
                        {
                            JobName = taskerJobAttribute.JobName,
                            JobInstance = jobInstance
                        });
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error while finding job: {jobClass.Name}", e);
                }
            }
        }

        return jobsToExecute;
    }
}