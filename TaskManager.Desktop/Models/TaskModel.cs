using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
namespace TaskManager.Desktop.Models;

public partial class TaskModel : ObservableValidator
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(AllowEmptyStrings = false, ErrorMessage = TaskItem.EmptyTitleError)]
    [Length(1, 100, ErrorMessage = TaskItem.LongTitleError)]
    private string? _title;

    [ObservableProperty]
    private bool _isCompleted;
    
    [ObservableProperty]
    private DateTimeOffset _createdAt;
    
    [ObservableProperty]
    private bool _isDeleted;
}
