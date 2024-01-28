using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tasker.Repository.Migrations.Factory;

public class TaskerMigrationsDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public static void Main(string[] args)
    {
    }

    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=tasker;User Id=sa;Password=Gui41477188;Integrated Security=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");

        return new DatabaseContext(optionsBuilder.Options);
    }
}