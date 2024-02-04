using Tasker.Executor.Model;

namespace Tasker.Executor.Service.Interfaces;

public interface IExecutorService
{
    public void ExecuteJob(string jobName);
}