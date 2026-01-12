using System;
using System.IO;
using System.Text.Json;

namespace RectangleWindows
{
    public class AppSettings
    {
        public int GapSize { get; set; } = 0;
        public int SizeOffset { get; set; } = 30;
        public int WidthStepSize { get; set; } = 30;
        public int MoveStepSize { get; set; } = 50;
        public float AlmostMaximizeWidth { get; set; } = 0.9f;
        public float AlmostMaximizeHeight { get; set; } = 0.9f;
        public bool WindowSnapping { get; set; } = true;
        public bool LaunchOnLogin { get; set; } = false;
        public bool MoveCursorWithWindow { get; set; } = false;
        public bool UseCursorScreenDetection { get; set; } = false;
        public bool MoveCursorAcrossDisplays { get; set; } = false;
        public string[] IgnoredApps { get; set; } = Array.Empty<string>();
        public int SpecifiedWidth { get; set; } = 1680;
        public int SpecifiedHeight { get; set; } = 1050;
        public bool SubsequentExecutionModeCycle { get; set; } = false;
        public bool SubsequentExecutionModeTraverseDisplays { get; set; } = false;
        public bool TodoModeEnabled { get; set; } = false;
        public string TodoAppName { get; set; } = "";
        public int TodoSidebarWidth { get; set; } = 400;
        public bool TodoSidebarRight { get; set; } = true;
        public float FootprintAlpha { get; set; } = 0.3f;
        public int FootprintBorderWidth { get; set; } = 2;
        public bool FootprintFade { get; set; } = false;
        
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RectangleWindows",
            "settings.json");
        
        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    return settings ?? new AppSettings();
                }
            }
            catch
            {
                // If loading fails, return defaults
            }
            
            return new AppSettings();
        }
        
        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(SettingsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsPath, json);
            }
            catch
            {
                // If saving fails, silently continue
            }
        }
    }
}

