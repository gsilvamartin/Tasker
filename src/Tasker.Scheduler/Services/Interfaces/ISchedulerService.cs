using Tasker.Repository.Entity;

namespace Tasker.Scheduler.Services.Interfaces;

public interface ISchedulerService
{
    public Task<IEnumerable<Job>> GetReadyToExecuteJobs();
    public Task UpdateJobStatus(int jobId, int jobsStatusId);
}