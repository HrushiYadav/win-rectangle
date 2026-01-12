using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RectangleWindows
{
    public class MultiWindowManager
    {
        private readonly WindowManager _windowManager;
        private readonly AppSettings _settings;

        public MultiWindowManager(WindowManager windowManager, AppSettings settings)
        {
            _windowManager = windowManager;
            _settings = settings;
        }

        public void TileAll()
        {
            var windows = GetVisibleWindows();
            if (windows.Count == 0) return;

            Screen screen = Screen.PrimaryScreen;
            Rectangle bounds = screen.WorkingArea;
            
            int cols = (int)Math.Ceiling(Math.Sqrt(windows.Count));
            int rows = (int)Math.Ceiling((double)windows.Count / cols);
            
            int tileWidth = bounds.Width / cols;
            int tileHeight = bounds.Height / rows;
            
            int index = 0;
            foreach (var hwnd in windows)
            {
                int col = index % cols;
                int row = index / cols;
                
                int x = bounds.X + col * tileWidth;
                int y = bounds.Y + row * tileHeight;
                
                SetWindowPos(hwnd, IntPtr.Zero, x, y, tileWidth, tileHeight, SWP_NOZORDER | SWP_NOACTIVATE);
                index++;
            }
        }

        public void CascadeAll()
        {
            var windows = GetVisibleWindows();
            if (windows.Count == 0) return;

            Screen screen = Screen.PrimaryScreen;
            Rectangle bounds = screen.WorkingArea;
            
            int offset = 30;
            int x = bounds.X + offset;
            int y = bounds.Y + offset;
            int width = bounds.Width - (windows.Count * offset);
            int height = bounds.Height - (windows.Count * offset);
            
            foreach (var hwnd in windows)
            {
                GetWindowRect(hwnd, out RECT rect);
                int currentWidth = rect.Right - rect.Left;
                int currentHeight = rect.Bottom - rect.Top;
                
                SetWindowPos(hwnd, IntPtr.Zero, x, y, 
                    Math.Min(width, currentWidth), 
                    Math.Min(height, currentHeight), 
                    SWP_NOZORDER | SWP_NOACTIVATE);
                
                x += offset;
                y += offset;
            }
        }

        public void TileActiveApp()
        {
            IntPtr activeWindow = GetForegroundWindow();
            if (activeWindow == IntPtr.Zero) return;

            uint processId;
            GetWindowThreadProcessId(activeWindow, out processId);
            
            var windows = GetVisibleWindows()
                .Where(hwnd =>
                {
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    return pid == processId;
                })
                .ToList();
            
            if (windows.Count == 0) return;

            Screen screen = Screen.PrimaryScreen;
            Rectangle bounds = screen.WorkingArea;
            
            int cols = (int)Math.Ceiling(Math.Sqrt(windows.Count));
            int rows = (int)Math.Ceiling((double)windows.Count / cols);
            
            int tileWidth = bounds.Width / cols;
            int tileHeight = bounds.Height / rows;
            
            int index = 0;
            foreach (var hwnd in windows)
            {
                int col = index % cols;
                int row = index / cols;
                
                int x = bounds.X + col * tileWidth;
                int y = bounds.Y + row * tileHeight;
                
                SetWindowPos(hwnd, IntPtr.Zero, x, y, tileWidth, tileHeight, SWP_NOZORDER | SWP_NOACTIVATE);
                index++;
            }
        }

        public void CascadeActiveApp()
        {
            IntPtr activeWindow = GetForegroundWindow();
            if (activeWindow == IntPtr.Zero) return;

            uint processId;
            GetWindowThreadProcessId(activeWindow, out processId);
            
            var windows = GetVisibleWindows()
                .Where(hwnd =>
                {
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    return pid == processId;
                })
                .ToList();
            
            if (windows.Count == 0) return;

            Screen screen = Screen.PrimaryScreen;
            Rectangle bounds = screen.WorkingArea;
            
            int offset = 30;
            int x = bounds.X + offset;
            int y = bounds.Y + offset;
            int width = bounds.Width - (windows.Count * offset);
            int height = bounds.Height - (windows.Count * offset);
            
            foreach (var hwnd in windows)
            {
                GetWindowRect(hwnd, out RECT rect);
                int currentWidth = rect.Right - rect.Left;
                int currentHeight = rect.Bottom - rect.Top;
                
                SetWindowPos(hwnd, IntPtr.Zero, x, y, 
                    Math.Min(width, currentWidth), 
                    Math.Min(height, currentHeight), 
                    SWP_NOZORDER | SWP_NOACTIVATE);
                
                x += offset;
                y += offset;
            }
        }

        private List<IntPtr> GetVisibleWindows()
        {
            var windows = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd) && GetParent(hWnd) == IntPtr.Zero)
                {
                    // Check if window has title
                    int length = GetWindowTextLength(hWnd);
                    if (length > 0)
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
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        #endregion
    }
}

