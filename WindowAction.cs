using System;

namespace RectangleWindows
{
    public enum WindowAction
    {
        // Halves
        LeftHalf,
        RightHalf,
        TopHalf,
        BottomHalf,
        CenterHalf,
        
        // Quarters
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        
        // Thirds
        FirstThird,
        CenterThird,
        LastThird,
        FirstTwoThirds,
        CenterTwoThirds,
        LastTwoThirds,
        
        // Vertical Thirds
        TopVerticalThird,
        MiddleVerticalThird,
        BottomVerticalThird,
        TopVerticalTwoThirds,
        BottomVerticalTwoThirds,
        
        // Corner Thirds
        TopLeftThird,
        TopRightThird,
        BottomLeftThird,
        BottomRightThird,
        
        // Fourths
        FirstFourth,
        SecondFourth,
        ThirdFourth,
        LastFourth,
        FirstThreeFourths,
        CenterThreeFourths,
        LastThreeFourths,
        
        // Sixths
        TopLeftSixth,
        TopCenterSixth,
        TopRightSixth,
        BottomLeftSixth,
        BottomCenterSixth,
        BottomRightSixth,
        
        // Eighths
        TopLeftEighth,
        TopCenterLeftEighth,
        TopCenterRightEighth,
        TopRightEighth,
        BottomLeftEighth,
        BottomCenterLeftEighth,
        BottomCenterRightEighth,
        BottomRightEighth,
        
        // Ninths
        TopLeftNinth,
        TopCenterNinth,
        TopRightNinth,
        MiddleLeftNinth,
        MiddleCenterNinth,
        MiddleRightNinth,
        BottomLeftNinth,
        BottomCenterNinth,
        BottomRightNinth,
        
        // Size Adjustments
        Larger,
        Smaller,
        LargerWidth,
        SmallerWidth,
        LargerHeight,
        SmallerHeight,
        
        // Movement
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        
        // Display
        NextDisplay,
        PreviousDisplay,
        
        // Special
        Maximize,
        AlmostMaximize,
        MaximizeHeight,
        Center,
        CenterProminently,
        Restore,
        
        // Advanced Resizing
        DoubleHeightUp,
        DoubleHeightDown,
        DoubleWidthLeft,
        DoubleWidthRight,
        HalveHeightUp,
        HalveHeightDown,
        HalveWidthLeft,
        HalveWidthRight,
        
        // Multi-window
        TileAll,
        CascadeAll,
        TileActiveApp,
        CascadeActiveApp,
        
        // Todo Mode
        LeftTodo,
        RightTodo,
        
        // Other
        ReverseAll,
        Specified
    }
}

