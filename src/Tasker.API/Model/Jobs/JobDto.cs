namespace Tasker.API.Model.Jobs;

public class JobDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? MaximumRetries { get; set; }

    public int? CurrentlyRetries { get; set; }

    public int StatusId { get; set; }

    public int SchedulingTypeId { get; set; }

    public bool? InProgress { get; set; }

    public string? ExecutedBy { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public DateTime? LastExecution { get; set; }

    public string? CronExpression { get; set; }

    public int? IntervalInMinutes { get; set; }
}