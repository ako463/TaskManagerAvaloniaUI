using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Desktop.Models;

namespace TaskManager.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private TaskModel? _selectedTask;

        public ObservableCollection<TaskModel> Tasks { get; } = new()
        {
            new TaskModel()
            {
                Id = 1,
                Title = "Задача А",
                CreatedAt = DateTime.Now,
                IsCompleted = false,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Id = 2,
                Title = "Задача Б",
                CreatedAt = DateTime.Now,
                IsCompleted = true,
                IsDeleted = false,
            },
            new TaskModel()
            {
                Id = 3,
                Title = "Задача В",
                CreatedAt = DateTime.Now,
                IsCompleted = false,
                IsDeleted = true,
            },
            new TaskModel()
            {
                Id = 4,
                Title = "Задача Г",
                CreatedAt = DateTime.Now,
                IsCompleted = false,
                IsDeleted = false,
            }
        };

        [RelayCommand]
        private void AddTask()
        {
            Tasks.Add(
                new TaskModel()
                {
                    Id = Tasks.LastOrDefault()?.Id ?? 0 + 1,
                    CreatedAt = DateTime.Now,
                    IsCompleted = false,
                    IsDeleted = false,
                });
        }
    }
}
