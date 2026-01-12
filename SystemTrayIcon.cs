using System;
using System.Windows;
using System.Windows.Forms;

namespace RectangleWindows
{
    public class SystemTrayIcon : IDisposable
    {
        private NotifyIcon? _notifyIcon;
        private readonly WindowManager _windowManager;
        private readonly HotkeyManager _hotkeyManager;
        private readonly AppSettings _settings;
        private MultiWindowManager? _multiWindowManager;
        private bool _disposed = false;

        public SystemTrayIcon(WindowManager windowManager, HotkeyManager hotkeyManager, AppSettings settings)
        {
            _windowManager = windowManager;
            _hotkeyManager = hotkeyManager;
            _settings = settings;
            _multiWindowManager = new MultiWindowManager(windowManager, settings);

            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Text = "Rectangle Windows - Window Manager",
                Visible = true
            };

            CreateContextMenu();
        }

        private void CreateContextMenu()
        {
            if (_notifyIcon == null) return;

            ContextMenuStrip menu = new ContextMenuStrip();

            // Window Actions
            menu.Items.Add("Left Half", null, (s, e) => _windowManager.ExecuteAction(WindowAction.LeftHalf));
            menu.Items.Add("Right Half", null, (s, e) => _windowManager.ExecuteAction(WindowAction.RightHalf));
            menu.Items.Add("Top Half", null, (s, e) => _windowManager.ExecuteAction(WindowAction.TopHalf));
            menu.Items.Add("Bottom Half", null, (s, e) => _windowManager.ExecuteAction(WindowAction.BottomHalf));
            menu.Items.Add("-");
            menu.Items.Add("Top Left", null, (s, e) => _windowManager.ExecuteAction(WindowAction.TopLeft));
            menu.Items.Add("Top Right", null, (s, e) => _windowManager.ExecuteAction(WindowAction.TopRight));
            menu.Items.Add("Bottom Left", null, (s, e) => _windowManager.ExecuteAction(WindowAction.BottomLeft));
            menu.Items.Add("Bottom Right", null, (s, e) => _windowManager.ExecuteAction(WindowAction.BottomRight));
            menu.Items.Add("-");
            menu.Items.Add("Maximize", null, (s, e) => _windowManager.ExecuteAction(WindowAction.Maximize));
            menu.Items.Add("Center", null, (s, e) => _windowManager.ExecuteAction(WindowAction.Center));
            menu.Items.Add("Restore", null, (s, e) => _windowManager.ExecuteAction(WindowAction.Restore));
            menu.Items.Add("-");
            menu.Items.Add("Reverse All Windows", null, (s, e) => _windowManager.ExecuteAction(WindowAction.ReverseAll));
            menu.Items.Add("Tile All Windows", null, (s, e) => _multiWindowManager?.TileAll());
            menu.Items.Add("Cascade All Windows", null, (s, e) => _multiWindowManager?.CascadeAll());
            menu.Items.Add("Tile Active App", null, (s, e) => _multiWindowManager?.TileActiveApp());
            menu.Items.Add("Cascade Active App", null, (s, e) => _multiWindowManager?.CascadeActiveApp());
            menu.Items.Add("-");
            if (_settings.TodoModeEnabled)
            {
                menu.Items.Add("Left Todo", null, (s, e) => _windowManager.ExecuteAction(WindowAction.LeftTodo));
                menu.Items.Add("Right Todo", null, (s, e) => _windowManager.ExecuteAction(WindowAction.RightTodo));
                menu.Items.Add("-");
            }
            menu.Items.Add("Preferences...", null, (s, e) => ShowPreferences());
            menu.Items.Add("-");
            menu.Items.Add("Exit", null, (s, e) => 
            {
                System.Windows.Application.Current.Shutdown();
            });
            
        private void ShowPreferences()
        {
            var prefsWindow = new PreferencesWindow(_settings, _hotkeyManager);
            prefsWindow.Show();
        }

            _notifyIcon.ContextMenuStrip = menu;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _notifyIcon?.Dispose();
                _disposed = true;
            }
        }
    }
}

