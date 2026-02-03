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

        private ObservableCollection<TaskModel> _filteredItems = new();

        private ObservableCollection<TaskModel> _allTasks { get; } = new()
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

        public MainViewModel()
        {
            ApplyFilter();
        }

        public ObservableCollection<TaskModel> FilteredTasks
        {
            get => _filteredItems;
            set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        private void ApplyFilter()
        {
            var filtered = _allTasks.Where(item => !item.IsDeleted).ToList();
            FilteredTasks = new ObservableCollection<TaskModel>(filtered);
        }

        [RelayCommand]
        private void AddTask()
        {
            _allTasks.Add(
                new TaskModel()
                {
                    Id = _allTasks.LastOrDefault()?.Id ?? 0 + 1,
                    CreatedAt = DateTime.Now,
                    IsCompleted = false,
                    IsDeleted = false,
                });

            ApplyFilter();
        }

        [RelayCommand]
        private void SoftDeleteTask()
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsDeleted = true;

                ApplyFilter();
            }
        }
    }
}
