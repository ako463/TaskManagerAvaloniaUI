using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Infrastructure;

namespace TaskManager.Tests.UnitTests.Stubs;

public class TaskItemContextStub : TaskItemContext
{
    public TaskItemContextStub(DbContextOptions<TaskItemContext> options) : base(options)
    {
        base.Database.EnsureDeleted();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    { 
        optionsBuilder.UseInMemoryDatabase("tasks"); 
    }
}
