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
        var dbName = $"tasks_{Guid.NewGuid()}";

        optionsBuilder.UseInMemoryDatabase(dbName); 
    }
}
