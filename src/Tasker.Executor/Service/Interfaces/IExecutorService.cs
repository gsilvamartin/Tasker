using Tasker.Executor.Model;

namespace Tasker.Executor.Service.Interfaces;

public interface IExecutorService
{
    public Task ExecuteJob(string jobName);
}