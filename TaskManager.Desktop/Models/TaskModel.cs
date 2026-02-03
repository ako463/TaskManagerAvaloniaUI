using System;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Desktop.Models;

public partial class TaskModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private bool _isCompleted;
    
    [ObservableProperty]
    private DateTime _createdAt;
    
    [ObservableProperty]
    private bool _isDeleted;
}
