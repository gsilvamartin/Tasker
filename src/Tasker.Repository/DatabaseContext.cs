using Microsoft.EntityFrameworkCore;
using Tasker.Repository.Entity;

namespace Tasker.Repository;

public class DatabaseContext : DbContext
{
    public DbSet<Status> Status { get; set; }
    public DbSet<GlobalConfiguration> GlobalConfigurations { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}