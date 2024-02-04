using Tasker.Executor.Service.Interfaces;
using Tasker.Logging.Interfaces;

namespace Tasker.Executor;

public class ExecutorWorker : BackgroundService
{
    private readonly IExecutorQueueService _executorQueueService;
    private readonly ITaskerLogger<ExecutorWorker> _logger;

    public ExecutorWorker(ITaskerLogger<ExecutorWorker> logger, IExecutorQueueService executorQueueService)
    {
        _logger = logger;
        _executorQueueService = executorQueueService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

            _executorQueueService.InitializeConsumer();
        }

        return Task.CompletedTask;
    }
}