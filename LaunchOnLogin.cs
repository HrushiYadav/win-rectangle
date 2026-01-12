using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;

namespace RectangleWindows
{
    public static class LaunchOnLogin
    {
        private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "RectangleWindows";

        public static bool IsEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey))
                {
                    return key?.GetValue(AppName) != null;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void SetEnabled(bool enabled)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
                {
                    if (key == null)
                    {
                        Registry.CurrentUser.CreateSubKey(RegistryKey);
                        using (RegistryKey newKey = Registry.CurrentUser.OpenSubKey(RegistryKey, true))
                        {
                            if (enabled)
                            {
                                string exePath = Assembly.GetExecutingAssembly().Location;
                                newKey?.SetValue(AppName, exePath);
                            }
                            else
                            {
                                newKey?.DeleteValue(AppName, false);
                            }
                        }
                    }
                    else
                    {
                        if (enabled)
                        {
                            string exePath = Assembly.GetExecutingAssembly().Location;
                            key.SetValue(AppName, exePath);
                        }
                        else
                        {
                            key.DeleteValue(AppName, false);
                        }
                    }
                }
            }
            catch
            {
                // Silently fail
            }
        }
    }
}

