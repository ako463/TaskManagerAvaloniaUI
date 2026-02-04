using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;

namespace TaskManager.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly string _initialTaskTitle = "Task ";
        private readonly ITaskService _taskService;

        public delegate void TaskAddedHandler(object item);
        public event TaskAddedHandler? TaskAdded;

        [ObservableProperty]
        private TaskModel? _selectedTask;

        private ObservableCollection<TaskModel> _filteredItems = new();
        private ObservableCollection<TaskModel> _allTasks = new();

        public MainViewModel(ITaskService taskService)
        {
            _taskService = taskService;
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
        private async Task LoadTasks()
        {
            var tasks = await _taskService.GetTasks();
            foreach (var task in tasks)
            {
                _allTasks.Add(task);
            }

            ApplyFilter();
        }

        [RelayCommand]
        private void AddTask()
        {
            // TODO: убрать Title когда добавлю фокус ячейке для изменения названия задачи
            
            string taskTitlePattern = @$"{_initialTaskTitle}(\d+)";
            int nextTitleId =  _allTasks.Count(t => Regex.IsMatch(t.Title ?? "", taskTitlePattern)) + 1;

            var task = new TaskModel()
            {
                Id = (_allTasks.LastOrDefault()?.Id ?? 0) + 1,
                Title = $"{_initialTaskTitle}{nextTitleId}",
                CreatedAt = DateTime.Now,
                IsCompleted = false,
                IsDeleted = false,
            };

            task.PropertyChanged += OnTaskChanged;

            _allTasks.Add(task);

            ApplyFilter();

            TaskAdded?.Invoke(task);
        }

        private void OnTaskChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TaskModel task && task.HasErrors == false)
            {
                // TODO: запись в репу
            }
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
