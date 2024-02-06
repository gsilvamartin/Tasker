using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tasker.Logging;
using Tasker.Logging.Interfaces;
using Tasker.Repository;
using Tasker.Repository.Context;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler;
using Tasker.Scheduler.Enums;
using Tasker.Scheduler.Services;
using Tasker.Scheduler.Services.Interfaces;
using Tasker.Scheduler.Utils;
using Tasker.Util;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.LoadEnvConfiguration();
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    switch (DatabaseProvider.SqlServer)
    {
        case DatabaseProvider.SqlServer:
            options.UseSqlServer(sqlServerOptionsAction => { });
            break;
    }
}, ServiceLifetime.Transient);

builder.Services.AddSingleton<ITaskerLogger<SchedulerQueueService>, TaskerLogger<SchedulerQueueService>>();
builder.Services.AddSingleton<ITaskerLogger<SchedulerService>, TaskerLogger<SchedulerService>>();
builder.Services.AddSingleton<ITaskerLogger<SchedulerWorker>, TaskerLogger<SchedulerWorker>>();
builder.Services.AddSingleton<ITaskerLogger<NetMqPublisher>, TaskerLogger<NetMqPublisher>>();
builder.Services.AddSingleton<ITaskerLogger<CronUtil>, TaskerLogger<CronUtil>>();

builder.Services.AddSingleton<CronUtil>();
builder.Services.AddTransient<NetMqPublisher>();
builder.Services.AddTransient<IBaseRepository<Job>, BaseRepository<Job>>();
builder.Services.AddTransient<IBaseRepository<JobHistory>, BaseRepository<JobHistory>>();
builder.Services.AddTransient<ISchedulerQueueService, SchedulerQueueService>();
builder.Services.AddTransient<ISchedulerService, SchedulerService>();

builder.Services.AddHostedService<SchedulerWorker>();

var host = builder.Build();
host.Run();