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

        var updatedTaskItem = await unitUnderTest.AddAsync(taskItem);

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

        var exception = await Record.ExceptionAsync(async () => await unitUnderTest.AddAsync(taskItem));

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

        var exception = await Record.ExceptionAsync(async () => await unitUnderTest.AddAsync(taskItem));

        Assert.NotNull(exception);
        Assert.Equal(TaskItem.LongTitleError, exception.Message);
    }

    [Fact]
    public async Task TaskRepository_ShouldSoftDeleteTask_Success()
    {
        var tasks = new List<TaskItem>
        {
            new TaskItem() { Index = 1, Title = "Task 1" },
            new TaskItem() { Index = 2, Title = "Task 2" },
            new TaskItem() { Index = 3, Title = "Task 3" }
        };

        await _context.TasksItems.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        var taskItemToSoftDelete = _context.TasksItems.Last();

        var unitUnderTest = new TaskRepository(_context);

        bool succeed = await unitUnderTest.SoftDeleteAsync(taskItemToSoftDelete);

        Assert.True(succeed);
        Assert.Equal(3, _context.TasksItems.Count());
        Assert.True(taskItemToSoftDelete.IsDeleted);
    }

    [Fact]
    public async Task TaskRepository_ShouldUpdateTask_Success()
    {
        string newTitle = "Updated task title";
        
        var tasks = new List<TaskItem>
        {
            new TaskItem() { Index = 1, Title = "Task 1" },
            new TaskItem() { Index = 2, Title = "Task 2" },
            new TaskItem() { Index = 3, Title = "Task 3" }
        };
        
        await _context.TasksItems.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        var affectedTaskItem = _context.TasksItems.Last();

        var taskItemToUpdate = new TaskItem()
        {
            Id = affectedTaskItem.Id,
            Title = newTitle,
            IsCompleted = true
        };

        var unitUnderTest = new TaskRepository(_context);

        bool succeed = await unitUnderTest.UpdateAsync(taskItemToUpdate);

        Assert.True(succeed);
        Assert.Equal(3, _context.TasksItems.Count());
        Assert.Equal(newTitle, affectedTaskItem.Title);
        Assert.True(affectedTaskItem.IsCompleted);
    }
}
