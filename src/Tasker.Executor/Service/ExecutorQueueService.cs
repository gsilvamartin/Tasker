using Tasker.Executor.Service.Interfaces;
using Tasker.Executor.Utils;
using Tasker.Logging.Interfaces;

namespace Tasker.Executor.Service;

public class ExecutorQueueService : IExecutorQueueService
{
    private readonly NetMqConsumer _netMqConsumer;
    private readonly IExecutorService _executorService;
    private readonly ITaskerLogger<ExecutorQueueService> _logger;

    public ExecutorQueueService(
        NetMqConsumer netMqConsumer,
        ITaskerLogger<ExecutorQueueService> logger,
        IExecutorService executorService)
    {
        _logger = logger;
        _netMqConsumer = netMqConsumer;
        _executorService = executorService;
    }

    public void InitializeConsumer()
    {
        _logger.LogInformation("Initializing consumer...");
        _netMqConsumer.StartListeningTopic(HandleSchedulerMessage);
    }

    public void HandleSchedulerMessage(string message)
    {
        _logger.LogInformation($"Received message: {message}");
        _executorService.ExecuteJob(message);
    }
}