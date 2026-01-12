using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows;

namespace RectangleWindows
{
    public class DragSnapManager : IDisposable
    {
        private readonly WindowManager _windowManager;
        private readonly AppSettings _settings;
        private LowLevelMouseHook? _mouseHook;
        private bool _isDragging = false;
        private IntPtr _draggedWindow = IntPtr.Zero;
        private RECT _initialWindowRect;
        private Point _initialMousePos;
        private FootprintWindow? _footprintWindow;
        private WindowAction? _currentSnapAction;
        private bool _disposed = false;

        public DragSnapManager(WindowManager windowManager, AppSettings settings)
        {
            _windowManager = windowManager;
            _settings = settings;
            _footprintWindow = new FootprintWindow();
            
            if (_settings.WindowSnapping)
            {
                EnableSnapping();
            }
        }

        public void EnableSnapping()
        {
            if (_mouseHook == null)
            {
                _mouseHook = new LowLevelMouseHook();
                _mouseHook.MouseDown += MouseHook_MouseDown;
                _mouseHook.MouseMove += MouseHook_MouseMove;
                _mouseHook.MouseUp += MouseHook_MouseUp;
            }
        }

        public void DisableSnapping()
        {
            _mouseHook?.Dispose();
            _mouseHook = null;
        }

        private void MouseHook_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IntPtr hwnd = WindowFromPoint(new POINT { X = e.X, Y = e.Y });
                if (hwnd != IntPtr.Zero && IsValidWindow(hwnd))
                {
                    _isDragging = true;
                    _draggedWindow = hwnd;
                    GetWindowRect(hwnd, out _initialWindowRect);
                    _initialMousePos = new Point(e.X, e.Y);
                }
            }
        }

        private void MouseHook_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedWindow != IntPtr.Zero)
            {
                // Check if window is being dragged to screen edge
                CheckSnapAreas(e.X, e.Y);
            }
        }

        private void MouseHook_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedWindow != IntPtr.Zero && e.Button == MouseButtons.Left)
            {
                // Final snap check
                CheckSnapAreas(e.X, e.Y, true);
                _isDragging = false;
                _draggedWindow = IntPtr.Zero;
            }
        }

        private void CheckSnapAreas(int mouseX, int mouseY, bool snapNow = false)
        {
            if (_draggedWindow == IntPtr.Zero) return;

            Screen screen = Screen.FromPoint(new System.Drawing.Point(mouseX, mouseY));
            Rectangle bounds = screen.Bounds;
            
            int margin = 20; // Snap margin in pixels
            WindowAction? snapAction = null;
            Rectangle snapRect = Rectangle.Empty;

            // Check edges
            if (mouseX <= bounds.Left + margin)
            {
                if (mouseY <= bounds.Top + margin)
                {
                    snapAction = WindowAction.TopLeft;
                    snapRect = new Rectangle(bounds.Left, bounds.Top, bounds.Width / 2, bounds.Height / 2);
                }
                else if (mouseY >= bounds.Bottom - margin)
                {
                    snapAction = WindowAction.BottomLeft;
                    snapRect = new Rectangle(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                }
                else
                {
                    snapAction = WindowAction.LeftHalf;
                    snapRect = new Rectangle(bounds.Left, bounds.Top, bounds.Width / 2, bounds.Height);
                }
            }
            else if (mouseX >= bounds.Right - margin)
            {
                if (mouseY <= bounds.Top + margin)
                {
                    snapAction = WindowAction.TopRight;
                    snapRect = new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top, bounds.Width / 2, bounds.Height / 2);
                }
                else if (mouseY >= bounds.Bottom - margin)
                {
                    snapAction = WindowAction.BottomRight;
                    snapRect = new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                }
                else
                {
                    snapAction = WindowAction.RightHalf;
                    snapRect = new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top, bounds.Width / 2, bounds.Height);
                }
            }
            else if (mouseY <= bounds.Top + margin)
            {
                snapAction = WindowAction.TopHalf;
                snapRect = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height / 2);
            }
            else if (mouseY >= bounds.Bottom - margin)
            {
                snapAction = WindowAction.BottomHalf;
                snapRect = new Rectangle(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Width, bounds.Height / 2);
            }

            if (snapAction.HasValue)
            {
                _currentSnapAction = snapAction;
                
                // Show footprint
                if (_footprintWindow != null && !snapRect.IsEmpty)
                {
                    var rect = new System.Windows.Rect(snapRect.Left, snapRect.Top, snapRect.Width, snapRect.Height);
                    _footprintWindow.ShowAt(rect, _settings);
                }
                
                if (snapNow)
                {
                    // Execute snap
                    _windowManager.ExecuteAction(snapAction.Value);
                    _footprintWindow?.HideFootprint();
                    _currentSnapAction = null;
                }
            }
            else
            {
                // Hide footprint if not snapping
                _footprintWindow?.HideFootprint();
                _currentSnapAction = null;
            }
        }

        private bool IsValidWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return false;
            if (!IsWindow(hwnd)) return false;
            if (!IsWindowVisible(hwnd)) return false;
            
            // Check if it's a top-level window
            if (GetParent(hwnd) != IntPtr.Zero) return false;
            
            return true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                DisableSnapping();
                _footprintWindow?.Close();
                _disposed = true;
            }
        }

        #region Win32 API

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        #endregion
    }

    public class LowLevelMouseHook : IDisposable
    {
        private const int WH_MOUSE_LL = 14;
        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private bool _disposed = false;

        public event EventHandler<MouseEventArgs>? MouseDown;
        public event EventHandler<MouseEventArgs>? MouseMove;
        public event EventHandler<MouseEventArgs>? MouseUp;

        public LowLevelMouseHook()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule?.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                
                MouseEventArgs? args = null;
                if (wParam == (IntPtr)WM_LBUTTONDOWN)
                {
                    args = new MouseEventArgs(MouseButtons.Left, 1, hookStruct.pt.x, hookStruct.pt.y, 0);
                    MouseDown?.Invoke(this, args);
                }
                else if (wParam == (IntPtr)WM_MOUSEMOVE)
                {
                    args = new MouseEventArgs(MouseButtons.None, 0, hookStruct.pt.x, hookStruct.pt.y, 0);
                    MouseMove?.Invoke(this, args);
                }
                else if (wParam == (IntPtr)WM_LBUTTONUP)
                {
                    args = new MouseEventArgs(MouseButtons.Left, 1, hookStruct.pt.x, hookStruct.pt.y, 0);
                    MouseUp?.Invoke(this, args);
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                UnhookWindowsHookEx(_hookID);
                _disposed = true;
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 0x0200;
    }
}

