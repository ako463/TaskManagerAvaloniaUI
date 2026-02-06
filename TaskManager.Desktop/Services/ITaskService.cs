using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskModel>> GetTasksAsync();
    Task<TaskModel> CreateTaskAsync();
    Task<bool> SoftDelete(Guid id);
    Task<bool> Update(TaskModel task);
}
