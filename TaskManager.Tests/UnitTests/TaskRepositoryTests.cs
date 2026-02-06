using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Infrastructure;
using TaskManager.Desktop.Services;
using TaskManager.Tests.UnitTests.Stubs;

namespace TaskManager.Tests.UnitTests;

public class TaskRepositoryTests
{
    private readonly TaskItemContextStub _context;

    List<TaskItem> _tasks;

    public TaskRepositoryTests()
    {
        var dbContextOptions = new DbContextOptions<TaskItemContext>();
        
        _context = new TaskItemContextStub(dbContextOptions);

        _tasks = new List<TaskItem>
        {
            TaskItem.New("Task 1", new DateTime(2025, 12, 1, 12, 0, 0)),
            TaskItem.New("Task 2", new DateTime(2025, 12, 1, 13, 0, 0)),
            TaskItem.New("Task 3", new DateTime(2025, 12, 1, 14, 0, 0))
        };
    }

    [Fact]
    public async Task TaskRepository_ShouldAddNewTask()
    {
        string title = "My task";
        var taskItem = TaskItem.New(title, new DateTime(2025, 12, 1, 12, 0, 0));

        var unitUnderTest = new TaskRepository(_context);

        var updatedTaskItem = await unitUnderTest.InsertAsync(taskItem);

        Assert.Equal(taskItem.Title, updatedTaskItem.Title);
        Assert.NotNull(updatedTaskItem.Id.ToString());
    }

    [Fact]
    public async Task TaskRepository_ShouldSoftDeleteTask_Success()
    {
        await _context.TasksItems.AddRangeAsync(_tasks);
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
        
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var affectedTaskItem = _context.TasksItems.Last();        

        var unitUnderTest = new TaskRepository(_context);

        var taskItemToUpdate = await unitUnderTest.GetByIdAsync(affectedTaskItem.Id);

        taskItemToUpdate.SetTitle(newTitle);
        taskItemToUpdate.SetCompleted(true);

        bool succeed = await unitUnderTest.UpdateAsync(taskItemToUpdate);

        Assert.True(succeed);
        Assert.Equal(3, _context.TasksItems.Count());
        Assert.Equal(newTitle, affectedTaskItem.Title);
        Assert.True(affectedTaskItem.IsCompleted);
    }
}
