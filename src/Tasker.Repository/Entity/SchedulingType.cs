using System;
using System.Collections.Generic;

namespace Tasker.Repository.Entity;

public partial class SchedulingType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
