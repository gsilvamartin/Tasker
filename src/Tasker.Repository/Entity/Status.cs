namespace Tasker.Repository.Entity;

public class Status: BaseEntity
{
    public string Name { get; set; }
    
    public ICollection<GlobalConfiguration> GlobalConfiguration { get; set; }
}