using Microsoft.EntityFrameworkCore;
using Tasker.Repository;
using Tasker.Repository.Context;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;
using Tasker.Scheduler.Enums;
using Tasker.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.LoadEnvConfiguration();


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

builder.Services.AddTransient<IBaseRepository<Job>, BaseRepository<Job>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();