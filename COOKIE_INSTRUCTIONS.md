# Cookie Setup Instructions

## Why You Need cookies.txt

YouTube requires authentication to download age-restricted videos. This application needs your cookies to prove you're logged in and old enough to watch these videos.

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
   - Move the `cookies.txt` file to the same folder as this application
   - The application will automatically find and use it

### Method 2: Manual Cookie Export

If you're comfortable with browser developer tools, you can manually export cookies from your browser's developer console.

## File Location

Place `cookies.txt` in:
```
[Application Folder]\cookies.txt
```

To find your application folder, click "Check Status" in the app.

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
- Update yt-dlp.exe to the latest version from: https://github.com/yt-dlp/yt-dlp/releases

## Required Components

This application needs these components to function:

1. **Node.js** (Required)
   - Download from: https://nodejs.org/
   - Get the LTS version (currently v22+)
   - Restart the application after installing

2. **ffmpeg.exe** (Required for video+audio)
   - Download from: https://ffmpeg.org/download.html
   - Extract ffmpeg.exe from the zip
   - Place in the same folder as this application

3. **cookies.txt** (Optional - for age-restricted videos)
   - Follow the instructions above
   - Only needed for restricted content
