using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RectangleWindows
{
    public class HotkeyBinding
    {
        public Keys Key { get; set; }
        public bool Control { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public bool Win { get; set; }
        
        public uint Modifiers
        {
            get
            {
                uint mods = 0;
                if (Control) mods |= 0x0002; // MOD_CONTROL
                if (Alt) mods |= 0x0001;     // MOD_ALT
                if (Shift) mods |= 0x0004;   // MOD_SHIFT
                if (Win) mods |= 0x0008;     // MOD_WIN
                return mods;
            }
        }
        
        public static HotkeyBinding FromString(string binding)
        {
            var result = new HotkeyBinding();
            if (string.IsNullOrEmpty(binding)) return result;
            
            string[] parts = binding.Split('+');
            foreach (string part in parts)
            {
                string trimmed = part.Trim().ToLower();
                switch (trimmed)
                {
                    case "ctrl":
                    case "control":
                        result.Control = true;
                        break;
                    case "alt":
                        result.Alt = true;
                        break;
                    case "shift":
                        result.Shift = true;
                        break;
                    case "win":
                    case "windows":
                        result.Win = true;
                        break;
                    default:
                        if (Enum.TryParse<Keys>(trimmed, true, out Keys key))
                        {
                            result.Key = key;
                        }
                        break;
                }
            }
            return result;
        }
        
        public override string ToString()
        {
            var parts = new List<string>();
            if (Control) parts.Add("Ctrl");
            if (Alt) parts.Add("Alt");
            if (Shift) parts.Add("Shift");
            if (Win) parts.Add("Win");
            parts.Add(Key.ToString());
            return string.Join(" + ", parts);
        }
    }
}

