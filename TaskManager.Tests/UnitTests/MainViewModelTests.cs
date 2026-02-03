using TaskManager.Desktop.ViewModels;

namespace TaskManager.Tests.UnitTests;

public class MainViewModelTests
{
    [Fact]
    public void MainViewModel_ShouldReturnCorrectAmountOfItems()
    {
        var unitUnderTest = new MainViewModel();

        Assert.Equal(unitUnderTest.Tasks?.Count, 4);
    }

    [Fact]
    public void MainViewModel_ShouldSuccessfullyAddNewTaskItem()
    {
        var unitUnderTest = new MainViewModel();

        unitUnderTest.AddTaskCommand.Execute(null);

        Assert.Equal(unitUnderTest.Tasks?.Count, 5);
    }
}