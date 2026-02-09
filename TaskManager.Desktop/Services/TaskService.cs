using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManager.Desktop.Domain;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Desktop.Services;

public class TaskService : ITaskService
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly ITaskNamingService _taskNameService;
    private readonly ITaskRepository _taskRepository;

    public TaskService(ILogger<MainViewModel> logger, 
        ITaskNamingService taskNameService, 
        ITaskRepository taskRepository)
    {
        _logger = logger;
        _taskRepository = taskRepository;
        _taskNameService = taskNameService;
    }

    public async Task<IEnumerable<TaskModel>> GetTasksAsync()
    {
        var taskItems = await _taskRepository.GetTaskItemsAsync();

        _logger.LogDebug($"Loaded {taskItems.Count()} tasks");

        return taskItems.Select(TaskItemMapper.MapToTaskModel);
    }

    public async Task<TaskModel> CreateTaskAsync()
    {
        string defaultTitle = await _taskNameService.CreateDefaultTitleAsync();
        var taskItem = TaskItem.New(defaultTitle, DateTimeOffset.UtcNow);

        var updatedTaskItem = await _taskRepository.InsertAsync(taskItem);

        _logger.LogDebug($"Added task id={updatedTaskItem.Id} - '{updatedTaskItem.Title}'");

        return TaskItemMapper.MapToTaskModel(updatedTaskItem);
    }

    public async Task<bool> SoftDelete(Guid taskId)
    {
        var taskItem = await _taskRepository.GetByIdAsync(taskId);

        var succeed = await _taskRepository.SoftDeleteAsync(taskItem);

        _logger.LogDebug($"Soft delete task id={taskItem.Id} - {(succeed ? "Succeed" : "Failed")}");

        return succeed;
    }

    public async Task<bool> Update(TaskModel task)
    {
        var taskItem = await _taskRepository.GetByIdAsync(task.Id);

        taskItem.SetTitle(task.Title);
        taskItem.SetCompleted(task.IsCompleted);

        var succeed = await _taskRepository.UpdateAsync(taskItem);

        _logger.LogDebug($"Update task id={taskItem.Id} - {(succeed ? "Succeed" : "Failed")}");

        return succeed;
    }
}
