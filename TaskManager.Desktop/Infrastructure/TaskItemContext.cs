using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManager.Desktop.Domain;

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
            var configuration = ConfigurationProvider.Provide();

            var usePostgres = configuration!.GetValue<bool>("Database:UsePostgres");

            if (usePostgres)
            {
                optionsBuilder.UseNpgsql(configuration!.GetValue<string>("Database:ConnectionStrings:Postgres"));
            }
            else
            {
                optionsBuilder.UseSqlite(configuration!.GetValue<string>("Database:ConnectionStrings:Sqlite"));
            }
        }
    }
}