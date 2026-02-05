using Moq;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;
using TaskManager.Desktop.ViewModels;
using TaskManager.Tests.UnitTests.Stubs;

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
                IsDeleted = true,
            },
            new TaskModel()
            {
                Index = 4,
                Title = "Задача Г",
                IsCompleted = false,
                IsDeleted = false,
            }
        };

        _taskServiceMock.Setup(x => x.GetTasks()).Returns(Task.FromResult<IEnumerable<TaskModel>>(list));
    }

    [Fact]
    public void MainViewModel_ShouldReturnCorrectAmountOfTasks()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        unitUnderTest.LoadTasksCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Count, 3);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullyAddNewTask()
    {
        _taskServiceMock
            .Setup(x => x.Add(It.IsAny<TaskModel>()))
            .Returns(Task.FromResult(new TaskModel()));

        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Count, 4);
    }

    [Fact]
    public void MainViewModel_ShouldGiveNewTasksConsistentTitles()
    {
        var taskServiceStub = new TaskServiceStub();

        var unitUnderTest = new MainViewModel(taskServiceStub);

        var titles = new string[] { "Task 1", "Task 2", "Task 3"};

        unitUnderTest.AddTaskCommand.Execute(null);
        unitUnderTest.AddTaskCommand.Execute(null);
        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(3, unitUnderTest.FilteredTasks?.Count(t => titles.Contains(t.Title)));
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullySoftDeleteTask()
    {
        _taskServiceMock
            .Setup(x => x.SoftDelete(It.IsAny<TaskModel>()))
            .Returns(Task.FromResult(true));

        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.SelectedTask = unitUnderTest.FilteredTasks.LastOrDefault();

        unitUnderTest.SoftDeleteTaskCommand.Execute(null);

        Assert.Equal(2, unitUnderTest.FilteredTasks?.Where(t => !t.IsDeleted).Count());
    }

    [Fact]
    public void MainViewModel_ShouldNotThrowSoftDeleteTask_WhenTaskNotSelected()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        var exception = Record.Exception(() => unitUnderTest.SoftDeleteTaskCommand.Execute(null));

        Assert.Null(exception);
    }
}