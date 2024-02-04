using System.Reflection;
using Tasker.Executor.Jobs;
using Tasker.Executor.Jobs.Annotations;
using Tasker.Executor.Model;
using Tasker.Executor.Service.Interfaces;
using Tasker.Executor.Utils;
using Tasker.Logging.Interfaces;
using Tasker.Repository;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler.Enums;

namespace Tasker.Executor.Service;

public class ExecutorService : IExecutorService
{
    private readonly IBaseRepository<Job> _jobRepository;
    private readonly IEnumerable<JobDefinition> _jobs;
    private readonly ITaskerLogger<ExecutorService> _logger;

    public ExecutorService(ITaskerLogger<ExecutorService> logger, IBaseRepository<Job> jobRepository)
    {
        _logger = logger;
        _jobRepository = jobRepository;
        _jobs = GetJobs();
    }

    public void ExecuteJob(string jobName)
    {
        try
        {
            var job = _jobs.FirstOrDefault(j => j.JobName == jobName);

            if (job == null)
            {
                _logger.LogError($"Job not found: {jobName}");
                throw new Exception($"Job not found: {jobName}");
            }

            UpdateJobStatus(jobName, (int)JobStatus.InProgress, true, true);

            _logger.LogInformation($"Executing job: {jobName}");

            job.JobInstance.Execute();

            UpdateJobStatus(jobName, (int)JobStatus.Completed, true);
            InsertJobHistory(jobName, (int)JobHistoryStatus.ExecutedSuccessfully, true);
        }
        catch (Exception e)
        {
            UpdateJobStatus(jobName, (int)JobStatus.Failed, false);
            InsertJobHistory(jobName, (int)JobHistoryStatus.Failed, true, e.StackTrace, e.Message, e.Source);

            _logger.LogCritical($"Error while executing job: {jobName}", e);
        }
    }

    private void InsertJobHistory(
        string jobName,
        int jobHistoryStatusId,
        bool success,
        string? stackTrace = null,
        string? errorMessage = null,
        string? exceptionName = null)
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogCritical("Error to insert job history", e);
        }
    }

    private void UpdateJobStatus(string jobName, int statusId, bool success, bool inProgress = false)
    {
        try
        {
            var job = _jobRepository.GetWhere(j => j.Name == jobName).Result.FirstOrDefault();

            if (job == null)
            {
                _logger.LogError($"Job not found: {jobName}");
                throw new Exception($"Job not found: {jobName}");
            }

            job.StatusId = statusId;
            job.InProgress = inProgress;
            job.LastExecution = DateTime.Now;
            job.CurrentlyRetries = !success ? job.CurrentlyRetries + 1 : 0;

            _jobRepository.Update(job).Wait();
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