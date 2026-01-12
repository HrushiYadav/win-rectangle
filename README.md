# Rectangle Windows

A window management tool for Windows, inspired by Rectangle for macOS. This application allows you to quickly organize and resize windows using keyboard shortcuts.

## Features

- **Window Snapping**: Snap windows to halves, quarters, thirds, and more
- **Keyboard Shortcuts**: Global hotkeys for quick window management
- **System Tray**: Runs in the background with a system tray icon
- **Restore**: Restore windows to their previous position

## Keyboard Shortcuts

- `Ctrl+Alt+Left Arrow` - Snap window to left half
- `Ctrl+Alt+Right Arrow` - Snap window to right half
- `Ctrl+Alt+Up Arrow` - Snap window to top half
- `Ctrl+Alt+Down Arrow` - Snap window to bottom half
- `Ctrl+Alt+Enter` - Maximize window
- `Ctrl+Alt+C` - Center window
- `Ctrl+Alt+D` - Snap to first third (left)
- `Ctrl+Alt+F` - Snap to center third
- `Ctrl+Alt+G` - Snap to last third (right)
- `Ctrl+Alt+Delete` - Restore window to previous position

## Installation

### Prerequisites

- Windows 10 or later
- .NET 8.0 Runtime (or SDK for building)

### Building from Source

1. Install the .NET 8.0 SDK from [Microsoft's website](https://dotnet.microsoft.com/download)
2. Open a terminal in the `RectangleWindows` directory
3. Run:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

### Creating an Executable

To create a standalone executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be in `bin/Release/net8.0-windows/win-x64/publish/`

## Usage

1. Launch the application
2. The application will run in the system tray (notification area)
3. Use keyboard shortcuts to manage windows
4. Right-click the system tray icon for a context menu with all available actions
5. Select "Exit" from the context menu to quit the application

## How It Works

Rectangle Windows uses Windows API calls to:
- Detect the currently active window
- Calculate the appropriate screen bounds
- Resize and reposition windows
- Register global hotkeys for keyboard shortcuts

## System Requirements

- Windows 10 or later
- .NET 8.0 Runtime
- Administrator privileges are NOT required

## Troubleshooting

### Hotkeys Not Working

- Make sure no other application is using the same keyboard shortcuts
- Try restarting the application
- Check if the application is running (look for the system tray icon)

### Windows Not Snapping Correctly

- Make sure the target window is not maximized or in a special state
- Some applications may not support programmatic resizing
- Try using the context menu actions instead of keyboard shortcuts

## License

This project is inspired by Rectangle for macOS. See the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

