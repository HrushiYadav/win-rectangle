using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RectangleWindows
{
    public static class TodoManager
    {
        private static IntPtr? _todoWindowHandle = null;
        private static Screen? _todoScreen = null;
        private static AppSettings? _settings;

        public static void Initialize(AppSettings settings)
        {
            _settings = settings;
        }

        public static void ExecuteTodoAction(WindowAction action)
        {
            if (_settings == null || !_settings.TodoModeEnabled)
                return;

            if (action == WindowAction.LeftTodo || action == WindowAction.RightTodo)
            {
                MoveAllWindows();
            }
        }

        public static void SetTodoWindow(IntPtr hwnd)
        {
            _todoWindowHandle = hwnd;
            _todoScreen = Screen.FromHandle(hwnd);
        }

        public static bool IsTodoWindow(IntPtr hwnd)
        {
            return _todoWindowHandle.HasValue && _todoWindowHandle.Value == hwnd;
        }

        private static void MoveAllWindows()
        {
            if (_settings == null || !_todoScreen.HasValue || !_todoWindowHandle.HasValue)
                return;

            Screen screen = _todoScreen.Value;
            Rectangle bounds = screen.WorkingArea;
            
            int sidebarWidth = _settings.TodoSidebarWidth;
            bool sidebarRight = _settings.TodoSidebarRight;
            
            // Get all windows on the screen
            var windows = GetAllWindowsOnScreen(screen)
                .Where(hwnd => hwnd != _todoWindowHandle.Value)
                .ToList();
            
            // Position todo window as sidebar
            Rectangle sidebarRect;
            if (sidebarRight)
            {
                sidebarRect = new Rectangle(
                    bounds.Right - sidebarWidth,
                    bounds.Top,
                    sidebarWidth,
                    bounds.Height);
            }
            else
            {
                sidebarRect = new Rectangle(
                    bounds.Left,
                    bounds.Top,
                    sidebarWidth,
                    bounds.Height);
            }
            
            SetWindowPos(_todoWindowHandle.Value, IntPtr.Zero,
                sidebarRect.Left, sidebarRect.Top,
                sidebarRect.Width, sidebarRect.Height,
                SWP_NOZORDER | SWP_NOACTIVATE);
            
            // Shift other windows away from sidebar
            Rectangle workArea;
            if (sidebarRight)
            {
                workArea = new Rectangle(
                    bounds.Left,
                    bounds.Top,
                    bounds.Width - sidebarWidth,
                    bounds.Height);
            }
            else
            {
                workArea = new Rectangle(
                    bounds.Left + sidebarWidth,
                    bounds.Top,
                    bounds.Width - sidebarWidth,
                    bounds.Height);
            }
            
            foreach (var hwnd in windows)
            {
                GetWindowRect(hwnd, out RECT rect);
                
                // If window overlaps with sidebar area, move it
                if (sidebarRight && rect.Right > workArea.Right)
                {
                    int newX = workArea.Right - (rect.Right - rect.Left);
                    SetWindowPos(hwnd, IntPtr.Zero, newX, rect.Top,
                        rect.Right - rect.Left, rect.Bottom - rect.Top,
                        SWP_NOZORDER | SWP_NOACTIVATE);
                }
                else if (!sidebarRight && rect.Left < workArea.Left)
                {
                    SetWindowPos(hwnd, IntPtr.Zero, workArea.Left, rect.Top,
                        rect.Right - rect.Left, rect.Bottom - rect.Top,
                        SWP_NOZORDER | SWP_NOACTIVATE);
                }
            }
        }

        private static List<IntPtr> GetAllWindowsOnScreen(Screen screen)
        {
            var windows = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd) && GetParent(hWnd) == IntPtr.Zero)
                {
                    Screen windowScreen = Screen.FromHandle(hWnd);
                    if (windowScreen == screen)
                    {
                        windows.Add(hWnd);
                    }
                }
                return true;
            }, IntPtr.Zero);
            return windows;
        }

        #region Win32 API

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        #endregion
    }
}

