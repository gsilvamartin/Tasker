using System;
using System.Collections.Generic;

namespace Tasker.Repository.Entity;

public partial class JobHistory
{
    public Guid Id { get; set; }

    public string? Message { get; set; }

    public int JobStatus { get; set; }

    public int JobId { get; set; }

    public DateTime? HistoryDate { get; set; }

    public bool Success { get; set; }

    public string? StackTrace { get; set; }

    public string? ExceptionMessage { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual JobHistoryStatus JobStatusNavigation { get; set; } = null!;
}
