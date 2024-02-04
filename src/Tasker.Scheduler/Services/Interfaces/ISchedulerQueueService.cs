namespace Tasker.Scheduler.Services.Interfaces;

public interface ISchedulerQueueService
{
    void SendJobToMq(string jobName);
}