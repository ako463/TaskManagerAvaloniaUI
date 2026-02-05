using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ValidateEntities();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ValidateEntities();

        return base.SaveChanges();
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

    protected void ValidateEntities()
    {
        var validationErrors = ChangeTracker
            .Entries<IValidatableObject>()
            .SelectMany(e => e.Entity.Validate(new ValidationContext(e)))
            .Where(r => r != ValidationResult.Success);

        if (validationErrors.Any())
        {
            throw new ValidationException(validationErrors.First().ErrorMessage);
        }
    }
}