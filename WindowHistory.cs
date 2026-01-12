using System;
using System.Collections.Generic;

namespace RectangleWindows
{
    public class WindowHistory
    {
        private readonly Dictionary<IntPtr, WindowManager.RECT> _restoreRects = new Dictionary<IntPtr, WindowManager.RECT>();
        private readonly Dictionary<IntPtr, (WindowAction action, WindowManager.RECT rect)> _lastActions = new Dictionary<IntPtr, (WindowAction, WindowManager.RECT)>();

        public void SaveRestoreRect(IntPtr hwnd, WindowManager.RECT rect)
        {
            _restoreRects[hwnd] = rect;
        }

        public bool HasRestoreRect(IntPtr hwnd)
        {
            return _restoreRects.ContainsKey(hwnd);
        }

        public bool TryGetRestoreRect(IntPtr hwnd, out WindowManager.RECT rect)
        {
            return _restoreRects.TryGetValue(hwnd, out rect);
        }

        public void RecordAction(IntPtr hwnd, WindowAction action, WindowManager.RECT rect)
        {
            _lastActions[hwnd] = (action, rect);
        }

        public void ClearLastAction(IntPtr hwnd)
        {
            _lastActions.Remove(hwnd);
        }
        
        public (WindowAction action, WindowManager.RECT rect)? GetLastAction(IntPtr hwnd)
        {
            if (_lastActions.TryGetValue(hwnd, out var action))
            {
                return action;
            }
            return null;
        }
    }
}

