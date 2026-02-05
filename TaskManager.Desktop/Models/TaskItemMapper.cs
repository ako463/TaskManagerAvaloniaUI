namespace TaskManager.Desktop.Models;

public static class TaskItemMapper
{
    public static TaskModel MapToTaskModel(TaskItem taskItem)
    {
        return new TaskModel()
        {
            Id = taskItem.Id,
            Index = taskItem.Index,
            CreatedAt = taskItem.CreatedAt,
            IsCompleted = taskItem.IsCompleted,
            IsDeleted = taskItem.IsDeleted,
            Title = taskItem.Title
        };
    }

    public static TaskItem MapFromTaskModel(TaskModel taskModel)
    {
        return new TaskItem()
        {
            Id = taskModel.Id,
            Index = taskModel.Index,
            CreatedAt = taskModel.CreatedAt,
            IsCompleted = taskModel.IsCompleted,
            IsDeleted = taskModel.IsDeleted,
            Title = taskModel.Title!
        };
    }
}
