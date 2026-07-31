# YouTube Downloader - Embedded Build Guide

## What's Embedded vs What Users Provide

### ✅ Embedded in Application (Automatic)
- **yt-dlp.exe** - YouTube downloader engine (~18 MB)
- **ffmpeg.exe** - Video/audio merger (~100 MB, if included in build)
- **Application logo** - UI branding

### 📤 User Must Provide (Optional)
- **cookies.txt** - Only needed for age-restricted videos
- Place in: Application executable directory

### 🔧 User Must Install (Required)
- **Node.js** - Required for YouTube downloads
- Download from: https://nodejs.org/

---

## How Embedding Works

###  First Run Behavior
When the application starts for the first time:

1. **Automatic Extraction**
   - yt-dlp.exe and ffmpeg.exe are extracted from the application
   - Files are placed in: `%LocalAppData%\YouTubeDownloader\tools\`
   - Extraction happens in the background
   - Status message shows during extraction

2. **Tools Location**
   ```
   C:\Users\YourName\AppData\Local\YouTubeDownloader\tools\
   ├── yt-dlp.exe
   └── ffmpeg.exe
   ```

3. **cookies.txt Location** (user-provided, optional)
   ```
   Same folder as YouTubeDownloader.exe
   ```

### Benefits of This Approach

✅ **Single Executable Distribution**
- One .exe file (with dependencies)
- No separate folders needed
- Clean installation

✅ **Automatic Updates**
- Tools extracted on first run
- Can be updated by replacing embedded resources
- Users don't need to manually update yt-dlp

✅ **Clean Uninstall**
- Tools stored in AppData
- Easy to clean up
- No scattered files

✅ **Smaller Initial Download**
- Only one package to distribute
- All tools included

---

## Building with Embedded Resources

### Step 1: Add ffmpeg.exe to Project

1. Download ffmpeg:
   - Go to: https://ffmpeg.org/download.html
   - Get Windows "essentials" build
   - Extract `ffmpeg.exe` from the archive

2. Place in project root:
   ```
   O:\development\visualstudio\csharp_projects\yt-dl\
   ├── ffmpeg.exe          ← Add this file
   ├── yt-dlp.exe         ← Already there
   ├── yt-dl_logo.png
   └── ...
   ```

3. Build automatically embeds it

### Step 2: Build Release

```cmd
QuickBuild.bat
```

or

```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

### Step 3: Distribute

**What to share:**
- `YouTubeDownloader.exe` (from publish folder)
- All `.dll` files from publish folder
- `yt-dl_logo.png`
- Documentation (README.md, etc.)

**NOT needed separately:**
- yt-dlp.exe (embedded)
- ffmpeg.exe (embedded)

---

## User Instructions (Updated)

### Installation

1. **Download and extract the application**
   - Extract all files to a folder

2. **Install Node.js** (Required!)
   - Download from: https://nodejs.org/
   - Get LTS version
   - Restart computer after install

3. **Run YouTubeDownloader.exe**
   - On first run, tools will extract automatically
   - Wait for "Ready" status

4. **(Optional) Add cookies.txt for age-restricted videos**
   - Export from browser using "Get cookies.txt LOCALLY" extension
   - Place in same folder as YouTubeDownloader.exe

### System Requirements

**Required:**
- Windows 10 or 11 (64-bit)
- Node.js (install separately)
- ~200 MB disk space for tools

**Included:**
- yt-dlp.exe (embedded)
- ffmpeg.exe (embedded)
- .NET 10 runtime (self-contained)

---

## Distribution Package Structure

```
YouTubeDownloader-v1.0.0/
├── YouTubeDownloader.exe          Main application (~5 MB)
├── yt-dl_logo.png                Application logo
├── System.*.dll                   .NET runtime (~60 MB)
├── README.md                      User guide
├── COOKIE_INSTRUCTIONS.md         Cookie setup guide
└── QUICKSTART.txt                Quick reference

Internal (embedded in YouTubeDownloader.exe):
├── yt-dlp.exe                    (~18 MB embedded)
└── ffmpeg.exe                    (~100 MB embedded if added)

Total package size:
- Without ffmpeg: ~85 MB
- With ffmpeg: ~185 MB
```

---

## Updating yt-dlp or ffmpeg

### For End Users:
No manual update needed! Tools are embedded in the application.

### For Developers:

1. Download latest yt-dlp.exe or ffmpeg.exe
2. Replace file in project root
3. Rebuild application
4. Release new version

Users get updated tools automatically with new app version.

---

## Troubleshooting

### "yt-dlp.exe not found" Error

**Cause:** Extraction failed or blocked by antivirus

**Fix:**
1. Check: `%LocalAppData%\YouTubeDownloader\tools\`
2. If folder empty, extraction was blocked
3. Add exception in antivirus
4. Restart application

### "ffmpeg.exe missing" Warning

**Cause:** ffmpeg wasn't added to project before building

**Fix for developers:**
1. Add ffmpeg.exe to project root
2. Rebuild application

**Fix for users:**
- Downloads will work but only video OR audio (not both)
- Update to newer version when available

### Application is Very Large

**Cause:** Self-contained with embedded tools

**Explanation:**
- yt-dlp.exe: ~18 MB
- ffmpeg.exe: ~100 MB
- .NET runtime: ~60 MB
- **Total: ~180-190 MB**

This is normal for a fully self-contained application.

**Alternatives:**
- Framework-dependent (requires .NET installed): ~120 MB
- Without embedded ffmpeg: ~85 MB

---

## Advanced: Build Configurations

### Option 1: Embed Everything (Recommended)
```xml
<ItemGroup>
  <EmbeddedResource Include="yt-dlp.exe" />
  <EmbeddedResource Include="ffmpeg.exe" Condition="Exists('ffmpeg.exe')" />
</ItemGroup>
```

**Pros:** Complete package, works everywhere  
**Cons:** Larger download (~185 MB)

### Option 2: Embed Only yt-dlp
```xml
<ItemGroup>
  <EmbeddedResource Include="yt-dlp.exe" />
  <!-- ffmpeg.exe not embedded -->
</ItemGroup>
```

**Pros:** Smaller download (~85 MB)  
**Cons:** Audio-only or video-only downloads

### Option 3: No Embedding (Old Method)
```xml
<ItemGroup>
  <None Update="yt-dlp.exe">
	<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
  <None Update="ffmpeg.exe">
	<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

**Pros:** Modular, easy to update  
**Cons:** Multiple files to distribute

---

## Security Considerations

### Antivirus False Positives

**Why it happens:**
- yt-dlp and ffmpeg are flagged by some antivirus
- Executables embedded in another executable can trigger warnings
- Self-extracting behavior mimics malware

**Solutions:**

1. **Code Signing** (Best)
   - Get code signing certificate ($100-400/year)
   - Sign YouTubeDownloader.exe
   - Dramatically reduces false positives

2. **Submit to Antivirus Vendors**
   - Upload to VirusTotal
   - Submit false positive reports
   - Major vendors will whitelist

3. **User Instructions**
   - Document that warnings are false positives
   - Explain why they occur
   - Provide VirusTotal link

### Validate Embedded Resources

Add hash checking to ensure integrity:

```csharp
// TODO: Add SHA256 validation after extraction
private bool ValidateExtractedFile(string path, string expectedHash)
{
	using var sha256 = SHA256.Create();
	using var stream = File.OpenRead(path);
	var hash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "");
	return hash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase);
}
```

---

## FAQ

**Q: Why embed instead of separate files?**  
A: Simpler distribution, single package, automatic extraction, professional appearance.

**Q: Can users update yt-dlp themselves?**  
A: No, it's embedded. They need a new app version. This is by design for consistency.

**Q: What if ffmpeg isn't embedded?**  
A: Downloads will work for video-only or audio-only, but not merged.

**Q: Where are tools extracted to?**  
A: `%LocalAppData%\YouTubeDownloader\tools\` - standard Windows app data location.

**Q: How do I add ffmpeg to my build?**  
A: Download ffmpeg.exe, place in project root, rebuild. It auto-embeds if present.

**Q: Will this work on all Windows versions?**  
A: Yes, Windows 10 and 11 (64-bit). .NET 10 runtime is included.

---

## Migration from Separate Files

If you previously distributed files separately:

### Old Structure:
```
YouTubeDownloader\
├── YouTubeDownloader.exe
├── yt-dlp.exe          ← Separate
├── ffmpeg.exe          ← Separate
└── cookies.txt
```

### New Structure:
```
YouTubeDownloader\
├── YouTubeDownloader.exe  ← Everything embedded
└── cookies.txt            ← Still separate

Tools auto-extract to:
%LocalAppData%\YouTubeDownloader\tools\
```

### User Impact:
- **Old version:** Manual file management
- **New version:** Automatic, clean, professional
- **cookies.txt:** Still manual (intentional)

---

**This approach gives you a professional, self-contained application while keeping cookies.txt as a user-provided configuration file.**
