using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Models;

namespace TaskManager.Tests.UnitTests;

public class TaskValidationTests
{
    [Fact]
    public void TaskModel_ShouldNotHaveAnyErrors()
    {
        var taskModel = new TaskModel()
        {
            Index = 1,
            Title = "Task A",
            CreatedAt = new DateTime(2025, 12, 1, 12, 0, 0),
            IsCompleted = false,
            IsDeleted = false,
        };

        Assert.False(taskModel.HasErrors);
    }

    [Fact]
    public void TaskModel_ShouldHaveError_EmptyTaskTitle()
    {
        var taskModel = new TaskModel()
        {
            Title = string.Empty,
        };

        Assert.True(taskModel.HasErrors);
        Assert.Equal(taskModel.GetErrors()?.Count(), 1);
        Assert.Equal(TaskItem.EmptyTitleError, taskModel.GetErrors()?.First().ErrorMessage);
    }

    [Fact]
    public void TaskModel_ShouldHaveError_TaskTitleExceedsMaximumLength()
    {
        string longTitle = new string('a', 110);

        var taskModel = new TaskModel()
        {
            Title = longTitle,
        };

        Assert.True(taskModel.HasErrors);
        Assert.Equal(taskModel.GetErrors()?.Count(), 1);
        Assert.Equal(TaskItem.LongTitleError, taskModel.GetErrors()?.First().ErrorMessage);
    }

    [Fact]
    public void TaskItem_ShouldNotHaveAnyErrors()
    {
        string title = "Task A";
        TaskItem? taskItem = null;

        var exception = Record.Exception(() => taskItem = TaskItem.New(title, new DateTime(2025, 12, 1, 12, 0, 0)));

        Assert.Null(exception);
        Assert.NotNull(taskItem);
    }

    [Fact]
    public void TaskItem_ShouldHaveError_EmptyTaskTitle()
    {
        string title = string.Empty;

        TaskItem? taskItem = null;

        var exception = Record.Exception(() => taskItem = TaskItem.New(title, new DateTime(2025, 12, 1, 12, 0, 0)));

        Assert.NotNull(exception);
        Assert.Equal(TaskItem.EmptyTitleError, exception.Message);
        Assert.Null(taskItem);
    }

    [Fact]
    public void TaskItem_ShouldHaveError_TaskTitleExceedsMaximumLength()
    {
        string longTitle = new string('a', 110);

        TaskItem? taskItem = null;

        var exception = Record.Exception(() => taskItem = TaskItem.New(longTitle, new DateTime(2025, 12, 1, 12, 0, 0)));

        Assert.NotNull(exception);
        Assert.Equal(TaskItem.LongTitleError, exception.Message);
        Assert.Null(taskItem);
    }
}
