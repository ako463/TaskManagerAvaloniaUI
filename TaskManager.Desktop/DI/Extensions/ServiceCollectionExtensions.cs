using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
