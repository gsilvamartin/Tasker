using Tasker.Executor;
using Tasker.Executor.Service;
using Tasker.Executor.Service.Interfaces;
using Tasker.Executor.Utils;
using Tasker.Logging;
using Tasker.Logging.Interfaces;
using Tasker.Util;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.LoadEnvConfiguration();

builder.Services.AddHostedService<ExecutorWorker>();
builder.Services.AddSingleton<ITaskerLogger<NetMqConsumer>, TaskerLogger<NetMqConsumer>>();
builder.Services.AddSingleton<ITaskerLogger<ExecutorService>, TaskerLogger<ExecutorService>>();
builder.Services.AddTransient<IExecutorService, ExecutorService>();
builder.Services.AddTransient<IExecutorQueueService, ExecutorQueueService>();
builder.Services.AddSingleton<NetMqConsumer>();

builder.Services.AddSingleton<ITaskerLogger<ExecutorWorker>, TaskerLogger<ExecutorWorker>>();

var host = builder.Build();
host.Run();