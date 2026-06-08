# YouTube Downloader Prerequisites

This application includes the main downloader program, but a few supporting files or tools may be needed depending on what you want to download.

## Required

### yt-dlp.exe
- Used to download videos and audio from YouTube.
- This program expects `yt-dlp.exe` to be in the same folder as the application.
- The app includes a toolbar option to update it:
  - **Tools > Update yt-dlp**

## Strongly Recommended

### ffmpeg.exe
- Required to merge separate video and audio streams into one playable video file.
- Required for most high-quality downloads such as 720p, 1080p, 1440p, and 4K.
- Also required for audio conversion, such as MP3 output.
- The app expects `ffmpeg.exe` to be in the same folder as the application.
- You can locate/copy it using:
  - **Tools > Locate ffmpeg.exe**

Without ffmpeg, downloads may be limited, fail to merge, or produce video-only/audio-only files.

## Required for Some YouTube Videos

### Node.js
- Needed when YouTube uses anti-bot or signature challenge protection.
- Especially useful for videos that fail with JavaScript, `nsig`, or challenge-related errors.
- Install the LTS version from:
  - https://nodejs.org/
- After installing Node.js, restart the application.

## Required for Age-Restricted or Account-Restricted Videos

### cookies.txt
- Needed when downloading videos that require a signed-in YouTube session.
- This includes:
  - age-restricted videos
  - member-only videos you have access to
  - private/unlisted videos that require your account
  - videos requiring confirmation or login

The app looks for `cookies.txt` in the same folder as the application.

You can import cookies using:
- **Import cookies** button in the main window
- **Tools > Import cookies.txt**

Recommended browser extension:
- **Get cookies.txt LOCALLY**

Basic steps:
1. Install the browser extension.
2. Sign in to YouTube in your browser.
3. Export cookies for YouTube.
4. Import the exported `cookies.txt` file in this app.
5. Enable **Bypass age restrictions** before downloading.

## Optional

### Portable Mode
- Portable mode saves settings to the application folder instead of the user's AppData folder.
- Useful when running from a USB drive or portable folder.
- Enable it from:
  - **Tools > Portable Mode**

### Dark Theme
- Optional visual theme.
- Enable it from:
  - **Tools > Dark Theme**

## Dependency Status

You can check the current dependency status inside the app by clicking:
- **Status** button

The status window reports:
- Node.js installed/not found
- ffmpeg.exe found/not found
- cookies.txt found/not found
- yt-dlp.exe found/not found
- application folder
- settings mode

## Quick Setup Checklist

For the best experience, make sure these are available:

- `yt-dlp.exe` in the app folder
- `ffmpeg.exe` in the app folder
- Node.js installed
- `cookies.txt` imported if downloading age-restricted/account-restricted videos

For normal public videos, `yt-dlp.exe` and `ffmpeg.exe` are usually enough.
