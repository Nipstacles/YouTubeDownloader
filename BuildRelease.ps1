# YouTube Downloader - Release Build Script
# This script creates a ready-to-distribute package

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "YouTube Downloader - Release Builder" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path "bin\Release") {
	Remove-Item "bin\Release" -Recurse -Force
}
if (Test-Path "Release-Package") {
	Remove-Item "Release-Package" -Recurse -Force
}

# Build Release
Write-Host "Building Release configuration..." -ForegroundColor Yellow
dotnet build -c Release

if ($LASTEXITCODE -ne 0) {
	Write-Host "Build failed!" -ForegroundColor Red
	exit 1
}

Write-Host "Build successful!" -ForegroundColor Green
Write-Host ""

# Publish self-contained executable
Write-Host "Publishing self-contained executable..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishReadyToRun=true

if ($LASTEXITCODE -ne 0) {
	Write-Host "Publish failed!" -ForegroundColor Red
	exit 1
}

# Create release package directory
Write-Host "Creating release package..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path "Release-Package" -Force | Out-Null
New-Item -ItemType Directory -Path "Release-Package\YouTubeDownloader" -Force | Out-Null

# Copy published files
$publishPath = "bin\Release\net10.0-windows\win-x64\publish"
Copy-Item "$publishPath\*" "Release-Package\YouTubeDownloader\" -Recurse -Force

# Copy documentation
Write-Host "Copying documentation..." -ForegroundColor Yellow
Copy-Item "README.md" "Release-Package\YouTubeDownloader\" -Force
Copy-Item "INSTALLATION.md" "Release-Package\YouTubeDownloader\" -Force
Copy-Item "COOKIE_INSTRUCTIONS.md" "Release-Package\YouTubeDownloader\" -Force
Copy-Item "EMBEDDED_BUILD_GUIDE.md" "Release-Package\YouTubeDownloader\" -Force

# Create quick start guide
Write-Host "Creating Quick Start guide..." -ForegroundColor Yellow
@"
# YouTube Downloader - Quick Start

## Installation

1. Extract all files to a folder (e.g., C:\Program Files\YouTubeDownloader)
2. Run YouTubeDownloader.exe (yt-dlp and ffmpeg extract automatically on first run)

## First Time Setup

### Required:
1. **Install Node.js** from https://nodejs.org/
   - Download the LTS version
   - Run the installer with default settings
   - **Restart your computer after installation**

### Included (Automatic):
- yt-dlp.exe (embedded in application)
- ffmpeg.exe (embedded in application)
- Both extract automatically on first run to %LocalAppData%\YouTubeDownloader\tools\

### Optional (for age-restricted videos):
2. **Export cookies.txt** using a browser extension
   - Install "Get cookies.txt LOCALLY" extension
   - Go to YouTube.com while logged in
   - Export cookies.txt
   - Place in the same folder as YouTubeDownloader.exe

## Usage

1. Launch YouTubeDownloader.exe
2. Click "Status" button to verify Node.js is installed
3. Paste a YouTube URL
4. Select quality
5. Choose output folder
6. Click Download

For detailed help, see README.md

## Updating

To update the application:
1. Download the latest version ZIP
2. Extract and replace old files
3. Tools (yt-dlp/ffmpeg) re-extract automatically
4. Your settings and downloads are preserved

## Support

- Check INSTALLATION.md for setup help
- Check README.md for troubleshooting
- See COOKIE_INSTRUCTIONS.md for age-restricted video help
- See EMBEDDED_BUILD_GUIDE.md for technical details
"@ | Out-File "Release-Package\YouTubeDownloader\QUICKSTART.txt" -Encoding UTF8

# Create version info
Write-Host "Creating version info..." -ForegroundColor Yellow
@"
YouTube Downloader v1.0.0
Build Date: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
.NET Version: 10.0
Platform: Windows x64
Build Type: Self-contained (includes .NET runtime)

Files included:
- YouTubeDownloader.exe (Main application)
- System.*.dll (Runtime libraries)
- yt-dl_logo.png (Application logo)
- README.md (Full documentation)
- INSTALLATION.md (Installation guide)
- COOKIE_INSTRUCTIONS.md (Cookie setup guide)
- EMBEDDED_BUILD_GUIDE.md (Technical details)
- QUICKSTART.txt (Quick reference)

Embedded (auto-extract on first run):
- yt-dlp.exe (YouTube downloader engine)
- ffmpeg.exe (Video/audio merger)

Required (download separately):
- Node.js: https://nodejs.org/

Optional (user-provided):
- cookies.txt: Export from browser for age-restricted videos

Release Notes:
- Initial release with embedded yt-dlp and ffmpeg
- Self-contained .NET 10 runtime
- Automatic tool extraction to AppData
- Quality selection: Best, 1440p, 1080p, 720p, 480p, 360p, Audio Only
- Thumbnail preview
- System status checks
- Age-restricted video support
- Custom filename option
"@ | Out-File "Release-Package\YouTubeDownloader\VERSION.txt" -Encoding UTF8

# Create a simple batch installer
Write-Host "Creating installer script..." -ForegroundColor Yellow
@"
@echo off
echo ========================================
echo YouTube Downloader Installation
echo ========================================
echo.

set "INSTALL_DIR=%ProgramFiles%\YouTubeDownloader"

echo This will install YouTube Downloader to:
echo %INSTALL_DIR%
echo.
echo Press Ctrl+C to cancel, or
pause

echo.
echo Installing...

if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"

xcopy /E /I /Y "YouTubeDownloader\*" "%INSTALL_DIR%\" >nul

if %ERRORLEVEL% EQU 0 (
	echo.
	echo Installation complete!
	echo.
	echo Create desktop shortcut? (Y/N)
	set /p createShortcut=

	if /i "%createShortcut%"=="Y" (
		powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\Desktop\YouTube Downloader.lnk'); $Shortcut.TargetPath = '%INSTALL_DIR%\YouTubeDownloader.exe'; $Shortcut.WorkingDirectory = '%INSTALL_DIR%'; $Shortcut.Save()"
		echo Desktop shortcut created!
	)

	echo.
	echo IMPORTANT: You must install Node.js for this app to work!
	echo Download from: https://nodejs.org/
	echo.
	echo After installing Node.js, restart your computer.
	echo.
	pause

	echo.
	echo Open installation folder? (Y/N)
	set /p openFolder=
	if /i "%openFolder%"=="Y" explorer "%INSTALL_DIR%"
) else (
	echo.
	echo Installation failed! Please run as Administrator.
	pause
)
"@ | Out-File "Release-Package\INSTALL.bat" -Encoding ASCII

# Create portable version instructions
@"
# Portable Version

This is the portable version of YouTube Downloader.
No installation required!

## To Use:

1. Extract the "YouTubeDownloader" folder anywhere you want
2. Install Node.js from https://nodejs.org/ (required)
3. Run YouTubeDownloader.exe from the extracted folder

## Optional Setup:

- Place ffmpeg.exe in the YouTubeDownloader folder (recommended)
- Place cookies.txt in the YouTubeDownloader folder (for age-restricted videos)

## Benefits of Portable Version:

- No administrator rights needed
- Run from USB drive
- Multiple installations possible
- Easy to update (just replace files)

See QUICKSTART.txt for detailed instructions.
"@ | Out-File "Release-Package\PORTABLE_README.txt" -Encoding UTF8

# Compress to ZIP
Write-Host "Creating ZIP archive..." -ForegroundColor Yellow
$zipPath = "Release-Package\YouTubeDownloader-v1.0.0-Portable.zip"
Compress-Archive -Path "Release-Package\YouTubeDownloader" -DestinationPath $zipPath -Force

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Release package created successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Package contents:" -ForegroundColor Cyan
Write-Host "- Release-Package\YouTubeDownloader\ (Portable version)" -ForegroundColor White
Write-Host "- Release-Package\INSTALL.bat (Installer script)" -ForegroundColor White
Write-Host "- Release-Package\PORTABLE_README.txt (Portable instructions)" -ForegroundColor White
Write-Host "- Release-Package\YouTubeDownloader-v1.0.0-Portable.zip (ZIP archive)" -ForegroundColor White
Write-Host ""
Write-Host "Distribution options:" -ForegroundColor Cyan
Write-Host "1. Share the ZIP file for portable use" -ForegroundColor White
Write-Host "2. Share the folder with INSTALL.bat for easy installation" -ForegroundColor White
Write-Host "3. Upload to GitHub releases" -ForegroundColor White
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Test the release by running Release-Package\YouTubeDownloader\YouTubeDownloader.exe" -ForegroundColor White
Write-Host "2. Test the installer by running Release-Package\INSTALL.bat as Administrator" -ForegroundColor White
Write-Host "3. Create a GitHub release or distribute the files" -ForegroundColor White
Write-Host ""

# Open release folder
$openFolder = Read-Host "Open Release-Package folder? (Y/N)"
if ($openFolder -eq "Y" -or $openFolder -eq "y") {
	explorer "Release-Package"
}
