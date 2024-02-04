namespace Tasker.Scheduler.Enums;

public enum JobStatus
{
    InProgress = 1,
    Completed = 2,
    SentToMq = 3,
    Failed = 4,
    Canceled = 5,
    FailedToSendToMq = 6
}