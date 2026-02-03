using TaskManager.Desktop.ViewModels;

namespace TaskManager.Tests.UnitTests;

public class MainViewModelTests
{
    [Fact]
    public void MainViewModel_ShouldReturnCorrectAmountOfTasks()
    {
        var unitUnderTest = new MainViewModel();

        Assert.Equal(unitUnderTest.FilteredTasks?.Count, 3);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullyAddNewTask()
    {
        var unitUnderTest = new MainViewModel();

        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Count, 4);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullySoftDeleteTask()
    {
        var unitUnderTest = new MainViewModel();

        unitUnderTest.SelectedTask = unitUnderTest.FilteredTasks.LastOrDefault();

        unitUnderTest.SoftDeleteTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.FilteredTasks?.Where(t => !t.IsDeleted).Count(), 2);
    }
}