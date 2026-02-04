using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskModel>> GetTasks()
    {
        var taskItems = await _taskRepository.GetTaskItems();

        return taskItems.Select(TaskItemMapper.MapToTaskModel);
    }

    public async Task<bool> Add(TaskModel task)
    {
        return await _taskRepository.Add(TaskItemMapper.MapFromTaskModel(task));
    }

    public async Task<bool> SoftDelete(TaskModel task)
    {   
        return await _taskRepository.SoftDelete(TaskItemMapper.MapFromTaskModel(task));
    }

    public async Task<bool> Update(TaskModel task)
    {
        return await _taskRepository.Update(TaskItemMapper.MapFromTaskModel(task));
    }
}
