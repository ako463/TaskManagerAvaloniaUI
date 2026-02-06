using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.Services;

public class TaskService : ITaskService
{
    private readonly ITaskNamingService _taskNameService;
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskNamingService taskNameService, ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        _taskNameService = taskNameService;
    }

    public async Task<IEnumerable<TaskModel>> GetTasksAsync()
    {
        var taskItems = await _taskRepository.GetTaskItemsAsync();

        return taskItems.Select(TaskItemMapper.MapToTaskModel);
    }

    public async Task<TaskModel> CreateTaskAsync()
    {
        string defaultTtitle = await _taskNameService.CreateDefaultTitleAsync();
        var taskItem = TaskItem.New(defaultTtitle, DateTimeOffset.UtcNow);

        var updatedTaskItem = await _taskRepository.InsertAsync(taskItem);

        return TaskItemMapper.MapToTaskModel(updatedTaskItem);
    }

    public async Task<bool> SoftDelete(Guid taskId)
    {
        var taskItem = await _taskRepository.GetByIdAsync(taskId)
           ?? throw new InvalidOperationException($"Task {taskId} not found");

        return await _taskRepository.SoftDeleteAsync(taskItem);
    }

    public async Task<bool> Update(TaskModel task)
    {
        var taskItem = await _taskRepository.GetByIdAsync(task.Id)
           ?? throw new InvalidOperationException($"Task {task.Id} not found");

        taskItem.SetTitle(task.Title);
        taskItem.SetCompleted(task.IsCompleted);

        return await _taskRepository.UpdateAsync(taskItem);
    }
}
