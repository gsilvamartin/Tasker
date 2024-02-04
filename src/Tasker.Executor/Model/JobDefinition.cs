using Tasker.Executor.Jobs;

namespace Tasker.Executor.Model;

public class JobDefinition
{
    public string JobName { get; set; }
    public BaseJob JobInstance { get; set; }
}