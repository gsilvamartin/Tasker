using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasker.Core.Configuration;
using Tasker.Executor;
using Tasker.Executor.Service;
using Tasker.Executor.Service.Interfaces;
using Tasker.Executor.Utils;
using Tasker.Logging;
using Tasker.Logging.Interfaces;
using Tasker.Repository.Context;
using Tasker.Scheduler;
using Tasker.Scheduler.Services;
using Tasker.Scheduler.Services.Interfaces;
using Tasker.Scheduler.Utils;
using Tasker.Util;

namespace Tasker.Core;

public static class TaskerExtensions
{
    public static IServiceCollection UseTaskerScheduler(
        this IServiceCollection services,
        Action<TaskerConfig> config)
    {
        var taskerConfig = GetTaskerConfig(config);

        InitializeEnvironmentVariables(ref services);
        InitializeDatabase(ref services, taskerConfig);

        Task.Run(() => InitializeExecutorDependencies(ref services));
        Task.Run(() => InitializeSchedulerDependencies(ref services));

        return services;
    }

    private static void InitializeExecutorDependencies(ref IServiceCollection services)
    {
        services.AddSingleton<ITaskerLogger<ExecutorQueueService>, TaskerLogger<ExecutorQueueService>>();
        services.AddSingleton<ITaskerLogger<ExecutorService>, TaskerLogger<ExecutorService>>();
        services.AddSingleton<ITaskerLogger<ExecutorWorker>, TaskerLogger<ExecutorWorker>>();
        services.AddSingleton<ITaskerLogger<NetMqConsumer>, TaskerLogger<NetMqConsumer>>();
        services.AddSingleton<ITaskerLogger<CronUtil>, TaskerLogger<CronUtil>>();

        services.AddSingleton<CronUtil>();
        services.AddTransient<NetMqConsumer>();
        services.AddTransient<IExecutorQueueService, ExecutorQueueService>();
        services.AddTransient<IExecutorService, ExecutorService>();

        services.AddHostedService<ExecutorWorker>();
    }

    private static void InitializeSchedulerDependencies(ref IServiceCollection services)
    {
        services.AddSingleton<ITaskerLogger<SchedulerQueueService>, TaskerLogger<SchedulerQueueService>>();
        services.AddSingleton<ITaskerLogger<SchedulerService>, TaskerLogger<SchedulerService>>();
        services.AddSingleton<ITaskerLogger<SchedulerWorker>, TaskerLogger<SchedulerWorker>>();
        services.AddSingleton<ITaskerLogger<NetMqPublisher>, TaskerLogger<NetMqPublisher>>();
        services.AddSingleton<ITaskerLogger<CronUtil>, TaskerLogger<CronUtil>>();

        services.AddSingleton<CronUtil>();
        services.AddTransient<NetMqPublisher>();
        services.AddTransient<ISchedulerQueueService, SchedulerQueueService>();
        services.AddTransient<ISchedulerService, SchedulerService>();

        services.AddHostedService<SchedulerWorker>();
    }

    private static TaskerConfig GetTaskerConfig(Action<TaskerConfig> taskerConfig)
    {
        var config = new TaskerConfig();
        taskerConfig.Invoke(config);

        return config;
    }

    private static void InitializeEnvironmentVariables(ref IServiceCollection services)
    {
        services.LoadEnvConfiguration();
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
        }, ServiceLifetime.Transient);
    }
}