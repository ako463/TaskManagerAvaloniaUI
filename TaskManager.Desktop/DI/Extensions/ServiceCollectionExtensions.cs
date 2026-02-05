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
        collection.AddDbContext<TaskItemContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        collection.AddScoped<ITaskRepository, TaskRepository>();
        collection.AddTransient<ITaskService, TaskService>();
        collection.AddTransient<MainViewModel>();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        collection.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
            builder.AddConsole();
        });
    }
}
