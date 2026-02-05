using System.ComponentModel.DataAnnotations;
using TaskManager.Desktop.Models;

namespace TaskManager.Tests.UnitTests;

public class TaskValidationTests
{
    private ValidationContext? _validationContext;

    [Fact]
    public void TaskModel_ShouldNotHaveAnyErrors()
    {
        var unitUnderTest = new TaskModel()
        {
            Index = 1,
            Title = "Task A",
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            IsDeleted = false,
        };

        Assert.False(unitUnderTest.HasErrors);
    }

    [Fact]
    public void TaskModel_ShouldHaveError_EmptyTaskTitle()
    {
        var unitUnderTest = new TaskModel()
        {
            Title = string.Empty,
        };

        Assert.True(unitUnderTest.HasErrors);
        Assert.Equal(unitUnderTest.GetErrors()?.Count(), 1);
        Assert.Equal(TaskItem.EmptyTitleError, unitUnderTest.GetErrors()?.First().ErrorMessage);
    }

    [Fact]
    public void TaskModel_ShouldHaveError_TaskTitleExceedsMaximumLength()
    {
        string longTitle = new string('a', 110);

        var unitUnderTest = new TaskModel()
        {
            Title = longTitle,
        };

        Assert.True(unitUnderTest.HasErrors);
        Assert.Equal(unitUnderTest.GetErrors()?.Count(), 1);
        Assert.Equal(TaskItem.LongTitleError, unitUnderTest.GetErrors()?.First().ErrorMessage);
    }

    [Fact]
    public void TaskItem_ShouldNotHaveAnyErrors()
    {
        var unitUnderTest = new TaskItem()
        {
            Title = "Task A",
        };

        _validationContext = new ValidationContext(unitUnderTest);

        var validationresults = unitUnderTest.Validate(_validationContext);

        Assert.Equal(0, validationresults?.Count());
        Assert.Equal(ValidationResult.Success, validationresults?.FirstOrDefault());
    }

    [Fact]
    public void TaskItem_ShouldHaveError_EmptyTaskTitle()
    {
        var unitUnderTest = new TaskItem()
        {
            Title = string.Empty,
        };
        
        _validationContext = new ValidationContext(unitUnderTest);

        var validationresults = unitUnderTest.Validate(_validationContext);

        Assert.Equal(1, validationresults?.Count());
        Assert.Equal(TaskItem.EmptyTitleError, validationresults?.FirstOrDefault()?.ErrorMessage);
    }

    [Fact]
    public void TaskItem_ShouldHaveError_TaskTitleExceedsMaximumLength()
    {
        string longTitle = new string('a', 110);

        var unitUnderTest = new TaskItem()
        {
            Title = longTitle,
        };

        _validationContext = new ValidationContext(unitUnderTest);

        var validationresults = unitUnderTest.Validate(_validationContext);

        Assert.Equal(1, validationresults?.Count());
        Assert.Equal(TaskItem.LongTitleError, validationresults?.FirstOrDefault()?.ErrorMessage);
    }
}
