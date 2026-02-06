using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManager.Desktop.Domain.Exceptions;
namespace TaskManager.Desktop.Domain;

public class TaskItem
{
    public const string EmptyTitleError = "Title cannot be empty";
    public const string LongTitleError = "Title must have at least 1 and maximum 100 chars";

    [Key]
    public Guid Id { get; private set; }
    public string? Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private TaskItem()
    {        
    }

    public static TaskItem New(string title, DateTimeOffset createdAt)
    {
        ValidateTitle(title);

        return new TaskItem()
        {
            Title = title,
            CreatedAt = createdAt,
            IsCompleted = false,
            IsDeleted = false,
        };
    }

    public void SetTitle(string? title)
    {
        ValidateTitle(title);

        Title = title;
    }

    public void SetCompleted(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }

    public void MarkAsDelete()
    {
        IsDeleted = true;
    }

    private static void ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidTaskItemException(EmptyTitleError);
        }
        else if (title.Length > 100)
        {
            throw new InvalidTaskItemException(LongTitleError);
        }
    }
}
