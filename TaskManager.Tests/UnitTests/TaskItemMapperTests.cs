using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Models;

namespace TaskManager.Tests.UnitTests;

public class TaskItemMapperTests
{
    [Fact]
    public void TaskItemMapper_ShouldMapToTaskModelCorrectly()
    {
        string title = "Task ABC";

        var taskItem = TaskItem.New(title, new DateTime(2026, 02, 04, 16, 20, 11));

        var taskModel = TaskItemMapper.MapToTaskModel(taskItem);

        Assert.Equal(taskItem.Title, taskModel.Title);
        Assert.Equal(taskItem.CreatedAt, taskModel.CreatedAt);
        Assert.Equal(taskItem.IsCompleted, taskModel.IsCompleted);
        Assert.Equal(taskItem.IsDeleted, taskModel.IsDeleted);
    }
}
