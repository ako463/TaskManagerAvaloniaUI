using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Desktop.ViewModels;
using TaskManager.Desktop.Views;
using TaskManager.Desktop.DI.Extensions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Avalonia.Threading;
using System.Threading.Tasks;
using System;
using Avalonia.Controls.Notifications;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;
using Castle.Core.Configuration;

namespace TaskManager.Desktop
{
    public partial class App : Application
    {
        private WindowNotificationManager? _manager;
        private ServiceProvider? _services;

        public ILogger<App>? Logger { get; private set; }

        public override void Initialize()
        {
            var configuration = Infrastructure.ConfigurationProvider.Provide();

            var collection = new ServiceCollection();
            collection.AddCommonServices(configuration!);

            SetupGlobalExceptionHandlers();

            _services = collection.BuildServiceProvider();

            Logger = _services.GetRequiredService<ILogger<App>>();
            Logger.LogInformation("Application starting...");

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var viewModel = _services?.GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = viewModel,
                };

                var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
                _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }

        private void SetupGlobalExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            Dispatcher.UIThread.UnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;

            LogException(exception, "AppDomain");
            ShowErrorNotification(exception);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception, "UI Thread");
            ShowErrorNotification(e.Exception);

            e.Handled = true;
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception, "TaskScheduler");
            ShowErrorNotification(e.Exception);

            e.SetObserved();
        }

        private void ShowErrorNotification(Exception ex)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var notification = new Notification("Error", ex.Message, NotificationType.Error);
                
                _manager?.Show(notification);
            });
        }

        private void LogException(Exception exception, string source)
        {
            Logger?.LogError(exception, source);
        }
    }
}