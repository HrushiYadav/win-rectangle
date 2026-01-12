using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RectangleWindows
{
    public partial class FootprintWindow : Window
    {
        public FootprintWindow()
        {
            InitializeComponent();
            this.Opacity = 0.3;
        }

        public void ShowAt(Rect rect, AppSettings settings)
        {
            this.Left = rect.Left;
            this.Top = rect.Top;
            this.Width = rect.Width;
            this.Height = rect.Height;
            
            // Apply settings
            this.Opacity = settings.FootprintAlpha;
            FootprintBorder.BorderThickness = new Thickness(settings.FootprintBorderWidth);
            
            if (settings.FootprintFade)
            {
                var fadeIn = new DoubleAnimation(0, settings.FootprintAlpha, TimeSpan.FromMilliseconds(200));
                this.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            
            if (!this.IsVisible)
            {
                this.Show();
            }
        }

        public void HideFootprint()
        {
            if (this.IsVisible)
            {
                this.Hide();
            }
        }
    }
}

