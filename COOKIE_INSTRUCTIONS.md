# Cookie Setup Instructions

## Why You Need cookies.txt

YouTube requires authentication to download age-restricted videos. This application needs your cookies to prove you're logged in and old enough to watch these videos.

## ✅ What's Already Included

The application comes with everything you need **except** cookies.txt:
- ✅ yt-dlp.exe (embedded in application)
- ✅ ffmpeg.exe (embedded in application)
- ✅ .NET runtime (included)
- ⚠️ cookies.txt (YOU must provide for age-restricted videos)
- ⚠️ Node.js (YOU must install separately)

## How to Export cookies.txt

### Method 1: Browser Extension (Recommended)

1. **Install the Extension**
   - Chrome/Edge: Search for "Get cookies.txt LOCALLY" in the Chrome Web Store
   - Firefox: Search for "cookies.txt" in Firefox Add-ons
   - **Important**: Use the "LOCALLY" version for security

2. **Log In to YouTube**
   - Go to https://www.youtube.com
   - Make sure you're logged in to your account

3. **Export Cookies**
   - Click the extension icon while on YouTube.com
   - Click "Export" or "Download"
   - Save the file as `cookies.txt`

4. **Place cookies.txt**
   - Put `cookies.txt` in the same folder as YouTubeDownloader.exe
   - The application will automatically find and use it

### Method 2: Manual Cookie Export

If you're comfortable with browser developer tools, you can manually export cookies from your browser's developer console.

## File Location

Place `cookies.txt` in the same folder as `YouTubeDownloader.exe`:

```
YouTubeDownloader\
├── YouTubeDownloader.exe
├── cookies.txt           ← Place here
└── other DLL files...
```

To find your application folder, click "Status" in the app - it will show the path.

## What About yt-dlp and ffmpeg?

**You don't need to download these!**

- yt-dlp.exe and ffmpeg.exe are embedded in the application
- They extract automatically on first run
- You only need to provide cookies.txt

## Security Notes

- **Never share your cookies.txt file** - it contains your login information
- Cookies expire after a while (usually weeks/months)
- If downloads stop working, try exporting a fresh cookies.txt file
- The extension only exports cookies for the current site (YouTube)

## Troubleshooting

**Downloads fail even with cookies.txt:**
- Make sure the file is named exactly `cookies.txt` (not `cookies.txt.txt`)
- Try exporting fresh cookies
- Verify you're logged in to YouTube when exporting
- Check that Node.js is installed (required for YouTube downloads)

**Age-restricted videos still won't download:**
- Some videos are blocked by YouTube's strictest protections
- Try waiting 10-30 minutes and try again (rate limiting)
- Update to the latest version of the application (includes updated yt-dlp)

**"cookies.txt not found" warning:**
- This is normal if you haven't added it yet
- You only need cookies.txt for age-restricted videos
- Regular videos work without it

## Required Components

This application needs these components to function:

### 1. Node.js (Required - User Install)
   - Download from: https://nodejs.org/
   - Get the LTS version (currently v22+)
   - Restart computer after installing
   - **Without Node.js, downloads will fail!**

### 2. cookies.txt (Optional - User Provides)
   - Follow the instructions above
   - Only needed for age-restricted content
   - Place in application folder

### 3. yt-dlp & ffmpeg (Included - Automatic)
   - Embedded in the application
   - Extract automatically on first run
   - No user action needed

## First Time Setup Checklist

- [ ] Install Node.js from https://nodejs.org/
- [ ] Restart your computer
- [ ] Run YouTubeDownloader.exe (tools extract automatically)
- [ ] Click "Status" button to verify Node.js is detected
- [ ] (Optional) Export and add cookies.txt for age-restricted videos

---

**That's it!** With Node.js installed and optionally cookies.txt added, you're ready to download YouTube videos.
