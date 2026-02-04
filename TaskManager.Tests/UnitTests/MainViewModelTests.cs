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
                Id = 1,
                Title = "Задача А",
                IsCompleted = false,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Id = 2,
                Title = "Задача Б",
                IsCompleted = true,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Id = 3,
                Title = "Задача В",
                IsCompleted = false,
                IsDeleted = true,
            },
            new TaskModel()
            {
                Id = 4,
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
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Count, 4);
    }

    [Fact]
    public void MainViewModel_ShouldGiveNewTasksConsistentTitles()
    {
        _taskServiceMock = new Mock<ITaskService>();

        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        var titles = new string[] { "Task 1", "Task 2", "Task 3"};

        unitUnderTest.AddTaskCommand.Execute(null);
        unitUnderTest.AddTaskCommand.Execute(null);
        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Count(t => titles.Contains(t.Title)), 3);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullySoftDeleteTask()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);
        unitUnderTest.LoadTasksCommand.Execute(null);

        unitUnderTest.SelectedTask = unitUnderTest.FilteredTasks.LastOrDefault();

        unitUnderTest.SoftDeleteTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Where(t => !t.IsDeleted).Count(), 2);
    }

    [Fact]
    public void MainViewModel_ShouldNotThrowSoftDeleteTask_WhenTaskNotSelected()
    {
        var unitUnderTest = new MainViewModel(_taskServiceMock.Object);

        var exception = Record.Exception(() => unitUnderTest.SoftDeleteTaskCommand.Execute(null));

        Assert.Null(exception);
    }
}