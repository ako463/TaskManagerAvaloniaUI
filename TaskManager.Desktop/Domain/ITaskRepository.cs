using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Desktop.Domain;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync();
    Task<IEnumerable<TaskItem>> GetTaskItemsAsync();
    Task<TaskItem> GetByIdAsync(Guid id);
    Task<TaskItem> InsertAsync(TaskItem taskItem);
    Task<bool> SoftDeleteAsync(TaskItem taskItem);
    Task<bool> UpdateAsync(TaskItem taskItem);
}
