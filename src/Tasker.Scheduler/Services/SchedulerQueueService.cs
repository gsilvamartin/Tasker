using Tasker.Logging.Interfaces;
using Tasker.Scheduler.Services.Interfaces;
using Tasker.Scheduler.Utils;

namespace Tasker.Scheduler.Services;

public class SchedulerQueueService : ISchedulerQueueService
{
    private readonly NetMqPublisher _netMqPublisher;
    private readonly ITaskerLogger<SchedulerQueueService> _logger;

    public SchedulerQueueService(NetMqPublisher netMqPublisher, ITaskerLogger<SchedulerQueueService> logger)
    {
        _netMqPublisher = netMqPublisher;
        _logger = logger;
    }

    public void SendJobToMq(string jobName)
    {
        try
        {
            _netMqPublisher.PublishBatchMessages(jobName);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while sending ready to execute jobs to MQ", e);
        }
    }
}