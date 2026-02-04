using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Infrastructure;

public class ApplicationContext : DbContext
{
    public DbSet<TaskItem> TasksItems { get; set; }

    public ApplicationContext()
    {
        //Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tasks;Username=postgres;Password=admin");
    }
}