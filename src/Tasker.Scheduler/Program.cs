using Microsoft.EntityFrameworkCore;
using Tasker.Logging;
using Tasker.Logging.Interfaces;
using Tasker.Repository.Context;
using Tasker.Scheduler;
using Tasker.Scheduler.Enums;
using Tasker.Scheduler.Services;
using Tasker.Scheduler.Services.Interfaces;
using Tasker.Scheduler.Utils;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    switch (DatabaseProvider.SqlServer)
    {
        case DatabaseProvider.SqlServer:
            options.UseSqlServer(
                builder.Configuration.GetValue<string>("API_SQL_CONNECTION_STRING"),
                sqlServerOptions => { });
            break;
    }
}, ServiceLifetime.Transient);

builder.Services.AddSingleton<ITaskerLogger<SchedulerService>, TaskerLogger<SchedulerService>>();
builder.Services.AddSingleton<ITaskerLogger<SchedulerWorker>, TaskerLogger<SchedulerWorker>>();
builder.Services.AddSingleton<ITaskerLogger<NetMqPublisher>, TaskerLogger<NetMqPublisher>>();
builder.Services.AddSingleton<ITaskerLogger<CronUtil>, TaskerLogger<CronUtil>>();

builder.Services.AddSingleton<CronUtil>();
builder.Services.AddTransient<NetMqPublisher>();
builder.Services.AddTransient<ISchedulerQueueService, SchedulerQueueService>();
builder.Services.AddTransient<ISchedulerService, SchedulerService>();

builder.Services.AddHostedService<SchedulerWorker>();

var host = builder.Build();
host.Run();