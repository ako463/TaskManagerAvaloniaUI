using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskModel>> GetTasks();
    Task<TaskModel> Add(TaskModel task);
    Task<bool> SoftDelete(TaskModel task);
    Task<bool> Update(TaskModel task);
}
