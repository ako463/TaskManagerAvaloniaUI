using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;
using TaskManager.Tests.UnitTests.Stubs;

namespace TaskManager.Tests.UnitTests;

public class TaskRepositoryTests
{
    private readonly TaskItemContextStub _context;

    public TaskRepositoryTests()
    {
        var dbContextOptions = new DbContextOptions<TaskItemContext>();
        
        _context = new TaskItemContextStub(dbContextOptions);
    }

    [Fact]
    public async Task TaskRepository_ShouldAddNewTask()
    {
        TaskItem taskItem = new()
        {
            Title = "My task",
            CreatedAt = new DateTime(2022, 12, 1, 12, 0, 0),
        };

        var unitUnderTest = new TaskRepository(_context);

        var updatedTaskItem = await unitUnderTest.Add(taskItem);

        Assert.Equal(taskItem.Title, updatedTaskItem.Title);
        Assert.NotNull(updatedTaskItem.Id.ToString());
    }

    [Fact]
    public async Task TaskRepository_ShouldRejectAddingNewTask_ThrowDueToEmptyTitle()
    {
        TaskItem taskItem = new()
        {
            Title = string.Empty
        };

        var unitUnderTest = new TaskRepository(_context);

        var exception = await Record.ExceptionAsync(async () => await unitUnderTest.Add(taskItem));

        Assert.NotNull(exception);
        Assert.Equal(TaskItem.EmptyTitleError, exception.Message);
    }

    [Fact]
    public async Task TaskRepository_ShouldRejectAddingNewTask_ThrowDueToLongTitle()
    {
        string longTitle = new string('a', 110);

        var taskItem = new TaskItem()
        {
            Title = longTitle,
        };

        var unitUnderTest = new TaskRepository(_context);

        var exception = await Record.ExceptionAsync(async () => await unitUnderTest.Add(taskItem));

        Assert.NotNull(exception);
        Assert.Equal(TaskItem.LongTitleError, exception.Message);
    }
}
