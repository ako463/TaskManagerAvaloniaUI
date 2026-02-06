using System.IO;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Desktop.Infrastructure;

public static class ConfigurationProvider
{
    public static IConfigurationRoot? Provide()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
