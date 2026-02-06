using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Desktop.Models;
using TaskManager.Desktop.Services;

namespace TaskManager.Desktop.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;

        public delegate void TaskAddedHandler(object item);
        public event TaskAddedHandler? TaskAdded;

        [ObservableProperty]
        private TaskModel? _selectedTask;

        private ObservableCollection<TaskModel> _tasks = new();

        public MainViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public ObservableCollection<TaskModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        [RelayCommand]
        private async Task LoadTasks()
        {
            await ReloadTasks();
        }

        [RelayCommand]
        private async Task AddTask()
        {
            var newTask = await _taskService.CreateTaskAsync();

            newTask.PropertyChanged += OnTaskChanged;

            _tasks.Add(newTask);

            TaskAdded?.Invoke(newTask);
        }

        [RelayCommand]
        private async Task SoftDeleteTask()
        {
            if (SelectedTask != null)
            {
                if (await _taskService.SoftDelete(SelectedTask.Id))
                {
                    SelectedTask = null;

                    foreach (var task in _tasks)
                    {
                        task.PropertyChanged -= OnTaskChanged;
                    }

                    await ReloadTasks();
                }
            }
        }

        private void OnTaskChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TaskModel task
                && task.HasErrors == false
                && (e.PropertyName == nameof(task.Title)
                || e.PropertyName == nameof(task.IsCompleted)))
            {
                _taskService.Update(task);
            }
        }

        private async Task ReloadTasks()
        {
            _tasks.Clear();

            var tasks = await _taskService.GetTasksAsync();
            foreach (var task in tasks)
            {
                task.PropertyChanged += OnTaskChanged;

                _tasks.Add(task);
            }
        }
    }
}
