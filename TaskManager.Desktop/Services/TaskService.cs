using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Desktop.Domain;
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
        var taskItems = await _taskRepository.GetTaskItemsAsync();

        return taskItems.Select(TaskItemMapper.MapToTaskModel);
    }

    public async Task<TaskModel> Add(TaskModel task)
    {
        var updatedTaskItem = await _taskRepository.AddAsync(TaskItemMapper.MapFromTaskModel(task));

        return TaskItemMapper.MapToTaskModel(updatedTaskItem);
    }

    public async Task<bool> SoftDelete(TaskModel task)
    {   
        return await _taskRepository.SoftDeleteAsync(TaskItemMapper.MapFromTaskModel(task));
    }

    public async Task<bool> Update(TaskModel task)
    {
        return await _taskRepository.UpdateAsync(TaskItemMapper.MapFromTaskModel(task));
    }
}
