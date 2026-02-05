using System;
namespace TaskManager.Desktop.Models;

public class TaskItem
{
    public Guid Id { get; set; }
    public int Index { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
