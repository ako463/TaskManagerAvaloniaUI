using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Infrastructure;

public class TaskItemContext : DbContext
{
    public DbSet<TaskItem> TasksItems { get; set; }

    public TaskItemContext(DbContextOptions<TaskItemContext> options) : base(options)
    {
    }

    public TaskItemContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Этот метод вызывается только если контекст создается без DI
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}