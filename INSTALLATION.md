# Installation Guide

Quick guide to get YouTube Downloader up and running.

## What You Get

This application comes **fully packaged** with almost everything you need:
- ✅ YouTubeDownloader.exe (main application)
- ✅ yt-dlp.exe (embedded - extracts automatically)
- ✅ ffmpeg.exe (embedded - extracts automatically)
- ✅ .NET 10 runtime (self-contained)

## What You Need to Install

### 1. Node.js (Required!)

**Without Node.js, downloads will fail!**

1. Go to: https://nodejs.org/
2. Download the **LTS version** (recommended)
3. Run the installer
4. Click through the wizard (default settings are fine)
5. **Restart your computer** after installation

**Why?** YouTube uses anti-bot protection that requires Node.js to bypass.

### Verify Node.js Installation

Open Command Prompt and type:
```cmd
node --version
```

You should see something like `v22.11.0` or similar.

## First Run

1. **Extract the ZIP file** to a permanent location
   - Good: `C:\Program Files\YouTubeDownloader\`
   - Good: `C:\Users\YourName\Apps\YouTubeDownloader\`
   - Bad: Your Downloads folder (temp location)

2. **Run YouTubeDownloader.exe**
   - On first run, it will extract yt-dlp and ffmpeg automatically
   - This takes a few seconds
   - Tools extract to: `%LocalAppData%\YouTubeDownloader\tools\`

3. **Click the "Status" button**
   - Verify Node.js shows: ✓ Installed
   - Verify yt-dlp shows: ✓ Embedded
   - Verify ffmpeg shows: ✓ Embedded

4. **Start downloading!**
   - Paste any YouTube URL
   - Select quality
   - Click Download

## Optional: Age-Restricted Videos

Most videos work without any additional setup. For **age-restricted videos only**:

1. **Export cookies.txt from your browser**
   - Install "Get cookies.txt LOCALLY" extension (Chrome/Firefox)
   - Go to YouTube.com (while logged in)
   - Click the extension → Export
   - Save as `cookies.txt`

2. **Place cookies.txt in the app folder**
   ```
   C:\...\YouTubeDownloader\
   ├── YouTubeDownloader.exe
   ├── cookies.txt          ← Add here
   └── other files...
   ```

3. **Enable the checkbox**
   - Check "Bypass age restrictions"
   - Download as normal

**Full instructions**: See `COOKIE_INSTRUCTIONS.md`

## Folder Structure

After installation and first run:

```
[Installation Folder]
├── YouTubeDownloader.exe       Main application
├── System.*.dll                Runtime files
├── yt-dl_logo.png             Icon
├── cookies.txt                 (Optional - you add this)
├── README.md                   Full documentation
├── COOKIE_INSTRUCTIONS.md      Cookie setup guide
└── INSTALLATION.md             This file

[AppData Folder] (%LocalAppData%\YouTubeDownloader\tools\)
├── yt-dlp.exe                  Auto-extracted
└── ffmpeg.exe                  Auto-extracted
```

## System Requirements

**Operating System:**
- Windows 10 (64-bit) or newer
- Windows 11 (64-bit)

**Disk Space:**
- ~200 MB for application files
- Additional space for downloaded videos

**RAM:**
- Minimum: 2 GB
- Recommended: 4 GB+

**Internet:**
- Required for downloading videos
- Bandwidth depends on video quality

**Required Software:**
- Node.js v18 or newer (LTS recommended)

## Antivirus Warnings

**You may see warnings from antivirus software.**

This is a **false positive**. Here's why:
- yt-dlp and ffmpeg are legitimate tools
- Executables embedded in another executable trigger warnings
- Self-extracting behavior mimics malware

**Solutions:**
1. Add an exception for `YouTubeDownloader.exe`
2. Verify the download source (official GitHub releases)
3. Check file hashes (if provided)
4. Submit false positive report to your antivirus vendor

## Uninstallation

To completely remove YouTube Downloader:

1. **Delete the application folder**
   ```
   C:\...\YouTubeDownloader\
   ```

2. **Delete the tools folder**
   ```
   %LocalAppData%\YouTubeDownloader\tools\
   ```
   Full path: `C:\Users\YourName\AppData\Local\YouTubeDownloader\tools\`

3. **Uninstall Node.js** (optional, if not needed for other apps)
   - Settings → Apps → Node.js → Uninstall

4. **Delete downloaded videos** (if desired)
   - Default location: `%UserProfile%\Downloads\`

## Troubleshooting Installation

### "Windows protected your PC" (SmartScreen)

This appears when the app isn't code-signed.

**Fix:**
1. Click "More info"
2. Click "Run anyway"

### "Node.js not detected" after installation

**Fix:**
1. Restart your computer (required!)
2. Restart the application
3. Click "Status" to verify

### "yt-dlp.exe not found" error

**Fix:**
1. Check: `%LocalAppData%\YouTubeDownloader\tools\`
2. If folder is empty, extraction was blocked
3. Add antivirus exception
4. Restart application

### Application won't start

**Possible causes:**
- Antivirus blocked the executable
- Missing .NET 10 runtime (shouldn't happen with self-contained build)
- Corrupted download

**Fix:**
1. Re-download the application
2. Verify ZIP extraction completed successfully
3. Add antivirus exception
4. Try running as administrator (right-click → Run as administrator)

### Downloads fail immediately

**Check these:**
1. Node.js installed? (Required!)
2. Internet connection working?
3. YouTube URL valid?
4. Video available in your country?

Click "Status" to verify system requirements.

## Updating

### Automatic Updates
There are no automatic updates. You must manually download new versions.

### Manual Update

1. Download the latest version
2. Close the old version
3. Extract new version to a new folder (or replace old files)
4. Run the new version
5. Tools will auto-extract again (safe to re-extract)

**Your settings and downloads are safe** - they're not stored in the app folder.

## Getting Help

**Read the documentation:**
- `README.md` - Full feature guide
- `COOKIE_INSTRUCTIONS.md` - Age-restricted video setup
- `EMBEDDED_BUILD_GUIDE.md` - Technical details

**Check Status:**
- Click the "Status" button in the app
- Shows Node.js, yt-dlp, ffmpeg status
- Shows folder locations

**Common Issues:**
- Most problems are "Node.js not installed"
- Restart computer after installing Node.js
- Check antivirus didn't block extraction

## Quick Start Checklist

Use this checklist for a smooth installation:

- [ ] Downloaded YouTubeDownloader ZIP
- [ ] Extracted to permanent folder
- [ ] Installed Node.js from https://nodejs.org/
- [ ] Restarted computer
- [ ] Ran YouTubeDownloader.exe (tools auto-extract)
- [ ] Clicked "Status" button - all green checkmarks
- [ ] Tested download with any YouTube URL
- [ ] (Optional) Added cookies.txt for age-restricted videos

---

**That's it! You're ready to download YouTube videos.**

For age-restricted videos, see `COOKIE_INSTRUCTIONS.md`.  
For detailed features, see `README.md`.  
For technical details, see `EMBEDDED_BUILD_GUIDE.md`.
