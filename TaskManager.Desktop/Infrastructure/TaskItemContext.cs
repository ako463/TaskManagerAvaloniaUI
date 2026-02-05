using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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