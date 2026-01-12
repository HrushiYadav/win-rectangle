using System;
using System.Drawing;

namespace RectangleWindows
{
    public static class WindowCalculation
    {
        public static WindowManager.RECT Calculate(WindowAction action, WindowManager.RECT currentRect, Rectangle screenBounds, AppSettings settings)
        {
            int width = currentRect.Right - currentRect.Left;
            int height = currentRect.Bottom - currentRect.Top;
            int screenWidth = screenBounds.Width;
            int screenHeight = screenBounds.Height;
            int screenX = screenBounds.X;
            int screenY = screenBounds.Y;
            
            // Apply gaps if enabled
            int gapSize = settings.GapSize;
            int gapLeft = gapSize;
            int gapRight = gapSize;
            int gapTop = gapSize;
            int gapBottom = gapSize;
            
            // Adjust screen bounds for gaps
            int workX = screenX + gapLeft;
            int workY = screenY + gapTop;
            int workWidth = screenWidth - gapLeft - gapRight;
            int workHeight = screenHeight - gapTop - gapBottom;

            return action switch
            {
                // Halves
                WindowAction.LeftHalf => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight
                },
                WindowAction.RightHalf => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                WindowAction.TopHalf => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.BottomHalf => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                WindowAction.CenterHalf => new WindowManager.RECT
                {
                    Left = workX + workWidth / 4,
                    Top = workY,
                    Right = workX + 3 * workWidth / 4,
                    Bottom = workY + workHeight
                },
                
                // Quarters
                WindowAction.TopLeft => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopRight => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.BottomLeft => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomRight => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Thirds
                WindowAction.FirstThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.CenterThird => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.LastThird => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                WindowAction.FirstTwoThirds => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.CenterTwoThirds => new WindowManager.RECT
                {
                    Left = workX + workWidth / 6,
                    Top = workY,
                    Right = workX + 5 * workWidth / 6,
                    Bottom = workY + workHeight
                },
                WindowAction.LastTwoThirds => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Vertical Thirds
                WindowAction.TopVerticalThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.MiddleVerticalThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + 2 * workHeight / 3
                },
                WindowAction.BottomVerticalThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                WindowAction.TopVerticalTwoThirds => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + 2 * workHeight / 3
                },
                WindowAction.BottomVerticalTwoThirds => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Corner Thirds
                WindowAction.TopLeftThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.TopRightThird => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.BottomLeftThird => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomRightThird => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Fourths
                WindowAction.FirstFourth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 4,
                    Bottom = workY + workHeight
                },
                WindowAction.SecondFourth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 4,
                    Top = workY,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight
                },
                WindowAction.ThirdFourth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY,
                    Right = workX + 3 * workWidth / 4,
                    Bottom = workY + workHeight
                },
                WindowAction.LastFourth => new WindowManager.RECT
                {
                    Left = workX + 3 * workWidth / 4,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                WindowAction.FirstThreeFourths => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + 3 * workWidth / 4,
                    Bottom = workY + workHeight
                },
                WindowAction.CenterThreeFourths => new WindowManager.RECT
                {
                    Left = workX + workWidth / 8,
                    Top = workY,
                    Right = workX + 7 * workWidth / 8,
                    Bottom = workY + workHeight
                },
                WindowAction.LastThreeFourths => new WindowManager.RECT
                {
                    Left = workX + workWidth / 4,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Sixths
                WindowAction.TopLeftSixth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopCenterSixth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopRightSixth => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.BottomLeftSixth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomCenterSixth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY + workHeight / 2,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomRightSixth => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Eighths
                WindowAction.TopLeftEighth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 4,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopCenterLeftEighth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 4,
                    Top = workY,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopCenterRightEighth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY,
                    Right = workX + 3 * workWidth / 4,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.TopRightEighth => new WindowManager.RECT
                {
                    Left = workX + 3 * workWidth / 4,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 2
                },
                WindowAction.BottomLeftEighth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth / 4,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomCenterLeftEighth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 4,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth / 2,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomCenterRightEighth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 2,
                    Top = workY + workHeight / 2,
                    Right = workX + 3 * workWidth / 4,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomRightEighth => new WindowManager.RECT
                {
                    Left = workX + 3 * workWidth / 4,
                    Top = workY + workHeight / 2,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Ninths
                WindowAction.TopLeftNinth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.TopCenterNinth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.TopRightNinth => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight / 3
                },
                WindowAction.MiddleLeftNinth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + workHeight / 3,
                    Right = workX + workWidth / 3,
                    Bottom = workY + 2 * workHeight / 3
                },
                WindowAction.MiddleCenterNinth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY + workHeight / 3,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + 2 * workHeight / 3
                },
                WindowAction.MiddleRightNinth => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY + workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + 2 * workHeight / 3
                },
                WindowAction.BottomLeftNinth => new WindowManager.RECT
                {
                    Left = workX,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomCenterNinth => new WindowManager.RECT
                {
                    Left = workX + workWidth / 3,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + 2 * workWidth / 3,
                    Bottom = workY + workHeight
                },
                WindowAction.BottomRightNinth => new WindowManager.RECT
                {
                    Left = workX + 2 * workWidth / 3,
                    Top = workY + 2 * workHeight / 3,
                    Right = workX + workWidth,
                    Bottom = workY + workHeight
                },
                
                // Size Adjustments
                WindowAction.Larger => CalculateSizeChange(currentRect, screenBounds, settings.SizeOffset, settings.SizeOffset),
                WindowAction.Smaller => CalculateSizeChange(currentRect, screenBounds, -settings.SizeOffset, -settings.SizeOffset),
                WindowAction.LargerWidth => CalculateSizeChange(currentRect, screenBounds, settings.WidthStepSize, 0),
                WindowAction.SmallerWidth => CalculateSizeChange(currentRect, screenBounds, -settings.WidthStepSize, 0),
                WindowAction.LargerHeight => CalculateSizeChange(currentRect, screenBounds, 0, settings.SizeOffset),
                WindowAction.SmallerHeight => CalculateSizeChange(currentRect, screenBounds, 0, -settings.SizeOffset),
                
                // Movement
                WindowAction.MoveLeft => CalculateMove(currentRect, screenBounds, -settings.MoveStepSize, 0),
                WindowAction.MoveRight => CalculateMove(currentRect, screenBounds, settings.MoveStepSize, 0),
                WindowAction.MoveUp => CalculateMove(currentRect, screenBounds, 0, -settings.MoveStepSize),
                WindowAction.MoveDown => CalculateMove(currentRect, screenBounds, 0, settings.MoveStepSize),
                
                // Special
                WindowAction.Maximize => new WindowManager.RECT
                {
                    Left = screenX,
                    Top = screenY,
                    Right = screenX + screenWidth,
                    Bottom = screenY + screenHeight
                },
                WindowAction.AlmostMaximize => CalculateAlmostMaximize(currentRect, screenBounds, settings),
                WindowAction.MaximizeHeight => new WindowManager.RECT
                {
                    Left = currentRect.Left,
                    Top = workY,
                    Right = currentRect.Right,
                    Bottom = workY + workHeight
                },
                WindowAction.Center => new WindowManager.RECT
                {
                    Left = screenX + (screenWidth - width) / 2,
                    Top = screenY + (screenHeight - height) / 2,
                    Right = screenX + (screenWidth - width) / 2 + width,
                    Bottom = screenY + (screenHeight - height) / 2 + height
                },
                WindowAction.CenterProminently => CalculateCenterProminently(currentRect, screenBounds),
                
                // Advanced Resizing
                WindowAction.DoubleHeightUp => CalculateDoubleHalve(currentRect, screenBounds, 0, 2, true),
                WindowAction.DoubleHeightDown => CalculateDoubleHalve(currentRect, screenBounds, 0, 2, false),
                WindowAction.DoubleWidthLeft => CalculateDoubleHalve(currentRect, screenBounds, 2, 0, true),
                WindowAction.DoubleWidthRight => CalculateDoubleHalve(currentRect, screenBounds, 2, 0, false),
                WindowAction.HalveHeightUp => CalculateDoubleHalve(currentRect, screenBounds, 0, 0.5f, true),
                WindowAction.HalveHeightDown => CalculateDoubleHalve(currentRect, screenBounds, 0, 0.5f, false),
                WindowAction.HalveWidthLeft => CalculateDoubleHalve(currentRect, screenBounds, 0.5f, 0, true),
                WindowAction.HalveWidthRight => CalculateDoubleHalve(currentRect, screenBounds, 0.5f, 0, false),
                
                // Specified (custom size)
                WindowAction.Specified => CalculateSpecified(currentRect, screenBounds, settings),
                
                _ => currentRect
            };
        }
        
        private static WindowManager.RECT CalculateSpecified(WindowManager.RECT currentRect, Rectangle screenBounds, AppSettings settings)
        {
            int width = settings.SpecifiedWidth > 0 ? settings.SpecifiedWidth : 1680;
            int height = settings.SpecifiedHeight > 0 ? settings.SpecifiedHeight : 1050;
            
            // Center on screen
            int x = screenBounds.X + (screenBounds.Width - width) / 2;
            int y = screenBounds.Y + (screenBounds.Height - height) / 2;
            
            return new WindowManager.RECT
            {
                Left = x,
                Top = y,
                Right = x + width,
                Bottom = y + height
            };
        }
        
        private static WindowManager.RECT CalculateSizeChange(WindowManager.RECT currentRect, Rectangle screenBounds, int widthDelta, int heightDelta)
        {
            int newWidth = Math.Max(100, (currentRect.Right - currentRect.Left) + widthDelta);
            int newHeight = Math.Max(100, (currentRect.Bottom - currentRect.Top) + heightDelta);
            
            int centerX = (currentRect.Left + currentRect.Right) / 2;
            int centerY = (currentRect.Top + currentRect.Bottom) / 2;
            
            return new WindowManager.RECT
            {
                Left = centerX - newWidth / 2,
                Top = centerY - newHeight / 2,
                Right = centerX + newWidth / 2,
                Bottom = centerY + newHeight / 2
            };
        }
        
        private static WindowManager.RECT CalculateMove(WindowManager.RECT currentRect, Rectangle screenBounds, int xDelta, int yDelta)
        {
            int width = currentRect.Right - currentRect.Left;
            int height = currentRect.Bottom - currentRect.Top;
            
            return new WindowManager.RECT
            {
                Left = currentRect.Left + xDelta,
                Top = currentRect.Top + yDelta,
                Right = currentRect.Left + xDelta + width,
                Bottom = currentRect.Top + yDelta + height
            };
        }
        
        private static WindowManager.RECT CalculateAlmostMaximize(WindowManager.RECT currentRect, Rectangle screenBounds, AppSettings settings)
        {
            float widthFactor = settings.AlmostMaximizeWidth > 0 ? settings.AlmostMaximizeWidth : 0.9f;
            float heightFactor = settings.AlmostMaximizeHeight > 0 ? settings.AlmostMaximizeHeight : 0.9f;
            
            int newWidth = (int)(screenBounds.Width * widthFactor);
            int newHeight = (int)(screenBounds.Height * heightFactor);
            
            return new WindowManager.RECT
            {
                Left = screenBounds.X + (screenBounds.Width - newWidth) / 2,
                Top = screenBounds.Y + (screenBounds.Height - newHeight) / 2,
                Right = screenBounds.X + (screenBounds.Width - newWidth) / 2 + newWidth,
                Bottom = screenBounds.Y + (screenBounds.Height - newHeight) / 2 + newHeight
            };
        }
        
        private static WindowManager.RECT CalculateCenterProminently(WindowManager.RECT currentRect, Rectangle screenBounds)
        {
            int width = currentRect.Right - currentRect.Left;
            int height = currentRect.Bottom - currentRect.Top;
            
            // Center prominently uses 80% of screen
            int prominentWidth = (int)(screenBounds.Width * 0.8);
            int prominentHeight = (int)(screenBounds.Height * 0.8);
            
            int finalWidth = Math.Min(width, prominentWidth);
            int finalHeight = Math.Min(height, prominentHeight);
            
            return new WindowManager.RECT
            {
                Left = screenBounds.X + (screenBounds.Width - finalWidth) / 2,
                Top = screenBounds.Y + (screenBounds.Height - finalHeight) / 2,
                Right = screenBounds.X + (screenBounds.Width - finalWidth) / 2 + finalWidth,
                Bottom = screenBounds.Y + (screenBounds.Height - finalHeight) / 2 + finalHeight
            };
        }
        
        private static WindowManager.RECT CalculateDoubleHalve(WindowManager.RECT currentRect, Rectangle screenBounds, float widthFactor, float heightFactor, bool upOrLeft)
        {
            int width = currentRect.Right - currentRect.Left;
            int height = currentRect.Bottom - currentRect.Top;
            
            int newWidth = widthFactor > 0 ? (int)(width * widthFactor) : width;
            int newHeight = heightFactor > 0 ? (int)(height * heightFactor) : height;
            
            int x = currentRect.Left;
            int y = currentRect.Top;
            
            if (widthFactor > 0)
            {
                if (upOrLeft) x = currentRect.Left - (newWidth - width);
                else x = currentRect.Left;
            }
            
            if (heightFactor > 0)
            {
                if (upOrLeft) y = currentRect.Top - (newHeight - height);
                else y = currentRect.Top;
            }
            
            return new WindowManager.RECT
            {
                Left = x,
                Top = y,
                Right = x + newWidth,
                Bottom = y + newHeight
            };
        }
    }
}

