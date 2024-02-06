using Microsoft.EntityFrameworkCore;
using Tasker.Executor;
using Tasker.Executor.Service;
using Tasker.Executor.Service.Interfaces;
using Tasker.Executor.Utils;
using Tasker.Logging;
using Tasker.Logging.Interfaces;
using Tasker.Repository;
using Tasker.Repository.Context;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler.Enums;
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

builder.Services.AddTransient<IBaseRepository<Job>, BaseRepository<Job>>();
builder.Services.AddTransient<IBaseRepository<JobHistory>, BaseRepository<JobHistory>>();
builder.Services.AddSingleton<ITaskerLogger<ExecutorQueueService>, TaskerLogger<ExecutorQueueService>>();
builder.Services.AddSingleton<ITaskerLogger<NetMqConsumer>, TaskerLogger<NetMqConsumer>>();
builder.Services.AddSingleton<ITaskerLogger<ExecutorService>, TaskerLogger<ExecutorService>>();
builder.Services.AddSingleton<ITaskerLogger<ExecutorWorker>, TaskerLogger<ExecutorWorker>>();
builder.Services.AddTransient<IExecutorQueueService, ExecutorQueueService>();
builder.Services.AddTransient<IExecutorService, ExecutorService>();
builder.Services.AddSingleton<NetMqConsumer>();

builder.Services.AddHostedService<ExecutorWorker>();

var host = builder.Build();
host.Run();