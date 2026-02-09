using Microsoft.EntityFrameworkCore;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Domain.Exceptions;
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
    public async Task TaskRepository_ShouldReturnAllTasks()
    {
        MarkFirstAndLastItemAsDelete();

        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var repository = new TaskRepository(_context);

        var tasks = await repository.GetAllTaskItemsAsync();

        Assert.Equal(3, tasks?.Count());
    }

    [Fact]
    public async Task TaskRepository_ShouldReturnOnlyAvailableTasks()
    {
        MarkFirstAndLastItemAsDelete();
        
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var repository = new TaskRepository(_context);

        var tasks = await repository.GetTaskItemsAsync();

        Assert.Equal(1, tasks?.Count());
    }

    [Fact]
    public async Task TaskRepository_ShouldAddNewTask_Success()
    {
        string title = "My task";
        var taskItem = TaskItem.New(title, new DateTime(2025, 12, 1, 12, 0, 0));

        var repository = new TaskRepository(_context);

        var updatedTaskItem = await repository.InsertAsync(taskItem);

        Assert.Equal(taskItem.Title, updatedTaskItem.Title);
        Assert.NotNull(updatedTaskItem.Id.ToString());
    }

    [Fact]
    public async Task TaskRepository_ShouldGetById_Success()
    {
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var taskToFind = _context.TasksItems.Last();

        var repository = new TaskRepository(_context);

        var foundItem = await repository.GetByIdAsync(taskToFind.Id);

        Assert.NotNull(foundItem);
        Assert.Equal(taskToFind.Id, foundItem.Id);
    }

    [Fact]
    public async Task TaskRepository_ShouldGetById_Failed()
    {
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        Guid notExistentId = Guid.NewGuid();

        var repository = new TaskRepository(_context);

        var exception = await Record.ExceptionAsync(() => repository.GetByIdAsync(notExistentId));

        Assert.NotNull(exception);
        Assert.Equal(typeof(NotFoundException), exception.GetType());
        Assert.True(exception.Message?.EndsWith("not found"));
    }

    [Fact]
    public async Task TaskRepository_ShouldSoftDeleteTask_Success()
    {
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var taskItemToSoftDelete = _context.TasksItems.Last();

        var repository = new TaskRepository(_context);

        bool succeed = await repository.SoftDeleteAsync(taskItemToSoftDelete);

        Assert.True(succeed);
        Assert.Equal(3, _context.TasksItems.Count());
        Assert.True(taskItemToSoftDelete.IsDeleted);
    }

    [Fact]
    public async Task TaskRepository_ShouldSoftDeleteTask_Failed_CannotDeleteTwice()
    {
        _tasks.Last().MarkAsDelete();

        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var repository = new TaskRepository(_context);

        var taskItemToSoftDelete = _context.TasksItems.Last();

        bool succeed = await repository.SoftDeleteAsync(taskItemToSoftDelete);

        Assert.False(succeed);
    }

    [Fact]
    public async Task TaskRepository_ShouldUpdateTask_Success()
    {
        string newTitle = "Updated task title";
        
        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var affectedTaskItem = _context.TasksItems
            .AsNoTracking()
            .Last();        

        var repository = new TaskRepository(_context);

        var taskItemToUpdate = await repository.GetByIdAsync(affectedTaskItem.Id);

        taskItemToUpdate.SetTitle(newTitle);
        taskItemToUpdate.SetCompleted(true);

        bool succeed = await repository.UpdateAsync(taskItemToUpdate);

        Assert.True(succeed);

        var updatedTask = await repository.GetByIdAsync(taskItemToUpdate.Id);

        Assert.NotNull(updatedTask);
        Assert.Equal(newTitle, updatedTask.Title);
        Assert.True(updatedTask.IsCompleted);
    }

    [Fact]
    public async Task TaskRepository_ShouldUpdateTask_Failed_CannotUpdateDeleted()
    {
        string newTitle = "Updated task title";

        _tasks.Last().MarkAsDelete();

        await _context.TasksItems.AddRangeAsync(_tasks);
        await _context.SaveChangesAsync();

        var affectedTaskItem = _context.TasksItems
            .AsNoTracking()
            .Last();

        var repository = new TaskRepository(_context);

        var taskItemToUpdate = await repository.GetByIdAsync(affectedTaskItem.Id);

        taskItemToUpdate.SetTitle(newTitle);
        taskItemToUpdate.SetCompleted(true);

        bool succeed = await repository.UpdateAsync(taskItemToUpdate);

        Assert.False(succeed);

        // Сбрасываем контекст, чтобы очистить кэш с не сохраненными изменениями
        _context.ChangeTracker.Clear();

        var updatedTask = await repository.GetByIdAsync(taskItemToUpdate.Id);

        Assert.NotNull(updatedTask);
        Assert.NotEqual(newTitle, updatedTask.Title);
        Assert.False(updatedTask.IsCompleted);
    }

    private void MarkFirstAndLastItemAsDelete()
    {
        _tasks.First().MarkAsDelete();
        _tasks.Last().MarkAsDelete();
    }
}
