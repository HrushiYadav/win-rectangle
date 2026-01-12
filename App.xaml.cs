using System.Windows;

namespace RectangleWindows
{
    public partial class App : Application
    {
        private SystemTrayIcon? _systemTrayIcon;
        private WindowManager? _windowManager;
        private HotkeyManager? _hotkeyManager;
        private DragSnapManager? _dragSnapManager;
        private AppSettings? _settings;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Hide the main window
            MainWindow.WindowState = WindowState.Minimized;
            MainWindow.ShowInTaskbar = false;
            MainWindow.Hide();

            // Load settings
            _settings = AppSettings.Load();
            
            // Set launch on login
            LaunchOnLogin.SetEnabled(_settings.LaunchOnLogin);
            
            // Initialize Todo Manager
            TodoManager.Initialize(_settings);

            // Initialize components
            _windowManager = new WindowManager(_settings);
            _hotkeyManager = new HotkeyManager(_windowManager, _settings);
            _dragSnapManager = new DragSnapManager(_windowManager, _settings);
            _systemTrayIcon = new SystemTrayIcon(_windowManager, _hotkeyManager, _settings);

            // Register hotkeys
            _hotkeyManager.RegisterHotkeys();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _hotkeyManager?.UnregisterHotkeys();
            _dragSnapManager?.Dispose();
            _systemTrayIcon?.Dispose();
            base.OnExit(e);
        }
    }
}

