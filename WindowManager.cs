using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;

namespace RectangleWindows
{
    public class WindowManager
    {
        private readonly WindowHistory _history = new WindowHistory();
        private readonly AppSettings _settings;

        public WindowManager(AppSettings settings)
        {
            _settings = settings;
        }

        public void ExecuteAction(WindowAction action)
        {
            // Handle multi-window actions
            if (action == WindowAction.ReverseAll)
            {
                ReverseAllWindows();
                return;
            }
            
            if (action == WindowAction.LeftTodo || action == WindowAction.RightTodo)
            {
                TodoManager.ExecuteTodoAction(action);
                return;
            }

            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
            {
                SystemSounds.Beep.Play();
                return;
            }

            // Check if window is valid
            if (!IsWindow(hwnd) || !IsWindowVisible(hwnd))
            {
                SystemSounds.Beep.Play();
                return;
            }

            // Get current window rect
            GetWindowRect(hwnd, out RECT currentRect);

            // Handle NextDisplay/PreviousDisplay
            if (action == WindowAction.NextDisplay || action == WindowAction.PreviousDisplay)
            {
                MoveToNextDisplay(hwnd, action == WindowAction.NextDisplay);
                return;
            }

            // Get the screen that contains the window
            Rectangle screenBounds = GetScreenBounds(hwnd);
            
            if (action == WindowAction.Restore)
            {
                if (_history.TryGetRestoreRect(hwnd, out WindowManager.RECT restoreRect))
                {
                    SetWindowPos(hwnd, IntPtr.Zero, restoreRect.Left, restoreRect.Top,
                        restoreRect.Right - restoreRect.Left, restoreRect.Bottom - restoreRect.Top,
                        SWP_NOZORDER | SWP_NOACTIVATE);
                    _history.ClearLastAction(hwnd);
                }
                return;
            }

            // Save restore rect if needed
            if (!_history.HasRestoreRect(hwnd))
            {
                _history.SaveRestoreRect(hwnd, currentRect);
            }

            // Check if app should be ignored
            if (ShouldIgnoreWindow(hwnd))
            {
                return;
            }

            // Check for subsequent execution mode (cycle sizes)
            WindowAction actualAction = CheckSubsequentExecutionMode(action, hwnd);

            // Calculate new position
            RECT newRect = WindowCalculation.Calculate(actualAction, currentRect, screenBounds, _settings);

            // Apply the new position
            SetWindowPos(hwnd, IntPtr.Zero, newRect.Left, newRect.Top,
                newRect.Right - newRect.Left, newRect.Bottom - newRect.Top,
                SWP_NOZORDER | SWP_NOACTIVATE);
            
            // Move cursor if enabled
            if (_settings.MoveCursorWithWindow || (_settings.MoveCursorAcrossDisplays && action == WindowAction.NextDisplay || action == WindowAction.PreviousDisplay))
            {
                int centerX = (newRect.Left + newRect.Right) / 2;
                int centerY = (newRect.Top + newRect.Bottom) / 2;
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(centerX, centerY);
            }

            // Record action
            _history.RecordAction(hwnd, actualAction, newRect);
        }
        
        private WindowAction CheckSubsequentExecutionMode(WindowAction action, IntPtr hwnd)
        {
            if (!_settings.SubsequentExecutionModeCycle)
                return action;
            
            var lastAction = _history.GetLastAction(hwnd);
            if (lastAction == null || lastAction.Value.action != action)
                return action;
            
            // Cycle through sizes for repeated commands
            return action switch
            {
                WindowAction.LeftHalf => WindowAction.FirstTwoThirds,
                WindowAction.FirstTwoThirds => WindowAction.FirstThird,
                WindowAction.FirstThird => WindowAction.LeftHalf,
                WindowAction.RightHalf => WindowAction.LastTwoThirds,
                WindowAction.LastTwoThirds => WindowAction.LastThird,
                WindowAction.LastThird => WindowAction.RightHalf,
                _ => action
            };
        }
        
        private void ReverseAllWindows()
        {
            IntPtr activeWindow = GetForegroundWindow();
            if (activeWindow == IntPtr.Zero) return;
            
            Screen currentScreen = Screen.FromHandle(activeWindow);
            Rectangle screenBounds = currentScreen.Bounds;
            
            var windows = GetAllWindowsOnScreen(currentScreen);
            
            foreach (var hwnd in windows)
            {
                GetWindowRect(hwnd, out RECT rect);
                int width = rect.Right - rect.Left;
                int offsetFromLeft = rect.Left - screenBounds.Left;
                
                int newX = screenBounds.Right - offsetFromLeft - width;
                
                SetWindowPos(hwnd, IntPtr.Zero, newX, rect.Top, width, rect.Bottom - rect.Top,
                    SWP_NOZORDER | SWP_NOACTIVATE);
            }
        }
        
        private void MoveToNextDisplay(IntPtr hwnd, bool next)
        {
            Screen currentScreen = Screen.FromHandle(hwnd);
            Screen[] allScreens = Screen.AllScreens;
            
            if (allScreens.Length <= 1)
            {
                SystemSounds.Beep.Play();
                return;
            }
            
            int currentIndex = Array.IndexOf(allScreens, currentScreen);
            int targetIndex;
            
            if (next)
            {
                targetIndex = (currentIndex + 1) % allScreens.Length;
            }
            else
            {
                targetIndex = (currentIndex - 1 + allScreens.Length) % allScreens.Length;
            }
            
            Screen targetScreen = allScreens[targetIndex];
            Rectangle targetBounds = targetScreen.WorkingArea;
            
            GetWindowRect(hwnd, out RECT currentRect);
            int width = currentRect.Right - currentRect.Left;
            int height = currentRect.Bottom - currentRect.Top;
            
            // Center on new screen
            int newX = targetBounds.X + (targetBounds.Width - width) / 2;
            int newY = targetBounds.Y + (targetBounds.Height - height) / 2;
            
            SetWindowPos(hwnd, IntPtr.Zero, newX, newY, width, height,
                SWP_NOZORDER | SWP_NOACTIVATE);
        }
        
        private System.Collections.Generic.List<IntPtr> GetAllWindowsOnScreen(Screen screen)
        {
            var windows = new System.Collections.Generic.List<IntPtr>();
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

        private bool ShouldIgnoreWindow(IntPtr hwnd)
        {
            if (_settings.IgnoredApps == null || _settings.IgnoredApps.Length == 0)
                return false;
            
            try
            {
                uint processId;
                GetWindowThreadProcessId(hwnd, out processId);
                
                var process = System.Diagnostics.Process.GetProcessById((int)processId);
                string processName = process.ProcessName.ToLower();
                
                foreach (string ignoredApp in _settings.IgnoredApps)
                {
                    if (processName.Contains(ignoredApp.ToLower()) || 
                        process.MainModule?.FileName?.ToLower().Contains(ignoredApp.ToLower()) == true)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // If we can't check, don't ignore
            }
            
            return false;
        }

        private Rectangle GetScreenBounds(IntPtr hwnd)
        {
            RECT windowRect;
            GetWindowRect(hwnd, out windowRect);
            
            int centerX = (windowRect.Left + windowRect.Right) / 2;
            int centerY = (windowRect.Top + windowRect.Bottom) / 2;

            return Screen.GetBounds(new System.Drawing.Point(centerX, centerY));
        }

        #region Win32 API

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        #endregion
    }

}

