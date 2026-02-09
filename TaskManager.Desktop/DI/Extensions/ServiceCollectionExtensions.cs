using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Services;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Desktop.DI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection, IConfiguration configuration)
    {
        var usePostgres = configuration.GetValue<bool>("Database:UsePostgres");

        if (usePostgres)
        {
            collection.AddDbContext<TaskItemContext>(options =>
                options.UseNpgsql(configuration.GetValue<string>("Database:ConnectionStrings:Postgres")));
        }
        else
        {
            collection.AddDbContext<TaskItemContext>(options =>
                options.UseSqlite(configuration.GetValue<string>("Database:ConnectionStrings:Sqlite")));
        }

        collection.AddScoped<ITaskNamingService, TaskNamingService>();
        collection.AddScoped<ITaskRepository, TaskRepository>();
        collection.AddTransient<ITaskService, TaskService>();
        collection.AddTransient<MainViewModel>();

        var loggingConfig = configuration.GetSection("Logging");
        string path = loggingConfig.GetValue<string>("path") ?? "logs/log-.txt";
        var rollingInterval = (RollingInterval)Enum.Parse(typeof(RollingInterval), loggingConfig.GetValue<string>("rollingInterval") ?? "Day");
        int retainedFileCountLimit = loggingConfig.GetValue<int>("retainedFileCountLimit");
        string outputTemplate = loggingConfig.GetValue<string>("outputTemplate") 
            ?? "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: path,
                rollingInterval: rollingInterval,
                retainedFileCountLimit: retainedFileCountLimit,
                outputTemplate: outputTemplate)
            .CreateLogger();

        collection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
            builder.AddConsole();
        });
    }
}
