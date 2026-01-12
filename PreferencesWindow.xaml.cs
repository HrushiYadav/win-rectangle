using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RectangleWindows
{
    public partial class PreferencesWindow : Window
    {
        private readonly AppSettings _settings;
        private readonly HotkeyManager _hotkeyManager;
        private bool _settingsChanged = false;

        public PreferencesWindow(AppSettings settings, HotkeyManager hotkeyManager)
        {
            InitializeComponent();
            _settings = settings;
            _hotkeyManager = hotkeyManager;
            
            LoadSettings();
            LoadShortcuts();
            LoadIgnoredApps();
        }

        private void LoadSettings()
        {
            GapSizeSlider.Value = _settings.GapSize;
            GapSizeLabel.Text = $"{_settings.GapSize} px";
            
            SizeOffsetTextBox.Text = _settings.SizeOffset.ToString();
            WidthStepSizeTextBox.Text = _settings.WidthStepSize.ToString();
            
            AlmostMaximizeWidthSlider.Value = _settings.AlmostMaximizeWidth;
            AlmostMaximizeWidthLabel.Text = _settings.AlmostMaximizeWidth.ToString("F1");
            
            AlmostMaximizeHeightSlider.Value = _settings.AlmostMaximizeHeight;
            AlmostMaximizeHeightLabel.Text = _settings.AlmostMaximizeHeight.ToString("F1");
            
            WindowSnappingCheckBox.IsChecked = _settings.WindowSnapping;
            MoveCursorCheckBox.IsChecked = _settings.MoveCursorWithWindow;
            UseCursorScreenDetectionCheckBox.IsChecked = _settings.UseCursorScreenDetection;
            
            // Load new settings if controls exist
            if (LaunchOnLoginCheckBox != null)
                LaunchOnLoginCheckBox.IsChecked = _settings.LaunchOnLogin;
            if (SubsequentExecutionCycleCheckBox != null)
                SubsequentExecutionCycleCheckBox.IsChecked = _settings.SubsequentExecutionModeCycle;
            if (TodoModeCheckBox != null)
                TodoModeCheckBox.IsChecked = _settings.TodoModeEnabled;
        }

        private void LoadShortcuts()
        {
            var shortcuts = new List<ShortcutItem>
            {
                new ShortcutItem { ActionName = "Left Half", Action = WindowAction.LeftHalf },
                new ShortcutItem { ActionName = "Right Half", Action = WindowAction.RightHalf },
                new ShortcutItem { ActionName = "Top Half", Action = WindowAction.TopHalf },
                new ShortcutItem { ActionName = "Bottom Half", Action = WindowAction.BottomHalf },
                new ShortcutItem { ActionName = "Maximize", Action = WindowAction.Maximize },
                new ShortcutItem { ActionName = "Center", Action = WindowAction.Center },
                new ShortcutItem { ActionName = "Restore", Action = WindowAction.Restore },
                new ShortcutItem { ActionName = "Top Left", Action = WindowAction.TopLeft },
                new ShortcutItem { ActionName = "Top Right", Action = WindowAction.TopRight },
                new ShortcutItem { ActionName = "Bottom Left", Action = WindowAction.BottomLeft },
                new ShortcutItem { ActionName = "Bottom Right", Action = WindowAction.BottomRight },
            };
            
            ShortcutsDataGrid.ItemsSource = shortcuts;
        }

        private void LoadIgnoredApps()
        {
            if (_settings.IgnoredApps != null)
            {
                foreach (string app in _settings.IgnoredApps)
                {
                    IgnoredAppsListBox.Items.Add(app);
                }
            }
        }

        private void GapSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (GapSizeLabel != null)
            {
                int value = (int)e.NewValue;
                GapSizeLabel.Text = $"{value} px";
                _settings.GapSize = value;
                _settingsChanged = true;
            }
        }

        private void SizeOffsetTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(SizeOffsetTextBox.Text, out int value))
            {
                _settings.SizeOffset = value;
                _settingsChanged = true;
            }
        }

        private void WidthStepSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(WidthStepSizeTextBox.Text, out int value))
            {
                _settings.WidthStepSize = value;
                _settingsChanged = true;
            }
        }

        private void AlmostMaximizeWidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AlmostMaximizeWidthLabel != null)
            {
                float value = (float)e.NewValue;
                AlmostMaximizeWidthLabel.Text = value.ToString("F1");
                _settings.AlmostMaximizeWidth = value;
                _settingsChanged = true;
            }
        }

        private void AlmostMaximizeHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AlmostMaximizeHeightLabel != null)
            {
                float value = (float)e.NewValue;
                AlmostMaximizeHeightLabel.Text = value.ToString("F1");
                _settings.AlmostMaximizeHeight = value;
                _settingsChanged = true;
            }
        }

        private void WindowSnappingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.WindowSnapping = true;
            _settingsChanged = true;
        }

        private void WindowSnappingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.WindowSnapping = false;
            _settingsChanged = true;
        }

        private void MoveCursorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.MoveCursorWithWindow = true;
            _settingsChanged = true;
        }

        private void MoveCursorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.MoveCursorWithWindow = false;
            _settingsChanged = true;
        }

        private void UseCursorScreenDetectionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.UseCursorScreenDetection = true;
            _settingsChanged = true;
        }

        private void UseCursorScreenDetectionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.UseCursorScreenDetection = false;
            _settingsChanged = true;
        }

        private void ChangeShortcut_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag as ShortcutItem;
            if (item != null)
            {
                var dialog = new ShortcutRecorderDialog();
                if (dialog.ShowDialog() == true)
                {
                    item.ShortcutDisplay = dialog.ShortcutString;
                    ShortcutsDataGrid.Items.Refresh();
                    _settingsChanged = true;
                }
            }
        }

        private void AddIgnoredApp_Click(object sender, RoutedEventArgs e)
        {
            string appName = NewIgnoredAppTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(appName) && !IgnoredAppsListBox.Items.Contains(appName))
            {
                IgnoredAppsListBox.Items.Add(appName);
                NewIgnoredAppTextBox.Clear();
                _settingsChanged = true;
            }
        }

        private void RemoveIgnoredApp_Click(object sender, RoutedEventArgs e)
        {
            if (IgnoredAppsListBox.SelectedItem != null)
            {
                IgnoredAppsListBox.Items.Remove(IgnoredAppsListBox.SelectedItem);
                _settingsChanged = true;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Save ignored apps
            _settings.IgnoredApps = IgnoredAppsListBox.Items.Cast<string>().ToArray();
            
            if (_settingsChanged)
            {
                _settings.Save();
            }
            
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LaunchOnLoginCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.LaunchOnLogin = true;
            LaunchOnLogin.SetEnabled(true);
            _settingsChanged = true;
        }

        private void LaunchOnLoginCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.LaunchOnLogin = false;
            LaunchOnLogin.SetEnabled(false);
            _settingsChanged = true;
        }

        private void SubsequentExecutionCycleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.SubsequentExecutionModeCycle = true;
            _settingsChanged = true;
        }

        private void SubsequentExecutionCycleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.SubsequentExecutionModeCycle = false;
            _settingsChanged = true;
        }

        private void TodoModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.TodoModeEnabled = true;
            _settingsChanged = true;
        }

        private void TodoModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.TodoModeEnabled = false;
            _settingsChanged = true;
        }

        private void TodoSidebarWidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TodoSidebarWidthTextBox.Text, out int value))
            {
                _settings.TodoSidebarWidth = value;
                _settingsChanged = true;
            }
        }

        private void TodoSidebarRightCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _settings.TodoSidebarRight = true;
            _settingsChanged = true;
        }

        private void TodoSidebarRightCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _settings.TodoSidebarRight = false;
            _settingsChanged = true;
        }
    }

    public class ShortcutItem
    {
        public string ActionName { get; set; } = "";
        public WindowAction Action { get; set; }
        public string ShortcutDisplay { get; set; } = "Not set";
    }
}

