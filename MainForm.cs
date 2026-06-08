using System.Diagnostics;

namespace yt_dl
{
    public partial class MainForm : Form
    {
        private string? _cookiesPath;
        private bool _isDownloading = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeQualityOptions();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckSystemStatus();
            LoadLogo();
        }

        private void LoadLogo()
        {
            try
            {
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dl_logo.png");
                if (File.Exists(logoPath))
                {
                    picLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch
            {
                // Logo is optional, continue without it
            }
        }

        private void InitializeQualityOptions()
        {
            cmbQuality.Items.AddRange(new object[]
            {
                "Best Quality (Default)",
                "1440p (2K)",
                "1080p (Full HD)",
                "720p (HD)",
                "480p",
                "360p",
                "Audio Only (Best)",
                "Audio Only (MP3 320k)"
            });
            cmbQuality.SelectedIndex = 0;

            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            txtOutput.Text = downloadsPath;

            _cookiesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookies.txt");
        }

        private void CheckSystemStatus()
        {
            // Check Node.js
            bool hasNodeJs = CheckForNodeJs();
            lblNodeStatus.Text = hasNodeJs
                ? "Node.js: ✓ Installed"
                : "Node.js: ✗ Not Found (Required!)";
            lblNodeStatus.ForeColor = hasNodeJs ? Color.Green : Color.Red;

            // Check cookies.txt
            bool hasCookies = File.Exists(_cookiesPath);
            lblCookieStatus.Text = hasCookies
                ? "cookies.txt: ✓ Found"
                : "cookies.txt: ⚠ Not Found (for age-restricted)";
            lblCookieStatus.ForeColor = hasCookies ? Color.Green : Color.Orange;
        }

        private void btnCheckStatus_Click(object sender, EventArgs e)
        {
            CheckSystemStatus();
            MessageBox.Show(
                $"System Status:\n\n" +
                $"Node.js: {(CheckForNodeJs() ? "✓ Installed" : "✗ Not Found")}\n" +
                $"ffmpeg.exe: {(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe")) ? "✓ Found" : "✗ Not Found")}\n" +
                $"cookies.txt: {(File.Exists(_cookiesPath) ? "✓ Found" : "✗ Not Found")}\n" +
                $"yt-dlp.exe: {(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe")) ? "✓ Found" : "✗ Not Found")}\n\n" +
                $"App Folder:\n{AppDomain.CurrentDomain.BaseDirectory}",
                "System Status",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text) ||
                (!txtUrl.Text.Contains("youtube.com") && !txtUrl.Text.Contains("youtu.be")))
            {
                picThumbnail.Image = null;
                return;
            }

            await LoadThumbnail(txtUrl.Text);
        }

        private async Task LoadThumbnail(string url)
        {
            try
            {
                string videoId = ExtractVideoId(url);
                if (string.IsNullOrEmpty(videoId)) return;

                string thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/mqdefault.jpg";

                using var client = new HttpClient();
                var imageBytes = await client.GetByteArrayAsync(thumbnailUrl);
                using var ms = new MemoryStream(imageBytes);
                picThumbnail.Image = Image.FromStream(ms);
            }
            catch
            {
                picThumbnail.Image = null;
            }
        }

        private string ExtractVideoId(string url)
        {
            try
            {
                if (url.Contains("youtu.be/"))
                {
                    return url.Split("youtu.be/")[1].Split('?')[0].Split('&')[0];
                }
                else if (url.Contains("youtube.com/watch") && url.Contains("v="))
                {
                    var vIndex = url.IndexOf("v=");
                    if (vIndex >= 0)
                    {
                        var videoId = url.Substring(vIndex + 2);
                        var ampIndex = videoId.IndexOf('&');
                        return ampIndex >= 0 ? videoId.Substring(0, ampIndex) : videoId;
                    }
                }
            }
            catch { }
            return "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var folderBrowser = new FolderBrowserDialog
            {
                Description = "Select output folder for downloaded videos",
                UseDescriptionForTitle = true,
                SelectedPath = txtOutput.Text
            };

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = folderBrowser.SelectedPath;
            }
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpMessage = @"To download age-restricted videos:

Method 1: Use a browser extension to export cookies
1. Install 'Get cookies.txt LOCALLY' extension for your browser
   - Chrome/Edge: Search in Web Store
   - Firefox: Search in Add-ons
2. Go to YouTube.com (while logged in)
3. Click the extension icon and save cookies.txt
4. Place cookies.txt in the same folder as this application
5. Enable 'Bypass age restrictions' and download

Method 2: Try without browser encryption (may work)
- Just enable the checkbox and try downloading
- If it fails, use Method 1

The app will look for 'cookies.txt' in its folder automatically.";

            MessageBox.Show(helpMessage, "Help - Age Restricted Videos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string DetectInstalledBrowser()
        {
            string[] browsers = { "edge", "firefox", "chrome", "brave", "opera" };

            foreach (string browser in browsers)
            {
                try
                {
                    string testPath = browser.ToLower() switch
                    {
                        "edge" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                              @"Microsoft\Edge\User Data"),
                        "firefox" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                  @"Mozilla\Firefox\Profiles"),
                        "chrome" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                 @"Google\Chrome\User Data"),
                        "brave" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                @"BraveSoftware\Brave-Browser\User Data"),
                        "opera" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                @"Opera Software\Opera Stable"),
                        _ => ""
                    };

                    if (Directory.Exists(testPath))
                    {
                        return browser;
                    }
                }
                catch { }
            }

            return "edge";
        }

        private bool CheckForNodeJs()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process != null)
                {
                    process.WaitForExit(2000);
                    return process.ExitCode == 0;
                }
            }
            catch { }

            return false;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                MessageBox.Show("Download already in progress!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Please enter a YouTube URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOutput.Text) || !Directory.Exists(txtOutput.Text))
            {
                MessageBox.Show("Please select a valid output folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isDownloading = true;
            btnDownload.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;

            try
            {
                await DownloadVideo(txtUrl.Text);
                lblStatus.Text = "Download complete!";
                MessageBox.Show("Download complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Download failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Download failed.";
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                btnDownload.Enabled = true;
                _isDownloading = false;
            }
        }

        private async Task DownloadVideo(string url)
        {
            string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");

            if (!File.Exists(ytDlpPath))
            {
                throw new FileNotFoundException("yt-dlp.exe not found in the application directory.");
            }

            // Check for Node.js
            bool hasNodeJs = CheckForNodeJs();
            if (!hasNodeJs)
            {
                var result = MessageBox.Show(
                    "Node.js is NOT detected on your system.\n\n" +
                    "Node.js is REQUIRED to download from YouTube (especially age-restricted videos).\n" +
                    "Without it, most downloads will fail with 'n challenge' errors.\n\n" +
                    "Would you like to continue anyway?\n\n" +
                    "To fix: Install Node.js from https://nodejs.org/\n" +
                    "(Download the LTS version, restart this app after installing)",
                    "Node.js Required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    throw new OperationCanceledException("Download cancelled - Node.js required.");
                }
            }

            // Check for ffmpeg
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            if (!File.Exists(ffmpegPath))
            {
                var result = MessageBox.Show(
                    "ffmpeg.exe is required to merge video and audio streams.\n\n" +
                    "Without it, you'll only get video OR audio, not both.\n\n" +
                    "Would you like to continue anyway?\n\n" +
                    "(Place ffmpeg.exe in the same folder as this application to enable full video+audio downloads)",
                    "ffmpeg Not Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    throw new OperationCanceledException("Download cancelled - ffmpeg required.");
                }
            }

            string qualityArgs = GetQualityArguments();

            // Handle custom filename
            string outputTemplate;
            if (!string.IsNullOrWhiteSpace(txtFilename.Text))
            {
                string sanitizedFilename = string.Join("_", txtFilename.Text.Split(Path.GetInvalidFileNameChars()));
                outputTemplate = Path.Combine(txtOutput.Text, $"{sanitizedFilename}.%(ext)s");
            }
            else
            {
                outputTemplate = Path.Combine(txtOutput.Text, "%(title)s.%(ext)s");
            }

            // Add ffmpeg location if it exists
            string ffmpegArgs = "";
            if (File.Exists(ffmpegPath))
            {
                ffmpegArgs = $"--ffmpeg-location \"{ffmpegPath}\"";
            }

            // Add Node.js runtime and other arguments
            string extraArgs = hasNodeJs
                ? "--js-runtimes node --extractor-args \"youtube:player_client=android,web\" --no-check-certificates"
                : "--extractor-args \"youtube:player_client=android,web\" --no-check-certificates";

            string authArgs = "";
            if (chkBypassRestrictions.Checked)
            {
                if (File.Exists(_cookiesPath))
                {
                    authArgs = $"--cookies \"{_cookiesPath}\"";
                    lblStatus.Text = "Using cookies.txt for authentication...";
                }
                else
                {
                    string browser = DetectInstalledBrowser();
                    authArgs = $"--cookies-from-browser {browser}:default --compat-options no-cookies-cleanup";
                    lblStatus.Text = $"Attempting to use {browser} cookies...";
                }
            }

            var argsList = new List<string> { qualityArgs };
            argsList.Add(extraArgs);
            if (!string.IsNullOrEmpty(ffmpegArgs))
                argsList.Add(ffmpegArgs);
            if (!string.IsNullOrEmpty(authArgs))
                argsList.Add(authArgs);
            argsList.Add($"-o \"{outputTemplate}\"");
            argsList.Add($"\"{url}\"");

            string allArguments = string.Join(" ", argsList);

            var startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                Arguments = allArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };

            var output = new System.Text.StringBuilder();
            var error = new System.Text.StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    output.AppendLine(e.Data);
                    this.Invoke(() => lblStatus.Text = e.Data.Length > 50 ? e.Data.Substring(0, 50) + "..." : e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    error.AppendLine(e.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string errorText = error.ToString();
                string detailedMessage = $"yt-dlp exited with code {process.ExitCode}.";

                if (errorText.Contains("n challenge") || errorText.Contains("nsig") || errorText.Contains("Some formats may be missing") || errorText.Contains("JavaScript runtime"))
                {
                    bool hasNode = CheckForNodeJs();
                    detailedMessage += "\n\nYouTube anti-bot protection (n challenge) is active.\n\n";

                    if (!hasNode)
                    {
                        detailedMessage += "**CRITICAL: Node.js is NOT installed!**\n" +
                                         "Node.js is REQUIRED to solve YouTube's n challenge.\n\n" +
                                         "TO FIX:\n" +
                                         "1. Download Node.js from: https://nodejs.org/\n" +
                                         "   (Get the LTS version - currently v22.x or later)\n" +
                                         "2. Install Node.js (default settings are fine)\n" +
                                         "3. RESTART this application\n" +
                                         "4. Try downloading again\n\n";
                    }
                    else
                    {
                        detailedMessage += "Node.js is installed, but the challenge still failed.\n\n" +
                                         "Other things to try:\n" +
                                         "1. Wait 10-30 minutes (YouTube may be rate-limiting)\n" +
                                         "2. Use a different quality setting\n" +
                                         "3. Ensure cookies.txt is fresh (re-export from browser)\n" +
                                         "4. Update to yt-dlp nightly build:\n" +
                                         "   https://github.com/yt-dlp/yt-dlp-nightly-builds/releases\n\n";
                    }

                    detailedMessage += "Note: Some age-restricted videos cannot be downloaded\n" +
                                     "due to YouTube's strict protection.";
                }
                else if (errorText.Contains("ffmpeg") || errorText.Contains("Requested formats are incompatible"))
                {
                    detailedMessage += "\n\nffmpeg is required to merge video and audio.\n\n" +
                                     "To fix:\n" +
                                     "1. Download ffmpeg from: https://ffmpeg.org/download.html\n" +
                                     "   (For Windows, get the 'essentials' build)\n" +
                                     "2. Extract ffmpeg.exe from the zip file\n" +
                                     "3. Place ffmpeg.exe in this folder:\n" +
                                     $"   {AppDomain.CurrentDomain.BaseDirectory}\n" +
                                     "4. Try downloading again";
                }
                else if (errorText.Contains("DPAPI") || errorText.Contains("decrypt"))
                {
                    detailedMessage += "\n\nBrowser cookie encryption error. To fix:\n\n" +
                                     "1. Install browser extension 'Get cookies.txt LOCALLY'\n" +
                                     "2. Go to YouTube.com (logged in)\n" +
                                     "3. Click extension, save cookies.txt\n" +
                                     "4. Place cookies.txt in this app's folder:\n" +
                                     $"   {AppDomain.CurrentDomain.BaseDirectory}\n" +
                                     "5. Try downloading again\n\n" +
                                     "Click 'Help' link in the app for more details.";
                }
                else if (errorText.Contains("Sign in to confirm your age") || errorText.Contains("age"))
                {
                    detailedMessage += "\n\nThis video is age-restricted.\n" +
                                     "Enable 'Bypass age restrictions' and follow Help instructions.";
                }
                else if (errorText.Contains("cookies"))
                {
                    detailedMessage += "\n\nCookie error. Please export cookies.txt file.\n" +
                                     "Click 'Help' link for instructions.";
                }

                detailedMessage += $"\n\nError details:\n{errorText}";
                throw new Exception(detailedMessage);
            }
        }

        private string GetQualityArguments()
        {
            return cmbQuality.SelectedIndex switch
            {
                // Best Quality - no restrictions
                0 => "-f \"bestvideo+bestaudio/best\"",

                // 1440p (2K) - prefer 1440p, but allow higher if available
                1 => "-f \"bestvideo[height>=1440]+bestaudio/bestvideo[height>=1080]+bestaudio/best\" -S \"res:1440\"",

                // 1080p (Full HD) - get 1080p or higher, prefer exactly 1080p
                2 => "-f \"bestvideo[height>=1080]+bestaudio/bestvideo+bestaudio/best\" -S \"res:1080\"",

                // 720p (HD) - get 720p or higher, prefer exactly 720p
                3 => "-f \"bestvideo[height>=720]+bestaudio/bestvideo+bestaudio/best\" -S \"res:720\"",

                // 480p - get 480p or higher, prefer exactly 480p
                4 => "-f \"bestvideo[height>=480]+bestaudio/bestvideo+bestaudio/best\" -S \"res:480\"",

                // 360p - get 360p or higher, prefer exactly 360p
                5 => "-f \"bestvideo[height>=360]+bestaudio/bestvideo+bestaudio/best\" -S \"res:360\"",

                // Audio only options
                6 => "-f \"bestaudio\" -x",
                7 => "-f \"bestaudio\" -x --audio-format mp3 --audio-quality 320K",

                _ => "-f \"bestvideo+bestaudio/best\""
            };
        }
    }
}
