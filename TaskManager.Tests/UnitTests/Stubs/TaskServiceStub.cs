using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;

namespace TaskManager.Tests.UnitTests.Stubs;

public class TaskServiceStub : ITaskService
{
    public Task<TaskModel> CreateTaskAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TaskModel>> GetTasksAsync()
    {
        return await Task.FromResult<IEnumerable<TaskModel>>(
            [
                new TaskModel()
                {
                    Title = "Fist named task",
                },
                new TaskModel()
                {
                    Title = "Task 1",
                },
                new TaskModel()
                {
                    Title = "Task 2",
                }
            ]);
    }

    public Task<bool> SoftDelete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(TaskModel task)
    {
        throw new NotImplementedException();
    }
}
