namespace Tasker.Executor.Service.Interfaces;

public interface IExecutorQueueService
{
    public void InitializeConsumer();

    public void HandleSchedulerMessage(string message);
}