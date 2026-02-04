using Microsoft.Extensions.DependencyInjection;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Desktop.DI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddTransient<MainViewModel>();
    }
}
