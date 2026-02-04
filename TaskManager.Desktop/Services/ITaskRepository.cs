using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetTaskItems();
    Task<bool> Add(TaskItem taskItem);
    Task<bool> SoftDelete(TaskItem taskItem);
    Task<bool> Update(TaskItem taskItem);
}
