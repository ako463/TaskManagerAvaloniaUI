using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Desktop.Domain;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetTaskItemsAsync();
    Task<TaskItem> AddAsync(TaskItem taskItem);
    Task<bool> SoftDeleteAsync(TaskItem taskItem);
    Task<bool> UpdateAsync(TaskItem taskItem);
}
