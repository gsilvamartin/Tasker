using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Repository.Entity;

public abstract class BaseEntity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
}