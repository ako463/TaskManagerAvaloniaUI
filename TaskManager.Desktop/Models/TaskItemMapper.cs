using TaskManager.Desktop.Domain;

namespace TaskManager.Desktop.Models;

public static class TaskItemMapper
{
    public static TaskModel MapToTaskModel(TaskItem taskItem)
    {
        return new TaskModel()
        {
            Id = taskItem.Id,
            CreatedAt = taskItem.CreatedAt,
            IsCompleted = taskItem.IsCompleted,
            IsDeleted = taskItem.IsDeleted,
            Title = taskItem.Title
        };
    }
}
