# YouTube Downloader

A Windows desktop application for downloading YouTube videos with quality selection and batch download support.

## Features

✅ **Quality Selection**
- Best Quality (Default)
- 1440p (2K)
- 1080p (Full HD)
- 720p (HD)
- 480p / 360p
- Audio Only (Best Quality / MP3 320k)

✅ **Video Preview**
- Automatic thumbnail preview when pasting URLs
- Verify you have the correct video at a glance

✅ **System Status Checks**
- Automatic detection of Node.js
- Checks for cookies.txt file
- Verifies ffmpeg presence

✅ **Age-Restricted Content**
- Support for age-restricted videos using cookies.txt
- Clear instructions for cookie setup

✅ **Custom Filenames**
- Optional custom filename for downloads
- Defaults to video title if not specified

## Requirements

### Essential (Required)
1. **Node.js** - Download from https://nodejs.org/
   - Required for YouTube's anti-bot protection
   - Get the LTS version (v22+)
   - Restart application after installing

2. **yt-dlp.exe** - Already included in the application folder
   - Update periodically from: https://github.com/yt-dlp/yt-dlp/releases

### Recommended
3. **ffmpeg.exe** - Download from https://ffmpeg.org/download.html
   - Required to merge video and audio streams
   - Without it, you'll only get video OR audio
   - Place in the same folder as the application

### Optional
4. **cookies.txt** - Only needed for age-restricted videos
   - See `COOKIE_INSTRUCTIONS.md` for setup guide
   - Export using browser extension "Get cookies.txt LOCALLY"

## How to Use

### Video Download
1. Paste a YouTube URL in the "YouTube URL" field
2. Verify the thumbnail preview (loads automatically)
3. Select your desired quality
4. Choose output folder (or use default Downloads folder)
5. (Optional) Enter a custom filename
6. Click "Download"

### Age-Restricted Videos
1. Enable "Bypass age restrictions" checkbox
2. Click "Help" link for cookie setup instructions
3. Export cookies.txt from your browser
4. Place cookies.txt in application folder
5. Download as normal

## Status Indicators

The application shows real-time status for:
- **Node.js**: ✓ Installed / ✗ Not Found
- **cookies.txt**: ✓ Found / ⚠ Not Found

Click "Check Status" for detailed system information.

## Troubleshooting

### "Node.js is NOT detected"
- Install Node.js from https://nodejs.org/
- Restart the application after installation
- Most downloads will fail without Node.js

### "ffmpeg is required to merge video and audio"
- Download ffmpeg from https://ffmpeg.org/download.html
- Extract ffmpeg.exe from the zip
- Place in the application folder

### "n challenge" or YouTube anti-bot errors
- Ensure Node.js is installed
- Wait 10-30 minutes (YouTube rate limiting)
- Update yt-dlp.exe to the latest version
- Try a different quality setting

### Age-restricted videos fail
- Follow cookie setup instructions in `COOKIE_INSTRUCTIONS.md`
- Ensure you're logged in to YouTube when exporting cookies
- Export fresh cookies (they expire over time)
- Some videos may be blocked by YouTube's strictest protections

### Downloads are video-only or audio-only
- Install ffmpeg.exe in the application folder
- The app will automatically use it to merge streams

## File Locations

All required files should be in the same folder as the application:

```
[App Folder]/
├── yt-dl.exe (this application)
├── yt-dlp.exe (included)
├── ffmpeg.exe (download separately)
├── cookies.txt (export from browser if needed)
├── COOKIE_INSTRUCTIONS.md
└── README.md
```

To find your application folder, click "Check Status" in the app.

## Tips

- **Quality Selection**: "Best Quality" automatically picks the best available format
- **Thumbnails**: The preview updates as soon as you paste a URL
- **Custom Names**: Leave filename blank to use the video's original title
- **First Run**: Click "Check Status" to verify all components are ready
- **Quality Note**: Some videos may not have all quality options available. The downloader will get the closest available quality.

## Updates

### yt-dlp Updates
YouTube frequently changes their systems. Update yt-dlp.exe regularly:
1. Download latest from: https://github.com/yt-dlp/yt-dlp/releases
2. Replace yt-dlp.exe in the application folder
3. Restart the application

### Cookie Refresh
If age-restricted downloads stop working:
1. Re-export cookies.txt from your browser
2. Replace the old cookies.txt file
3. Try downloading again

## License

This application uses:
- **yt-dlp** - https://github.com/yt-dlp/yt-dlp
- **ffmpeg** - https://ffmpeg.org/

## Support

For issues with:
- **This Application**: Check status indicators and troubleshooting section
- **yt-dlp**: Visit https://github.com/yt-dlp/yt-dlp/issues
- **Node.js**: Visit https://nodejs.org/
- **ffmpeg**: Visit https://ffmpeg.org/
