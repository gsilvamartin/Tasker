using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Repository.Entity;

public class GlobalConfiguration : BaseEntity
{
    [Required] public string Name { get; set; }

    public int MaximumRetries { get; set; }

    public int CurrentlyRetries { get; set; }

    public int StatusId { get; set; }

    public Status Status { get; set; }

    public bool InProgress { get; set; }

    public string ExecutedBy { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastExecution { get; set; }

    public DateTime? LastUpdate { get; set; }
}