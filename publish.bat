@echo off
echo Publishing Rectangle Windows as standalone executable...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
if %ERRORLEVEL% EQU 0 (
    echo Publish successful!
    echo Executable location: bin\Release\net8.0-windows\win-x64\publish\RectangleWindows.exe
) else (
    echo Publish failed!
    exit /b 1
)

