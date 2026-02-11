using Moq;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Infrastructure;

namespace TaskManager.Tests.UnitTests;

public class TaskNamingServiceTests
{
    [Fact]
    public async Task TaskNamingService_ShouldGiveCorrectDefaultTitle()
    {
        var date = new DateTime(2026, 02, 04, 16, 20, 11);
        var list = Enumerable.Range(3, 10).Select(i => TaskItem.New($"Task {i}", date)).ToList();

        list.Last().MarkAsDelete();
        
        list.Add(TaskItem.New("Some other title", date));

        Mock<ITaskRepository> _taskRepositoryMock = new();
        _taskRepositoryMock.Setup(x => x.GetAllTaskItemsAsync()).Returns(Task.FromResult<IEnumerable<TaskItem>>(list));

        var namingService = new TaskNamingService(_taskRepositoryMock.Object);

        string newTitle = await namingService.CreateDefaultTitleAsync();

        Assert.Equal("Task 13", newTitle);
    }
}
