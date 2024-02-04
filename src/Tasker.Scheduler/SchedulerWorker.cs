using Tasker.Logging.Interfaces;
using Tasker.Scheduler.Enums;
using Tasker.Scheduler.Services.Interfaces;

namespace Tasker.Scheduler;

public class SchedulerWorker : BackgroundService
{
    private readonly ISchedulerQueueService _schedulerQueueService;
    private readonly ISchedulerService _schedulerService;
    private readonly ITaskerLogger<SchedulerWorker> _logger;

    public SchedulerWorker(
        ITaskerLogger<SchedulerWorker> logger,
        ISchedulerService schedulerService,
        ISchedulerQueueService schedulerQueueService)
    {
        _logger = logger;
        _schedulerService = schedulerService;
        _schedulerQueueService = schedulerQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var jobs = (await _schedulerService.GetReadyToExecuteJobs()).ToList();

                _logger.LogInformation($"Found {jobs.Count} jobs to execute");

                foreach (var job in jobs)
                {
                    try
                    {
                        _logger.LogInformation($"Sending job {job.Name} to MQ");

                        await _schedulerService.UpdateJobStatus(job.Id, (int)JobStatus.SentToMq);
                        _schedulerQueueService.SendJobToMq(job.Name);
                    }
                    catch (Exception e)
                    {
                        await _schedulerService.UpdateJobStatus(job.Id, (int)JobStatus.FailedToSendToMq);
                        _logger.LogError($"Error while sending job to MQ: {job.Name}", e);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while executing scheduler", e);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}