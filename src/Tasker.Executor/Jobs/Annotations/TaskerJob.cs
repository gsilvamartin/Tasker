namespace Tasker.Executor.Jobs.Annotations;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TaskerJob : Attribute
{
    public string JobName { get; }

    public TaskerJob(string jobName)
    {
        JobName = jobName;
    }
}