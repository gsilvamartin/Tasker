using System.ComponentModel.DataAnnotations;

namespace Tasker.Dashboard.Model.Job;

public class CreateJobRequest
{
    [Required(ErrorMessage = "Job name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Maximum retries is required.")]
    [Range(0, 30, ErrorMessage = "Maximum retries must be a non-negative integer and have a maximum of 30 retries.")]
    public int MaximumRetries { get; set; }

    public string CronExpression { get; set; }

    [Required(ErrorMessage = "Interval in minutes is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Interval in minutes must be a non-negative integer.")]
    public int IntervalInMinutes { get; set; }
}