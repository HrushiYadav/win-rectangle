using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace RectangleWindows
{
    public partial class ShortcutRecorderDialog : Window
    {
        public string ShortcutString { get; private set; } = "";
        private Keys _recordedKey = Keys.None;
        private bool _ctrlPressed = false;
        private bool _altPressed = false;
        private bool _shiftPressed = false;
        private bool _winPressed = false;

        public ShortcutRecorderDialog()
        {
            InitializeComponent();
            this.KeyDown += ShortcutRecorderDialog_KeyDown;
            this.KeyUp += ShortcutRecorderDialog_KeyUp;
            this.PreviewKeyDown += ShortcutRecorderDialog_PreviewKeyDown;
        }

        private void ShortcutRecorderDialog_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void ShortcutRecorderDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _ctrlPressed = true;
            else if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
                _altPressed = true;
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                _shiftPressed = true;
            else if (e.Key == Key.LWin || e.Key == Key.RWin)
                _winPressed = true;
            else if (e.Key != Key.None)
            {
                _recordedKey = (Keys)KeyInterop.VirtualKeyFromKey(e.Key);
                UpdateDisplay();
            }
        }

        private void ShortcutRecorderDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                _ctrlPressed = false;
            else if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
                _altPressed = false;
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                _shiftPressed = false;
            else if (e.Key == Key.LWin || e.Key == Key.RWin)
                _winPressed = false;
        }

        private void UpdateDisplay()
        {
            var parts = new System.Collections.Generic.List<string>();
            if (_ctrlPressed) parts.Add("Ctrl");
            if (_altPressed) parts.Add("Alt");
            if (_shiftPressed) parts.Add("Shift");
            if (_winPressed) parts.Add("Win");
            if (_recordedKey != Keys.None)
                parts.Add(_recordedKey.ToString());

            ShortcutTextBlock.Text = parts.Count > 0 ? string.Join(" + ", parts) : "Waiting for keys...";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (_recordedKey != Keys.None)
            {
                var parts = new System.Collections.Generic.List<string>();
                if (_ctrlPressed) parts.Add("Ctrl");
                if (_altPressed) parts.Add("Alt");
                if (_shiftPressed) parts.Add("Shift");
                if (_winPressed) parts.Add("Win");
                parts.Add(_recordedKey.ToString());
                
                ShortcutString = string.Join(" + ", parts);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please press a key combination.", "Invalid Shortcut", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

