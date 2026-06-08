# Release Checklist v1.0.0

Use this checklist to ensure a quality release.

## Pre-Build Checklist

### Code Quality
- [ ] All features working as expected
- [ ] No debug code or console writes left in
- [ ] Error messages are user-friendly
- [ ] All UI text is spelled correctly
- [ ] Logo displays correctly

### Project Configuration
- [ ] Version number is correct in `yt-dl.csproj`
- [ ] Assembly name is set to "YouTubeDownloader"
- [ ] Product name is correct
- [ ] Copyright year is current

### Documentation
- [ ] README.md is up to date
- [ ] COOKIE_INSTRUCTIONS.md is accurate
- [ ] All documentation reflects current features
- [ ] Screenshots are current (if any)

## Build Process

### Using PowerShell Script (Recommended)
- [ ] Run `.\BuildRelease.ps1`
- [ ] Script completes without errors
- [ ] Release-Package folder is created
- [ ] ZIP file is generated

### Using Batch File (Simple)
- [ ] Run `QuickBuild.bat`
- [ ] Build completes successfully
- [ ] Navigate to publish folder
- [ ] Manually create distribution package

### Manual Build (Advanced)
- [ ] Set configuration to "Release"
- [ ] Build solution successfully
- [ ] Publish with correct settings
- [ ] Collect all files manually

## Testing

### Basic Functionality Test
- [ ] Extract fresh copy to test folder
- [ ] Run YouTubeDownloader.exe
- [ ] Application starts without errors
- [ ] Logo displays
- [ ] All controls are visible
- [ ] Window size is correct

### Node.js Detection
- [ ] Status check correctly detects Node.js
- [ ] Warning appears if Node.js not installed
- [ ] Help links work

### Download Test
- [ ] Paste a YouTube URL
- [ ] Thumbnail loads automatically
- [ ] Select different quality options
- [ ] Browse button selects folder
- [ ] Download completes successfully
- [ ] File is saved to correct location

### Quality Options Test
- [ ] Best Quality works
- [ ] 1440p works (if available)
- [ ] 1080p works
- [ ] 720p works
- [ ] 480p works
- [ ] 360p works
- [ ] Audio Only works
- [ ] MP3 Audio works

### Edge Cases
- [ ] Works without ffmpeg (shows warning)
- [ ] Works without cookies.txt
- [ ] Handles invalid URLs gracefully
- [ ] Handles network errors gracefully
- [ ] Custom filename works
- [ ] Custom filename with special characters works

### Age-Restricted Video Test (Optional)
- [ ] Checkbox enables authentication
- [ ] Help link opens instructions
- [ ] Works with cookies.txt
- [ ] Shows appropriate errors without cookies

### Clean Machine Test (Important!)
- [ ] Test on computer WITHOUT .NET installed
- [ ] Test on fresh Windows 10 install (VM recommended)
- [ ] Test on fresh Windows 11 install (VM recommended)
- [ ] Verify self-contained deployment works

## Package Verification

### File Check
- [ ] YouTubeDownloader.exe present
- [ ] yt-dlp.exe present
- [ ] yt-dl_logo.png present
- [ ] All .dll files present
- [ ] README.md present
- [ ] COOKIE_INSTRUCTIONS.md present
- [ ] QUICKSTART.txt present (if included)

### File Sizes
- [ ] Total package is reasonable size (60-100 MB for self-contained)
- [ ] YouTubeDownloader.exe is correct size
- [ ] No debug files (.pdb) included (unless intentional)

### Archive Check
- [ ] ZIP file opens correctly
- [ ] All files extract properly
- [ ] Folder structure is correct
- [ ] No unnecessary files included

## Distribution Preparation

### Documentation Review
- [ ] README has correct version number
- [ ] Installation instructions are clear
- [ ] Requirements are listed
- [ ] Screenshots are up to date (if any)
- [ ] Links are correct and working

### Legal Check
- [ ] License file included (if applicable)
- [ ] Copyright notices are correct
- [ ] Third-party licenses mentioned (yt-dlp, ffmpeg)
- [ ] Disclaimer about YouTube ToS included

### Release Notes
- [ ] Changelog is written
- [ ] New features are listed
- [ ] Bug fixes are listed
- [ ] Known issues are listed
- [ ] Credits are given (if applicable)

## Distribution

### GitHub Release (Recommended)
- [ ] Repository is created
- [ ] Code is pushed to GitHub
- [ ] Release is created with tag v1.0.0
- [ ] ZIP file is uploaded
- [ ] Release notes are added
- [ ] Release is published

### Direct Distribution
- [ ] ZIP file is uploaded to hosting
- [ ] Download link is tested
- [ ] Link is shared with users
- [ ] Download counter is set up (optional)

### Promotion (Optional)
- [ ] Social media post prepared
- [ ] Reddit post prepared (check subreddit rules)
- [ ] Blog post written
- [ ] Email list notified (if applicable)

## Post-Release

### Monitoring
- [ ] Watch for bug reports
- [ ] Monitor download counts
- [ ] Check GitHub issues
- [ ] Respond to user feedback

### User Support
- [ ] GitHub Discussions enabled
- [ ] Issue templates created
- [ ] FAQ section created
- [ ] Support email set up (optional)

### Future Planning
- [ ] Ideas for next version documented
- [ ] Feature requests tracked
- [ ] Bug fix priorities set

## Security

### Virus Scan
- [ ] Scanned with Windows Defender
- [ ] Scanned with VirusTotal (optional)
- [ ] No false positives (or documented if any)

### Code Signing (Optional but Recommended)
- [ ] Executable is digitally signed
- [ ] Certificate is valid
- [ ] Signature verifies correctly

*Note: Code signing requires a certificate ($100-$400/year) but significantly improves user trust and reduces SmartScreen warnings.*

## Version-Specific Notes

### For v1.0.0 (Initial Release)
- [ ] This is first public release
- [ ] Extra testing on multiple machines
- [ ] Consider beta testing period
- [ ] Gather initial user feedback before promoting widely

### For Future Updates
- [ ] Previous version downloaded and saved
- [ ] Migration path tested (if data/settings involved)
- [ ] Backward compatibility considered
- [ ] Update instructions written

## Rollback Plan

If major issues are found:
- [ ] Know how to unpublish release
- [ ] Have previous version available
- [ ] Communication plan for users
- [ ] Bug fix priority list

## Success Criteria

Release is ready when:
- [ ] All checkboxes above are complete
- [ ] Tested on at least 2 different computers
- [ ] No critical bugs remain
- [ ] Documentation is complete
- [ ] Package is properly created
- [ ] Distribution method is ready

---

## Quick Reference Commands

### Build Release:
```powershell
.\BuildRelease.ps1
```
or
```cmd
QuickBuild.bat
```

### Manual Publish:
```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

### Create ZIP:
```powershell
Compress-Archive -Path "YouTubeDownloader" -DestinationPath "YouTubeDownloader-v1.0.0.zip"
```

---

**Sign and date when complete:**

Released by: _________________

Date: _________________

Version: v1.0.0

Tested on machines: _________________

Issues found: _________________

Notes: _________________
