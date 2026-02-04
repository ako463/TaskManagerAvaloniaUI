using TaskManager.Desktop.Models;

namespace TaskManager.Tests.UnitTests;

public class TaskItemMapperTests
{
    [Fact]
    public void TaskItemMapper_ShouldMapFromTaskModelCorrectly()
    {
        var taskModel = new TaskModel()
        {
            Id = 17,
            Title = "Task ABC",
            CreatedAt = new DateTime(2026, 02, 04, 16, 20, 11),
            IsCompleted = true,
            IsDeleted = true,
        };

        var taskItem = TaskItemMapper.MapFromTaskModel(taskModel);

        Assert.Equal(taskModel.Id, taskItem.Id);
        Assert.Equal(taskModel.Title, taskItem.Title);
        Assert.Equal(taskModel.CreatedAt, taskItem.CreatedAt);
        Assert.Equal(taskModel.IsCompleted, taskItem.IsCompleted);
        Assert.Equal(taskModel.IsDeleted, taskItem.IsDeleted);        
    }

    [Fact]
    public void TaskItemMapper_ShouldMapToTaskModelCorrectly()
    {
        var taskItem = new TaskItem()
        {
            Id = 32,
            Title = "Task DEF",
            CreatedAt = new DateTime(2026, 02, 04, 16, 20, 11),
            IsCompleted = true,
            IsDeleted = true,
        };

        var taskModel = TaskItemMapper.MapToTaskModel(taskItem);

        Assert.Equal(taskItem.Id, taskModel.Id);
        Assert.Equal(taskItem.Title, taskModel.Title);
        Assert.Equal(taskItem.CreatedAt, taskModel.CreatedAt);
        Assert.Equal(taskItem.IsCompleted, taskModel.IsCompleted);
        Assert.Equal(taskItem.IsDeleted, taskModel.IsDeleted);
    }
}
