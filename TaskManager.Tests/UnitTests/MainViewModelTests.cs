using Moq;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Tests.UnitTests;

public class MainViewModelTests
{
    private Mock<ITaskService> _taskServiceMock;

    public MainViewModelTests()
    {
        _taskServiceMock = new Mock<ITaskService>();

        var list = new List<TaskModel>()
        {
            new TaskModel()
            {
                Index = 1,
                Title = "Задача А",
                IsCompleted = false,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Index = 2,
                Title = "Задача Б",
                IsCompleted = true,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Index = 3,
                Title = "Задача В",
                IsCompleted = false,
                IsDeleted = false,
            }
        };

        _taskServiceMock.Setup(x => x.GetTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskModel>>(list));
    }

    [Fact]
    public void MainViewModel_ShouldReturnCorrectAmountOfTasks()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        unitUnderTest.LoadTasksCommand.Execute(null);

        Assert.Equal(unitUnderTest.Tasks?.Count, 3);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullyAddNewTask()
    {
        _taskServiceMock
            .Setup(x => x.CreateTaskAsync())
            .Returns(Task.FromResult(new TaskModel()));

        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(4, unitUnderTest.Tasks?.Count);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullySoftDeleteTask()
    {
        _taskServiceMock
            .Setup(x => x.SoftDelete(It.IsAny<Guid>()))
            .Returns(Task.FromResult(true));

        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.SelectedTask = unitUnderTest.Tasks.LastOrDefault();

        unitUnderTest.SoftDeleteTaskCommand.Execute(null);

        Assert.Null(unitUnderTest.SelectedTask);
    }

    [Fact]
    public void MainViewModel_ShouldNotThrowSoftDeleteTask_WhenTaskNotSelected()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        var exception = Record.Exception(() => unitUnderTest.SoftDeleteTaskCommand.Execute(null));

        Assert.Null(exception);
    }
}