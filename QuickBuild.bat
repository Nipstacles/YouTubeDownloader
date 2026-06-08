@echo off
echo ========================================
echo YouTube Downloader - Quick Release
echo ========================================
echo.

echo Building release...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishReadyToRun=true

if %ERRORLEVEL% NEQ 0 (
	echo.
	echo Build failed!
	pause
	exit /b 1
)

echo.
echo Build successful! Files are in:
echo bin\Release\net10.0-windows\win-x64\publish\
echo.
echo Opening publish folder...
start "" "bin\Release\net10.0-windows\win-x64\publish"

echo.
echo Next steps:
echo 1. Test YouTubeDownloader.exe
echo 2. Copy all files to a distribution folder
echo 3. Add README.md and COOKIE_INSTRUCTIONS.md
echo 4. Create a ZIP file
echo 5. Share with others!
echo.
pause
