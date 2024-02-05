using System;
using System.Collections.Generic;

namespace Tasker.Repository.Entity;

public partial class JobHistoryStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<JobHistory> JobHistories { get; set; } = new List<JobHistory>();
}
