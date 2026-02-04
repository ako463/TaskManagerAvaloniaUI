using TaskManager.Desktop.Models;

namespace TaskManager.Tests.UnitTests;

public class TaskValidationTests
{
    [Fact]
    public void TaskValidation_ShouldNotHaveAnyErrors()
    {
        var unitUnderTest = new TaskModel()
        {
            Id = 1,
            Title = "Задача А",
            CreatedAt = DateTime.Now,
            IsCompleted = false,
            IsDeleted = false,
        };

        Assert.False(unitUnderTest.HasErrors);
    }

    [Fact]
    public void TaskValidation_ShouldHaveError_EmptyTaskTitle()
    {
        var unitUnderTest = new TaskModel()
        {
            Title = string.Empty,
        };

        Assert.True(unitUnderTest.HasErrors);
        Assert.Equal(unitUnderTest.GetErrors()?.Count(), 1);
        Assert.Equal("Input task title", unitUnderTest.GetErrors()?.First().ErrorMessage);
    }

    [Fact]
    public void TaskValidation_ShouldHaveError_TaskTitleExceedsMaximumLength()
    {
        string longTitle = new string('a', 110);

        var unitUnderTest = new TaskModel()
        {
            Title = longTitle,
        };

        Assert.True(unitUnderTest.HasErrors);
        Assert.Equal(unitUnderTest.GetErrors()?.Count(), 1);
        Assert.Equal("At least 1 and maximum 100 chars", unitUnderTest.GetErrors()?.First().ErrorMessage);
    }

}
