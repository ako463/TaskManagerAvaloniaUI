using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Services;
using TaskManager.Tests.UnitTests.Stubs;

namespace TaskManager.Tests.UnitTests;

public class TaskNamingServiceTests
{
    [Fact]
    public async Task TaskNamingService_ShouldGiveCorrectDefaultTitle()
    {
        // Arrange
        var dbContextOptions = new DbContextOptions<TaskItemContext>();
        var context = new TaskItemContextStub(dbContextOptions);

        var date = new DateTime(2026, 02, 04, 16, 20, 11);
        var tasks = Enumerable.Range(3, 10).Select(i => TaskItem.New($"Task {i}", date)).ToList();

        tasks.Last().MarkAsDelete();
        
        tasks.Add(TaskItem.New("Some other title", date));

        await context.TasksItems.AddRangeAsync(tasks);
        await context.SaveChangesAsync();

        ITaskNamingService namingService = new TaskRepository(context);

        // Act
        string newTitle = await namingService.CreateDefaultTitleAsync();

        // Assert
        Assert.Equal("Task 13", newTitle);
    }
}
