using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasker.Core.Configuration;
using Tasker.Repository;

namespace Tasker.Core
{
    public static class TaskerBuilderExtensions
    {
        public static void InitializeTasker(this IServiceCollection services, Action<TaskerConfig> taskerConfig)
        {
            if (taskerConfig == null)
                throw new ArgumentNullException(nameof(taskerConfig), "Tasker configuration is required.");

            var config = GetTaskerConfig(taskerConfig);

            InitializeDatabase(ref services, config);
        }

        private static void InitializeDatabase(ref IServiceCollection services, TaskerConfig config)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                switch (config.DatabaseProvider)
                {
                    case DatabaseProvider.SqlServer:
                        options.UseSqlServer(config.DatabaseConnectionString);
                        break;
                    case DatabaseProvider.Sqlite:
                        options.UseSqlite(config.DatabaseConnectionString);
                        break;
                    case DatabaseProvider.Postgres:
                        options.UseNpgsql(config.DatabaseConnectionString);
                        break;
                    default:
                        options.UseSqlServer(config.DatabaseConnectionString);
                        break;
                }
            });
        }

        private static TaskerConfig GetTaskerConfig(Action<TaskerConfig> taskerConfig)
        {
            var config = new TaskerConfig();
            taskerConfig.Invoke(config);

            return config;
        }
    }
}