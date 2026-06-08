# 📌 Quick Reference - Release Process

## 🚀 ONE-MINUTE RELEASE

```cmd
1. Run: QuickBuild.bat
2. Go to: bin\Release\net10.0-windows\win-x64\publish\
3. Copy all files to: YouTubeDownloader-v1.0.0\
4. Add: README.md + COOKIE_INSTRUCTIONS.md
5. ZIP the folder
6. Upload and share!
```

## 📝 User Instructions (Copy & Paste)

```
YouTube Downloader v1.0.0

REQUIREMENTS:
- Windows 10/11
- Node.js (https://nodejs.org/)

INSTALLATION:
1. Extract ZIP
2. Install Node.js
3. Run YouTubeDownloader.exe

USAGE:
1. Paste YouTube URL
2. Select quality
3. Click Download
```

## 🔗 File Locations

| File/Folder | Location |
|-------------|----------|
| Built files | `bin\Release\net10.0-windows\win-x64\publish\` |
| Documentation | Project root |
| Build script | `QuickBuild.bat` |
| Release guides | All `.md` files in root |

## 📦 Package Checklist

- [ ] YouTubeDownloader.exe
- [ ] yt-dlp.exe
- [ ] yt-dl_logo.png
- [ ] All .dll files
- [ ] README.md
- [ ] COOKIE_INSTRUCTIONS.md

## 🎯 Distribution Options

1. **GitHub Releases** - Best for open source
2. **Google Drive** - Easiest for sharing
3. **Your Website** - Most professional
4. **Direct Send** - For specific users

## ⚡ Common Commands

```bash
# Build release
QuickBuild.bat

# Manual build
dotnet publish -c Release -r win-x64 --self-contained true

# Create ZIP (PowerShell)
Compress-Archive -Path "FolderName" -DestinationPath "output.zip"

# Git setup
git init
git add .
git commit -m "v1.0.0"
git push
```

## 🆘 Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Build fails | Check .NET 10 is installed |
| Missing files | Ensure yt-dlp.exe and logo are in project |
| Won't run on other PC | Verify self-contained publish |
| Node.js not detected | User needs to install Node.js |
| No audio/video merge | User needs ffmpeg.exe |

## 📱 Support Template

```
Thanks for using YouTube Downloader!

Common issues:
1. Node.js required: nodejs.org
2. ffmpeg.exe for audio+video
3. cookies.txt for age-restricted

See README.md for full help.
```

## 🎉 Success Checklist

- [ ] App built successfully
- [ ] Tested on your computer
- [ ] All files packaged
- [ ] ZIP created
- [ ] Documentation included
- [ ] Upload location chosen
- [ ] Ready to share!

---

**Everything you need is ready. Time to release!** 🚀
