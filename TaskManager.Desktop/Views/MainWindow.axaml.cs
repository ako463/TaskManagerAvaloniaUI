using System;
using Avalonia.Controls;
using Avalonia.Threading;
using TaskManager.Desktop.ViewModels;

namespace TaskManager.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.TaskAdded += OnTaskAdded;

                // TODO: вызов команды через behavior
                viewModel.LoadTasksCommand.Execute(null);
            }
        }

        private void OnTaskAdded(object item)
        {
            // навигируем до новой строки
            this.taskDataGrid.SelectedItem = item;
            Dispatcher.UIThread.InvokeAsync((Action)(() => taskDataGrid.ScrollIntoView(item, null)), DispatcherPriority.ContextIdle);
        }
    }
}