# YouTube Downloader

A Windows desktop application for downloading YouTube videos and audio with quality selection, metadata preview, playlist handling, cookies support, and bundled downloader tools.

## Features

✅ **Quality Selection**
- Best Quality
- 1440p (2K) or higher
- 1080p (Full HD) or higher
- 720p (HD) or higher
- 480p or higher
- 360p or higher
- Audio Only (Best Quality)
- Audio Only (MP3 320k)

✅ **Video Preview and Metadata**
- Automatic thumbnail preview when pasting URLs
- Shows title, channel, duration, and maximum available resolution when metadata is available
- Status bar shows the detected video title

✅ **Progress and Cancel Support**
- Real download progress percentage
- Shows download size, speed, and ETA when yt-dlp reports them
- The Download button changes to Cancel while a download is active

✅ **Playlist Handling**
- Single-video downloads are the default, even when the URL contains playlist parameters
- Optional **Download full playlist** checkbox
- Playlist-friendly filename template option

✅ **File Naming Templates**
- Title only
- Uploader - Title
- Upload date - Title
- Playlist index - Title
- Custom filename field

✅ **System Status and Repair Tools**
- Detects Node.js
- Detects cookies.txt
- Detects yt-dlp.exe and shows its version
- Detects ffmpeg.exe
- Import cookies.txt from the UI
- Locate/copy ffmpeg.exe from the UI
- Open output folder
- Open app folder
- Update yt-dlp.exe from the Tools menu

✅ **Age-Restricted and Account-Restricted Content**
- Supports cookies.txt import
- Bypass age restrictions option
- Helpful error messages for cookies, login, age restriction, and anti-bot issues

✅ **Portable Mode and Theme Support**
- Portable mode stores settings beside the app in `settings.json`
- Standard mode stores settings in the user's AppData folder
- Dark theme toggle

✅ **Bundled Tool Support**
- `yt-dlp.exe` is included with published builds
- `ffmpeg.exe` can be included with published builds when present in the project folder
- In-app yt-dlp updater keeps the downloader current

## Requirements

See `PREREQUISITES.md` for the full prerequisite guide.

### Included or Bundled

1. **yt-dlp.exe**
   - Used to download video/audio.
   - Included in the published app folder.
   - Update from inside the app:
	 - **Tools > Update yt-dlp**

2. **ffmpeg.exe**
   - Required to merge separate video and audio streams.
   - Required for most high-quality downloads and audio conversion.
   - Included in release/publish output if `ffmpeg.exe` exists in the project folder before publishing.
   - Can also be copied into the app folder using:
	 - **Tools > Locate ffmpeg.exe**

### Installed Separately

3. **Node.js**
   - Required for some YouTube anti-bot/signature challenge scenarios.
   - Recommended for reliable downloading.
   - Install the LTS version from https://nodejs.org/
   - Restart this app after installing Node.js.

### Optional, But Required for Restricted Videos

4. **cookies.txt**
   - Required for age-restricted videos, account-restricted videos, member-only videos you have access to, or videos that require login.
   - Import from the main window using **Import cookies**.
   - Also available from:
	 - **Tools > Import cookies.txt**
   - Recommended browser extension:
	 - **Get cookies.txt LOCALLY**

## How to Use

### Download a Video
1. Paste a YouTube URL in the **YouTube URL** field.
2. Wait for thumbnail/metadata preview when available.
3. Select a quality.
4. Choose an output folder.
5. Choose a filename template.
6. Optional: enable **Download full playlist** for playlist URLs.
7. Optional: import cookies and enable **Bypass age restrictions** for restricted videos.
8. Click **Download**.

While downloading, the **Download** button becomes **Cancel**.

### Download Audio Only
1. Paste the YouTube URL.
2. Select either:
   - **Audio Only (Best Quality)**
   - **Audio Only (MP3 320k)**
3. Click **Download**.

### Download a Playlist
1. Paste a playlist URL or a video URL containing playlist parameters.
2. Enable **Download full playlist**.
3. Consider using the **Playlist index - Title** filename template.
4. Click **Download**.

If **Download full playlist** is not enabled, the app uses single-video mode by default.

### Age-Restricted Videos
1. Sign in to YouTube in your browser.
2. Export cookies using a cookies.txt browser extension.
3. Click **Import cookies** in the app.
4. Enable **Bypass age restrictions**.
5. Download as normal.

## Toolbar / Tools Menu

The **Tools** menu includes:

- **Update yt-dlp**
  - Downloads the latest official Windows `yt-dlp.exe` release and replaces the bundled copy safely.

- **Import cookies.txt**
  - Copies a selected cookies file into the app folder.

- **Locate ffmpeg.exe**
  - Copies a selected `ffmpeg.exe` into the app folder.

- **Open App Folder**
  - Opens the folder containing the application and bundled tools.

- **Portable Mode**
  - Stores settings in `settings.json` beside the app.

- **Dark Theme**
  - Toggles dark mode.

## Status Indicators

The app displays live status for:

- `yt-dlp.exe`
- `ffmpeg.exe`
- Node.js
- `cookies.txt`

Click **Status** for detailed system information, including:

- Node.js installed/not found
- ffmpeg.exe found/not found
- cookies.txt found/not found
- yt-dlp.exe found/not found
- app folder
- settings mode

## File Locations

Typical application folder contents:

```text
[App Folder]/
├── YouTubeDownloader.exe
├── yt-dlp.exe
├── ffmpeg.exe
├── yt-dl_logo.png
├── cookies.txt              optional
├── settings.json            portable mode only
├── PREREQUISITES.md
└── README.md
```

To open the app folder, use:

```text
Tools > Open App Folder
```

## Portable Mode vs Standard Mode

### Standard Mode
- Settings are stored in the user's AppData folder.
- Recommended for normal installed usage.

### Portable Mode
- Settings are stored in `settings.json` beside the app.
- Useful for portable folders or USB drives.
- Enable from:
  - **Tools > Portable Mode**

## Publishing / Release Build

Publish a self-contained Windows x64 build:

```powershell
dotnet publish "D:\local\development\vstudio\csharp\YouTubeDownloader\yt-dl.csproj" -c Release -r win-x64 --self-contained true -o "D:\local\development\vstudio\csharp\YouTubeDownloader\publish\win-x64"
```

Published output:

```text
publish/win-x64/
├── YouTubeDownloader.exe
├── yt-dlp.exe
├── ffmpeg.exe
└── runtime/support files
```

Distribute the full contents of `publish/win-x64`, not just the EXE.

## Installer

An Inno Setup installer script is included:

```text
YouTubeDownloaderInstaller.iss
```

The installer can package the published output and lets users choose between:

- Standard install
- Portable mode

To build the installer:

1. Install Inno Setup from https://jrsoftware.org/isinfo.php
2. Publish the app first.
3. Open `YouTubeDownloaderInstaller.iss` in Inno Setup.
4. Compile the installer.

Installer output is written to:

```text
installer/
```

## Troubleshooting

### Metadata unavailable
- The video may block metadata access.
- Try importing cookies.
- Try updating yt-dlp.
- Playlist URLs are treated as single-video URLs unless **Download full playlist** is enabled.

### Requested format is not available
- The selected quality may not be available for that video/client combination.
- Try **Best Quality** or click **List Formats**.
- Update yt-dlp from **Tools > Update yt-dlp**.

### ffmpeg is missing or merge fails
- Use **Tools > Locate ffmpeg.exe**.
- Ensure `ffmpeg.exe` is in the same folder as `YouTubeDownloader.exe`.

### Node.js is not detected
- Install Node.js LTS from https://nodejs.org/
- Restart the app.

### Age-restricted videos fail
- Import a fresh `cookies.txt` file.
- Enable **Bypass age restrictions**.
- Ensure the browser account used to export cookies can view the video.

### Download is stuck or unwanted
- Click **Cancel**. The Download button changes to Cancel while a download is active.

### Downloads are video-only or audio-only
- Ensure `ffmpeg.exe` is available.
- Use **Status** to verify ffmpeg is found.

## Tips

- Use **Best Quality** when unsure which resolution is available.
- Use **List Formats** to inspect available formats for a specific URL.
- Use **Playlist index - Title** for playlist downloads.
- Keep yt-dlp updated because YouTube changes frequently.
- Refresh cookies periodically; they expire or become invalid over time.

## Third-Party Tools

This application uses:

- **yt-dlp** - https://github.com/yt-dlp/yt-dlp
- **ffmpeg** - https://ffmpeg.org/
- **Node.js** - https://nodejs.org/ optional but recommended for YouTube challenge handling

## Support

For issues with:

- **This Application**: Check the status indicators, logs, and troubleshooting section.
- **yt-dlp**: https://github.com/yt-dlp/yt-dlp/issues
- **Node.js**: https://nodejs.org/
- **ffmpeg**: https://ffmpeg.org/
