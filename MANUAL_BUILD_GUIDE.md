# Manual Release Build Guide

If you prefer not to use the PowerShell script, follow these steps in Visual Studio.

## Step 1: Configure Release Build

1. In Visual Studio, click the dropdown next to "Debug" in the toolbar
2. Select **"Release"**
3. Make sure **"Any CPU"** or **"x64"** is selected

## Step 2: Build the Project

1. Click **Build** menu → **Rebuild Solution**
2. Wait for build to complete (check Output window)
3. Ensure no errors appear

## Step 3: Publish the Application

### Option A: Using Visual Studio Publish

1. Right-click the **yt-dl** project in Solution Explorer
2. Select **Publish...**
3. Choose **Folder** as target
4. Click **Next**
5. Choose **Folder** as specific target
6. Set location to: `bin\Release\net10.0-windows\publish`
7. Click **Finish**
8. Click **Show all settings**
9. Configure these settings:
   - **Configuration**: Release | Any CPU
   - **Target framework**: net10.0-windows
   - **Deployment mode**: Self-contained
   - **Target runtime**: win-x64
   - **Produce single file**: No (we need yt-dlp.exe separate)
   - **Enable ReadyToRun compilation**: Yes
10. Click **Save**
11. Click **Publish**

### Option B: Using Command Line

Open PowerShell in project directory and run:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishReadyToRun=true
```

## Step 4: Locate Published Files

Files will be in:
```
bin\Release\net10.0-windows\win-x64\publish\
```

## Step 5: Create Distribution Package

1. Create a new folder: `YouTubeDownloader-v1.0.0`

2. Copy these files from `publish` folder:
   - `YouTubeDownloader.exe`
   - `yt-dlp.exe`
   - `yt-dl_logo.png`
   - All `.dll` files
   - All other supporting files

3. Copy documentation:
   - `README.md`
   - `COOKIE_INSTRUCTIONS.md`
   - `QUICKSTART.txt` (create from template below)

4. Create `QUICKSTART.txt`:
```
YouTube Downloader - Quick Start

1. Install Node.js from https://nodejs.org/ (REQUIRED)
2. Download ffmpeg.exe from https://ffmpeg.org/ (Recommended)
   - Place in same folder as YouTubeDownloader.exe
3. Run YouTubeDownloader.exe
4. Click "Status" to verify Node.js
5. Paste YouTube URL and click Download!

For age-restricted videos:
- Export cookies.txt from your browser
- See COOKIE_INSTRUCTIONS.md for details

For help, see README.md
```

## Step 6: Create ZIP Archive

### Windows Explorer:
1. Right-click the `YouTubeDownloader-v1.0.0` folder
2. Select **Send to** → **Compressed (zipped) folder**
3. Rename to: `YouTubeDownloader-v1.0.0-Portable.zip`

### PowerShell:
```powershell
Compress-Archive -Path "YouTubeDownloader-v1.0.0" -DestinationPath "YouTubeDownloader-v1.0.0-Portable.zip"
```

## Step 7: Test the Release

### Test on Your Computer:
1. Extract the ZIP to a new folder (simulate fresh install)
2. Run `YouTubeDownloader.exe`
3. Test downloading a video
4. Verify all features work

### Test on Clean Computer (Recommended):
1. Use a virtual machine or friend's computer
2. Computer should NOT have .NET installed
3. Extract and run to test self-contained deployment
4. Install Node.js and test downloading

## Step 8: Distribute

Choose one or more methods:

### 1. Direct Download
- Upload ZIP to Google Drive, OneDrive, or Dropbox
- Share the link

### 2. GitHub Release
- Create repository on GitHub
- Go to Releases → Create new release
- Upload ZIP file
- Write release notes

### 3. Your Website
- Upload to your web hosting
- Create download page
- Include documentation

## Optional: Create an Installer

### Using Inno Setup (Free):

1. Download Inno Setup: https://jrsoftware.org/isinfo.php

2. Create `installer.iss` script:
```iss
[Setup]
AppName=YouTube Downloader
AppVersion=1.0.0
DefaultDirName={autopf}\YouTubeDownloader
DefaultGroupName=YouTube Downloader
OutputDir=.
OutputBaseFilename=YouTubeDownloader-Setup-v1.0.0

[Files]
Source: "YouTubeDownloader-v1.0.0\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\YouTube Downloader"; Filename: "{app}\YouTubeDownloader.exe"
Name: "{commondesktop}\YouTube Downloader"; Filename: "{app}\YouTubeDownloader.exe"

[Run]
Filename: "{app}\YouTubeDownloader.exe"; Description: "Launch YouTube Downloader"; Flags: postinstall nowait skipifsilent
```

3. Open `installer.iss` in Inno Setup
4. Click **Build** → **Compile**
5. Result: `YouTubeDownloader-Setup-v1.0.0.exe`

## File Size Expectations

- Self-contained (with .NET runtime): ~60-80 MB
- Framework-dependent (requires .NET installed): ~5-10 MB

The self-contained version is larger but works on any Windows PC without requiring .NET installation.

## Troubleshooting Build Issues

### "Unable to find package 'Microsoft.NETCore.App'"
- Update Visual Studio to latest version
- Run: `dotnet restore`

### "Could not copy yt-dlp.exe"
- Close any running instances
- Delete `bin` and `obj` folders
- Rebuild

### "NETSDK1045: The current .NET SDK does not support targeting .NET 10.0"
- Either:
  - Update to .NET 10 SDK
  - OR change `TargetFramework` to `net8.0-windows` in .csproj

### Executable won't run on other computers
- Ensure you selected "Self-contained" in publish settings
- Verify `win-x64` runtime is specified
- Check antivirus isn't blocking it

## Version Updates

When updating to v1.0.1, v1.1.0, etc.:

1. Update version in `yt-dl.csproj`:
   ```xml
   <Version>1.0.1</Version>
   ```

2. Rebuild and republish

3. Update documentation with changelog

4. Create new release with updated version number

## Checklist Before Release

- [ ] Version number updated in project file
- [ ] Tested on clean machine
- [ ] All documentation included
- [ ] README has correct version
- [ ] No debug/test code remains
- [ ] Logo displays correctly
- [ ] Node.js check works
- [ ] All quality options work
- [ ] Thumbnail preview works
- [ ] File downloads successfully

---

**Ready to distribute!** Share your ZIP file or installer with users.
