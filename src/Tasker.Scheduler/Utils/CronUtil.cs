using Tasker.Logging.Interfaces;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;

namespace Tasker.Scheduler.Utils;

public class CronUtil
{
    private readonly ITaskerLogger<CronUtil> _logger;

    public CronUtil(ITaskerLogger<CronUtil> logger)
    {
        _logger = logger;
    }

    public bool IsJobReadyToRun(
        int schedulingType,
        string? cronExpression,
        int? intervalInMinutes,
        DateTime? lastExecutionTime = null)
    {
        try
        {
            if (lastExecutionTime == null) return true;

            switch (schedulingType)
            {
                case (int)SchedulingType.Interval:
                {
                    if (intervalInMinutes == null)
                        throw new ArgumentNullException(nameof(intervalInMinutes));

                    return IsJobReadyToRunByInterval(lastExecutionTime.Value, intervalInMinutes.Value);
                }
            }

            return false;
        }
        catch (Exception e)
        {
            _logger.LogError("Invalid cron expression", e);

            throw new ArgumentException("Invalid cron expression");
        }
    }

    private bool IsJobReadyToRunByInterval(DateTime lastExecutionTime, int intervalInMinutes)
    {
        return lastExecutionTime.AddMinutes(intervalInMinutes) < DateTime.Now;
    }
}