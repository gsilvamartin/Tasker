using Tasker.Logging.Interfaces;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler.Enums;
using Tasker.Scheduler.Services.Interfaces;
using Tasker.Scheduler.Utils;

namespace Tasker.Scheduler.Services;

public class SchedulerService : ISchedulerService
{
    private readonly CronUtil _cronUtil;
    private readonly IBaseRepository<Job> _jobRepository;
    private readonly ITaskerLogger<SchedulerService> _logger;

    public SchedulerService(
        ITaskerLogger<SchedulerService> logger,
        CronUtil cronUtil,
        IBaseRepository<Job> jobRepository)
    {
        _logger = logger;
        _cronUtil = cronUtil;
        _jobRepository = jobRepository;
    }

    public async Task UpdateJobStatus(int jobId, int jobsStatusId)
    {
        try
        {
            var job = await _jobRepository.GetById(jobId);

            if (job == null)
            {
                _logger.LogError($"Job with id {jobId} not found");
                return;
            }

            job.StatusId = jobsStatusId;
            await _jobRepository.Update(job);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Critical error while updating job status", e);
        }
    }

    public async Task<IEnumerable<Job>> GetReadyToExecuteJobs()
    {
        try
        {
            return (await _jobRepository.GetWhere(x =>
                x.Status.Id == (int)JobStatus.Completed ||
                (x.Status.Id == (int)JobStatus.Failed && x.CurrentlyRetries < x.MaximumRetries) ||
                (x.Status.Id == (int)JobStatus.FailedToSendToMq && x.CurrentlyRetries < x.MaximumRetries) ||
                (x.Status.Id == (int)JobStatus.SentToMq &&
                 x.LastExecution.GetValueOrDefault().AddMinutes(5) < DateTime.Now)
            )).Where(x =>
                _cronUtil.IsJobReadyToRun(x.SchedulingTypeId, x.CronExpression, x.IntervalInMinutes, x.LastExecution));
        }
        catch (Exception e)
        {
            _logger.LogCritical("Critical error while getting jobs to execute", e);
            throw;
        }
    }
}