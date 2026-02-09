using Moq;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Tests.UnitTests;

public class MainViewModelTests
{
    private Mock<ITaskService> _taskServiceMock;
    private MainViewModel _mainViewModel;

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

        _mainViewModel = new MainViewModel(_taskServiceMock.Object);
    }

    [Fact]
    public void MainViewModel_ShouldReturnCorrectAmountOfTasks()
    {
        _mainViewModel.LoadTasksCommand.Execute(null);

        Assert.Equal(_mainViewModel.Tasks?.Count, 3);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullyAddNewTask()
    {
        _taskServiceMock
            .Setup(x => x.CreateTaskAsync())
            .Returns(Task.FromResult(new TaskModel()));

        _mainViewModel.LoadTasksCommand.Execute(null);

        _mainViewModel.AddTaskCommand.Execute(null);

        Assert.Equal(4, _mainViewModel.Tasks?.Count);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullySoftDeleteTask()
    {
        _taskServiceMock
            .Setup(x => x.SoftDelete(It.IsAny<Guid>()))
            .Returns(Task.FromResult(true));

        _mainViewModel.LoadTasksCommand.Execute(null);

        _mainViewModel.SelectedTask = _mainViewModel.Tasks.LastOrDefault();

        _mainViewModel.SoftDeleteTaskCommand.Execute(null);

        Assert.Null(_mainViewModel.SelectedTask);
    }

    [Fact]
    public void MainViewModel_ShouldNotThrowSoftDeleteTask_WhenTaskNotSelected()
    {
        var exception = Record.Exception(() => _mainViewModel.SoftDeleteTaskCommand.Execute(null));

        Assert.Null(exception);
    }
}