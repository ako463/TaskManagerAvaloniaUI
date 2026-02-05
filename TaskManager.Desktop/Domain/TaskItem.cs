using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaskManager.Desktop.Domain;

public class TaskItem : IValidatableObject
{
    public const string EmptyTitleError = "Title cannot be empty";
    public const string LongTitleError = "Title must have at least 1 and maximum 100 chars";

    [Key]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            yield return new ValidationResult(EmptyTitleError);
        }
        else if (Title.Length > 100)
        {
            yield return new ValidationResult(LongTitleError);
        }
    }
}
