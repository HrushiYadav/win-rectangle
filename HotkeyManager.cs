using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Interop;

namespace RectangleWindows
{
    public class HotkeyManager : IDisposable
    {
        private readonly WindowManager _windowManager;
        private readonly AppSettings _settings;
        private readonly HwndSource? _source;
        private readonly Dictionary<int, WindowAction> _hotkeyMap = new Dictionary<int, WindowAction>();
        private int _nextHotkeyId = 1;
        private bool _disposed = false;

        public HotkeyManager(WindowManager windowManager, AppSettings settings)
        {
            _windowManager = windowManager;
            _settings = settings;
            
            // Get the main window handle for hotkey registration
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null)
            {
                var helper = new WindowInteropHelper(mainWindow);
                helper.EnsureHandle();
                _source = HwndSource.FromHwnd(helper.Handle);
                _source?.AddHook(WndProc);
            }
        }

        public void RegisterHotkeys()
        {
            if (_source == null) return;
            
            UnregisterHotkeys(); // Clear existing
            
            // Register default hotkeys
            RegisterHotkey(WindowAction.LeftHalf, MOD_CONTROL | MOD_ALT, Keys.Left);
            RegisterHotkey(WindowAction.RightHalf, MOD_CONTROL | MOD_ALT, Keys.Right);
            RegisterHotkey(WindowAction.TopHalf, MOD_CONTROL | MOD_ALT, Keys.Up);
            RegisterHotkey(WindowAction.BottomHalf, MOD_CONTROL | MOD_ALT, Keys.Down);
            RegisterHotkey(WindowAction.Maximize, MOD_CONTROL | MOD_ALT, Keys.Return);
            RegisterHotkey(WindowAction.Center, MOD_CONTROL | MOD_ALT, Keys.C);
            RegisterHotkey(WindowAction.FirstThird, MOD_CONTROL | MOD_ALT, Keys.D);
            RegisterHotkey(WindowAction.CenterThird, MOD_CONTROL | MOD_ALT, Keys.F);
            RegisterHotkey(WindowAction.LastThird, MOD_CONTROL | MOD_ALT, Keys.G);
            RegisterHotkey(WindowAction.Restore, MOD_CONTROL | MOD_ALT, Keys.Delete);
            RegisterHotkey(WindowAction.TopLeft, MOD_CONTROL | MOD_ALT, Keys.NumPad7);
            RegisterHotkey(WindowAction.TopRight, MOD_CONTROL | MOD_ALT, Keys.NumPad9);
            RegisterHotkey(WindowAction.BottomLeft, MOD_CONTROL | MOD_ALT, Keys.NumPad1);
            RegisterHotkey(WindowAction.BottomRight, MOD_CONTROL | MOD_ALT, Keys.NumPad3);
        }
        
        private void RegisterHotkey(WindowAction action, uint modifiers, Keys key)
        {
            if (_source == null) return;
            
            int id = _nextHotkeyId++;
            if (RegisterHotKey(_source.Handle, id, modifiers, key))
            {
                _hotkeyMap[id] = action;
            }
        }

        public void UnregisterHotkeys()
        {
            if (_source == null) return;

            foreach (int id in _hotkeyMap.Keys)
            {
                UnregisterHotKey(_source.Handle, id);
            }
            _hotkeyMap.Clear();
            _nextHotkeyId = 1;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (_hotkeyMap.TryGetValue(id, out WindowAction action))
                {
                    _windowManager.ExecuteAction(action);
                    handled = true;
                }
            }

            return IntPtr.Zero;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                UnregisterHotkeys();
                _source?.RemoveHook(WndProc);
                _disposed = true;
            }
        }

        #region Win32 API

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        #endregion
    }
}

