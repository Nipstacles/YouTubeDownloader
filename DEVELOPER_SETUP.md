# Developer Setup Guide

This guide is for developers who want to build the YouTube Downloader application from source.

## Prerequisites

### Required Software
1. **Visual Studio 2022/2026** or **Visual Studio Code**
   - .NET 10 SDK installed
   - Windows target platform support

2. **Git** (if cloning from repository)
   - Download from: https://git-scm.com/

3. **Node.js** (for testing downloads)
   - Download from: https://nodejs.org/

### Required Files for Build

Before building, you **must** add these executable files to the project root:

```
O:\development\visualstudio\csharp_projects\yt-dl\
├── yt-dlp.exe          ← REQUIRED for build
├── ffmpeg.exe          ← REQUIRED for full functionality
└── yt-dl.csproj
```

## Getting the Required Files

### 1. Download yt-dlp.exe

**Option A: GitHub Releases (Recommended)**
1. Go to: https://github.com/yt-dlp/yt-dlp/releases
2. Find the latest release
3. Download: `yt-dlp.exe` (Windows executable)
4. Place in project root: `O:\...\yt-dl\yt-dlp.exe`

**Option B: Using winget (Windows Package Manager)**
```powershell
winget install yt-dlp.yt-dlp
# Then copy from: C:\Users\YourName\AppData\Local\Microsoft\WinGet\Packages\...
```

**File Size**: ~18 MB  
**Current Version**: 2024.12.13 or newer

### 2. Download ffmpeg.exe

**Option A: Official Build (Recommended)**
1. Go to: https://ffmpeg.org/download.html
2. Click "Windows" → "Windows builds from gyan.dev"
3. Download: `ffmpeg-release-essentials.zip`
4. Extract the archive
5. Navigate to: `ffmpeg-x.x-essentials_build\bin\`
6. Copy `ffmpeg.exe` to project root: `O:\...\yt-dl\ffmpeg.exe`

**Option B: Using Chocolatey**
```powershell
choco install ffmpeg
# Then copy from: C:\ProgramData\chocolatey\lib\ffmpeg\tools\ffmpeg\bin\
```

**File Size**: ~100 MB  
**Current Version**: 6.1 or newer

## Project Structure After Setup

Once you've added the required files, your project should look like:

```
yt-dl/
├── MainForm.cs
├── MainForm.Designer.cs
├── MainForm.resx
├── Program.cs
├── yt-dl.csproj
├── yt-dl.sln
├── yt-dlp.exe                    ← Added by you
├── ffmpeg.exe                    ← Added by you
├── yt-dl_logo.png
├── README.md
├── COOKIE_INSTRUCTIONS.md
├── EMBEDDED_BUILD_GUIDE.md
├── DEVELOPER_SETUP.md (this file)
├── QuickBuild.bat
└── BuildRelease.ps1
```

## Building the Application

### Quick Build (Recommended)

Double-click the batch file:
```
QuickBuild.bat
```

This will:
1. Build the Release configuration
2. Publish as self-contained for win-x64
3. Open the output folder

### Manual Build (Visual Studio)

1. Open `yt-dl.sln` in Visual Studio
2. Set configuration to **Release**
3. Right-click project → **Publish**
4. Use existing profile or create new:
   - Target: **Folder**
   - Configuration: **Release | Any CPU**
   - Target Framework: **net10.0-windows**
   - Deployment Mode: **Self-contained**
   - Target Runtime: **win-x64**
   - Produce single file: ❌ (unchecked)

### Manual Build (Command Line)

```powershell
cd O:\development\visualstudio\csharp_projects\yt-dl\

# Clean previous builds
dotnet clean -c Release

# Build Release
dotnet build -c Release

# Publish self-contained
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishReadyToRun=true
```

Output will be in:
```
bin\Release\net10.0-windows\win-x64\publish\
```

## Build Configuration Details

### Embedded Resources

The project embeds executables as resources:

```xml
<ItemGroup>
  <EmbeddedResource Include="yt-dlp.exe" />
  <EmbeddedResource Include="ffmpeg.exe" Condition="Exists('ffmpeg.exe')" />
</ItemGroup>
```

**Important Notes:**
- yt-dlp.exe is **required** – build will fail if missing
- ffmpeg.exe is **optional** – build succeeds without it, but:
  - The app will warn at runtime
  - Video+audio merging won't work

### Build Warnings

**CS1566: Error reading resource 'yt_dl.ffmpeg.exe'**
- **Cause**: ffmpeg.exe is not in the project root
- **Fix**: Download and add ffmpeg.exe (see above)
- **Note**: Build will still succeed; ffmpeg just won't be embedded

## Testing the Build

### 1. Verify Extraction

Run the published executable:
```
bin\Release\net10.0-windows\win-x64\publish\YouTubeDownloader.exe
```

Check that tools extracted to:
```
%LocalAppData%\YouTubeDownloader\tools\
├── yt-dlp.exe
└── ffmpeg.exe
```

### 2. Test Download

1. Click "Status" button
   - Verify Node.js is detected
   - Verify yt-dlp and ffmpeg status

2. Paste a test URL:
   ```
   https://www.youtube.com/watch?v=dQw4w9WgXcQ
   ```

3. Select quality and click "Download"

4. Verify output in Downloads folder

## Debugging

### Enable Debug Mode

In `MainForm.cs`, you can add logging:

```csharp
private void LogDebug(string message)
{
	Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
}
```

### Common Issues

**Build Error: "yt-dlp.exe not found"**
- yt-dlp.exe is missing from project root
- Download and add it (see above)

**Runtime Error: "yt-dlp.exe not found"**
- Extraction failed or was blocked
- Check: `%LocalAppData%\YouTubeDownloader\tools\`
- Add antivirus exception

**Downloads fail: "Node.js not detected"**
- Install Node.js
- Restart Visual Studio
- Rebuild and test

**Video-only or audio-only downloads**
- ffmpeg.exe wasn't embedded (missing during build)
- Rebuild with ffmpeg.exe in project root

## Release Packaging

### Using BuildRelease.ps1 (Automated)

```powershell
.\BuildRelease.ps1
```

This will:
1. Clean and build Release
2. Publish self-contained
3. Copy documentation files
4. Create version info
5. Generate ZIP package

Output: `Release-Package\YouTubeDownloader-vX.X.X.zip`

### Manual Package

1. Copy these files from `bin\Release\net10.0-windows\win-x64\publish\`:
   - YouTubeDownloader.exe
   - All .dll files
   - yt-dl_logo.png

2. Add documentation:
   - README.md
   - COOKIE_INSTRUCTIONS.md
   - EMBEDDED_BUILD_GUIDE.md
   - LICENSE (if applicable)

3. Create ZIP archive

## Version Updates

### Updating yt-dlp

1. Download latest yt-dlp.exe
2. Replace file in project root
3. Rebuild and republish
4. Users get updated yt-dlp with new app version

### Updating ffmpeg

1. Download latest ffmpeg.exe
2. Replace file in project root
3. Rebuild and republish

### Updating .NET Version

If migrating to a newer .NET version:

1. Update `yt-dl.csproj`:
   ```xml
   <TargetFramework>net11.0-windows</TargetFramework>
   ```

2. Clean solution:
   ```powershell
   dotnet clean
   ```

3. Rebuild and test

## Distribution Checklist

Before releasing a new version:

- [ ] yt-dlp.exe is latest version
- [ ] ffmpeg.exe is latest version
- [ ] Build succeeds without warnings
- [ ] Test download on clean machine
- [ ] Verify Node.js detection works
- [ ] Test age-restricted video with cookies.txt
- [ ] Verify tool extraction to AppData
- [ ] Update version numbers in docs
- [ ] Create release notes
- [ ] Test on Windows 10 and Windows 11
- [ ] Scan with antivirus (expect false positives)
- [ ] Create signed installer (optional but recommended)

## Code Signing (Optional)

To reduce antivirus false positives:

1. Obtain code signing certificate ($100-400/year)
   - DigiCert, Sectigo, or similar CA

2. Sign the executable:
   ```powershell
   signtool sign /f cert.pfx /p password /t http://timestamp.digicert.com YouTubeDownloader.exe
   ```

3. Verify signature:
   ```powershell
   signtool verify /pa YouTubeDownloader.exe
   ```

## Troubleshooting Build Issues

### "Could not find a part of the path"
- Long path issue; move project closer to root (e.g., `C:\dev\yt-dl\`)

### "Access denied" during build
- Close running instances of the app
- Check antivirus isn't locking files

### Embedded resource not extracting
- Check resource name matches: `yt_dl.yt-dlp.exe`
- Verify Build Action is set to "Embedded Resource"
- Clean and rebuild

### Self-contained publish is huge (~200 MB)
- This is normal (includes .NET runtime + tools)
- yt-dlp: ~18 MB
- ffmpeg: ~100 MB
- .NET runtime: ~60 MB

## Contributing

If contributing to the project:

1. Fork the repository
2. Create a feature branch
3. Add yt-dlp.exe and ffmpeg.exe (ignored by .gitignore)
4. Make your changes
5. Test thoroughly
6. Submit pull request

**Note**: Do not commit yt-dlp.exe or ffmpeg.exe to the repository.

---

## Quick Reference

**Project Root**: `O:\development\visualstudio\csharp_projects\yt-dl\`  
**Required Files**: yt-dlp.exe, ffmpeg.exe  
**Output Folder**: `bin\Release\net10.0-windows\win-x64\publish\`  
**Tools Extract To**: `%LocalAppData%\YouTubeDownloader\tools\`  

**Quick Build**: `QuickBuild.bat`  
**Full Package**: `BuildRelease.ps1`  

**yt-dlp Download**: https://github.com/yt-dlp/yt-dlp/releases  
**ffmpeg Download**: https://ffmpeg.org/download.html  
**Node.js Download**: https://nodejs.org/  

---

**Once you have yt-dlp.exe and ffmpeg.exe in place, building is as simple as running `QuickBuild.bat`!**
