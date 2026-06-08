# Distribution Guide

This guide explains how to package and distribute YouTube Downloader to others.

## Quick Release Process

1. **Run the build script:**
   ```powershell
   .\BuildRelease.ps1
   ```

2. **Test the release:**
   - Run `Release-Package\YouTubeDownloader\YouTubeDownloader.exe`
   - Verify all features work
   - Test on a clean machine if possible

3. **Distribute** using one of the methods below

---

## Distribution Methods

### Method 1: Portable ZIP (Recommended for Most Users)

**File:** `YouTubeDownloader-v1.0.0-Portable.zip`

**Advantages:**
- No installation needed
- Works on any Windows 10/11 PC
- Can run from USB drive
- No administrator rights required

**Share this file on:**
- GitHub Releases
- Google Drive / OneDrive
- Your website
- Direct download link

**User instructions:**
1. Download and extract ZIP
2. Install Node.js from https://nodejs.org/
3. Run `YouTubeDownloader.exe`

---

### Method 2: Installer Package

**Files:** 
- Entire `Release-Package` folder
- Must include `INSTALL.bat`

**Advantages:**
- Installs to Program Files
- Creates desktop shortcut option
- Feels more "professional"

**Requires:**
- Administrator rights to install
- User must be comfortable with .bat files

**Distribution:**
- Package entire folder as ZIP
- User extracts and runs `INSTALL.bat` as Administrator

---

### Method 3: Microsoft Store (Future)

For wider distribution, consider publishing to Microsoft Store:
- Requires developer account ($19 one-time fee)
- Automatic updates
- Built-in trust and security
- See: https://developer.microsoft.com/en-us/microsoft-store/register

---

## GitHub Releases (Recommended)

### Step 1: Create GitHub Repository

1. Go to https://github.com/new
2. Name it "youtube-downloader" or similar
3. Make it Public
4. Initialize with README (use your existing README.md)

### Step 2: Push Your Code

```bash
git init
git add .
git commit -m "Initial release v1.0.0"
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/youtube-downloader.git
git push -u origin main
```

### Step 3: Create Release

1. Go to your repository
2. Click "Releases" → "Create a new release"
3. Tag version: `v1.0.0`
4. Release title: `YouTube Downloader v1.0.0`
5. Description (use template below)
6. Upload: `YouTubeDownloader-v1.0.0-Portable.zip`
7. Publish release

### Release Description Template:

```markdown
# YouTube Downloader v1.0.0

A simple, user-friendly Windows application for downloading YouTube videos with quality selection.

## ✨ Features

- 📥 Download videos in multiple qualities (1440p, 1080p, 720p, 480p, 360p)
- 🎵 Audio-only download option (MP3)
- 🖼️ Thumbnail preview before download
- 🔒 Age-restricted video support (with cookies)
- 📊 System status checks (Node.js, ffmpeg, cookies)
- 🎯 Custom filename option
- 🚀 Fast and lightweight

## 📦 Download

**[Download YouTubeDownloader-v1.0.0-Portable.zip](link-to-zip)**

## ⚙️ Requirements

### Essential (Required):
- **Windows 10 or 11** (64-bit)
- **Node.js** - [Download here](https://nodejs.org/) (Get LTS version)
  - **MUST install Node.js or downloads will fail!**

### Recommended:
- **ffmpeg.exe** - [Download here](https://ffmpeg.org/download.html)
  - Place in app folder for video+audio merging

### Optional:
- **cookies.txt** - For age-restricted videos
  - See included instructions

## 🚀 Quick Start

1. Extract ZIP to any folder
2. **Install Node.js** (critical step!)
3. Run `YouTubeDownloader.exe`
4. Click "Status" to verify Node.js is installed
5. Paste YouTube URL and download!

## 📖 Documentation

- See `README.md` for full documentation
- See `COOKIE_INSTRUCTIONS.md` for age-restricted videos
- See `QUICKSTART.txt` for quick reference

## 🐛 Known Issues

- Some videos may not have all quality options (YouTube limitation)
- 4K downloads not available (YouTube restrictions)
- Requires active internet connection

## 🔄 Updating

### Update yt-dlp (recommended monthly):
1. Download latest from [yt-dlp releases](https://github.com/yt-dlp/yt-dlp/releases)
2. Replace `yt-dlp.exe` in app folder

## 📝 Changelog

### v1.0.0 (2025-01-XX)
- Initial release
- Multiple quality options
- Thumbnail preview
- System status checks
- Age-restricted video support
- Custom naming

## 🤝 Support

- Issues: [GitHub Issues](https://github.com/YOUR_USERNAME/youtube-downloader/issues)
- Questions: [Discussions](https://github.com/YOUR_USERNAME/youtube-downloader/discussions)

## ⚖️ License

This project uses:
- [yt-dlp](https://github.com/yt-dlp/yt-dlp) - Unlicense
- [ffmpeg](https://ffmpeg.org/) - LGPL/GPL

---

**Note:** This tool is for personal use only. Respect YouTube's Terms of Service and copyright laws.
```

---

## File Hosting Options

### Free Options:
1. **GitHub Releases** (Recommended)
   - 2GB per file limit
   - Unlimited downloads
   - Version tracking
   - Professional appearance

2. **Google Drive**
   - Easy to share
   - Anyone with link can download
   - May require Google account

3. **OneDrive**
   - Similar to Google Drive
   - 15GB free storage

4. **MediaFire / Mega**
   - Popular for software distribution
   - No account required to download

### Paid Options:
1. **Your own website**
   - Full control
   - Custom domain
   - Professional appearance

2. **DigitalOcean Spaces / AWS S3**
   - CDN distribution
   - Fast downloads worldwide
   - Pay per download

---

## Important Legal Notes

### Include These Disclaimers:

1. **In README.md:**
```
## Legal Notice

This software is provided for personal use only. Users are responsible for complying with:
- YouTube's Terms of Service
- Copyright laws in their jurisdiction
- Content creators' rights

Downloading copyrighted content without permission may be illegal in your country.
```

2. **In your release notes:**
```
⚠️ Legal Disclaimer: This tool is for personal use only. Respect copyright laws and YouTube's Terms of Service.
```

---

## Pre-Release Checklist

Before distributing:

- [ ] Test on clean Windows 10 machine
- [ ] Test on clean Windows 11 machine
- [ ] Verify Node.js detection works
- [ ] Test without ffmpeg (should warn but not crash)
- [ ] Test with age-restricted video
- [ ] Test all quality options
- [ ] Verify thumbnail preview loads
- [ ] Test custom filename feature
- [ ] Check all documentation is included
- [ ] Scan with antivirus (to ensure clean)
- [ ] Sign executable (optional but recommended)

---

## Version Numbering

Use semantic versioning: `MAJOR.MINOR.PATCH`

- **MAJOR**: Breaking changes (e.g., 2.0.0)
- **MINOR**: New features (e.g., 1.1.0)
- **PATCH**: Bug fixes (e.g., 1.0.1)

Examples:
- `v1.0.0` - Initial release
- `v1.0.1` - Fixed Node.js detection bug
- `v1.1.0` - Added playlist support
- `v2.0.0` - Complete UI redesign

---

## Promotion Tips

1. **Reddit:**
   - r/software
   - r/OpenSourceSoftware
   - r/youtube (check rules first)

2. **Social Media:**
   - Share on Twitter/X with screenshots
   - LinkedIn if targeting professionals
   - Facebook groups for tech enthusiasts

3. **Tech Blogs:**
   - Write a blog post about your app
   - Submit to sites like ProductHunt
   - List on AlternativeTo.net

4. **Word of Mouth:**
   - Ask friends to test and share
   - Post in Discord servers
   - Share in tech forums

---

## Support Strategy

1. **GitHub Issues** - Bug reports
2. **GitHub Discussions** - Questions and help
3. **Email** - For private inquiries
4. **FAQ Section** - In README.md

---

## Updating Your Release

When you make changes:

1. Update version in `yt-dl.csproj`
2. Run `.\BuildRelease.ps1`
3. Test thoroughly
4. Create new GitHub release
5. Update README with changelog
6. Notify users (if you have a list)

---

## Success Metrics

Track these to measure adoption:
- GitHub stars
- Download counts
- Issue reports (means people are using it!)
- Feature requests
- Community contributions
