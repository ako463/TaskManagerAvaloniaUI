using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;

namespace TaskManager.Tests.UnitTests;

public class TaskServiceStub : ITaskService
{
    public async Task<TaskModel> Add(TaskModel task)
    {
        return await Task.FromResult(task);
    }

    public async Task<IEnumerable<TaskModel>> GetTasks()
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

    public async Task<bool> SoftDelete(TaskModel task)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(TaskModel task)
    {
        throw new NotImplementedException();
    }
}
