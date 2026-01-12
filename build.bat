@echo off
echo Building Rectangle Windows...
dotnet build -c Release
if %ERRORLEVEL% EQU 0 (
    echo Build successful!
    echo Executable location: bin\Release\net8.0-windows\RectangleWindows.exe
) else (
    echo Build failed!
    exit /b 1
)

