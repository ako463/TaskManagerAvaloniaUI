using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using Avalonia.VisualTree;
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
            this.taskDataGrid.SelectedItem = item;
            Dispatcher.UIThread.InvokeAsync((Action)(() => taskDataGrid.ScrollIntoView(item, null)), DispatcherPriority.ContextIdle);
        }
    }
}