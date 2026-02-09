using Moq;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Infrastructure;

namespace TaskManager.Tests.UnitTests;

public class TaskNamingServiceTests
{
    [Fact]
    public async Task TaskNamingService_ShouldGiveCorrectDefaultTitle()
    {
        var list = new List<TaskItem>()
        {
            TaskItem.New("Task 1", new DateTime(2026, 02, 04, 16, 20, 11)),
            TaskItem.New("Task 2", new DateTime(2026, 02, 04, 16, 20, 11)),
            TaskItem.New("Some other title", new DateTime(2026, 02, 04, 16, 20, 11)),
            TaskItem.New("Task 3", new DateTime(2026, 02, 04, 16, 20, 11)),
        };

        list.Last().MarkAsDelete();

        Mock<ITaskRepository> _taskRepositoryMock = new();
        _taskRepositoryMock.Setup(x => x.GetAllTaskItemsAsync()).Returns(Task.FromResult<IEnumerable<TaskItem>>(list));

        var namingService = new TaskNamingService(_taskRepositoryMock.Object);

        string newTitle = await namingService.CreateDefaultTitleAsync();

        Assert.Equal("Task 4", newTitle);
    }
}
