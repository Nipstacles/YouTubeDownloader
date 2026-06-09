using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace yt_dl
{
    public partial class MainForm : Form
    {
        private string? _cookiesPath;
        private bool _isDownloading = false;
        private List<string> _downloadLog = new List<string>();
        private Process? _currentDownloadProcess;
        private bool _downloadCancelled;
        private CancellationTokenSource? _previewCancellation;
        private ComboBox cmbFilenameTemplate = null!;
        private Label lblYtDlpStatus = null!;
        private Label lblFfmpegStatus = null!;
        private Label lblMetadata = null!;
        private CheckBox chkDownloadPlaylist = null!;
        private Button btnCancel = null!;
        private Button btnOpenOutputFolder = null!;
        private Button btnCheckForUpdates = null!;
        private Label lblAppVersion = null!;
        private Button btnImportCookies = null!;
        private ToolStripMenuItem importCookiesToolStripMenuItem = null!;
        private ToolStripMenuItem locateFfmpegToolStripMenuItem = null!;
        private ToolStripMenuItem openAppFolderToolStripMenuItem = null!;
        private ToolStripMenuItem portableModeToolStripMenuItem = null!;
        private ToolStripMenuItem darkThemeToolStripMenuItem = null!;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem = null!;
        private CheckBox chkCheckForUpdatesOnStartup = null!;
        private bool _portableMode;
        private bool _darkTheme;
        private bool _checkForUpdatesOnStartup = true;
        private string _settingsPath = "";
        private static readonly Regex DownloadProgressRegex = new(@"\[download\]\s+(?<percent>\d+(?:\.\d+)?)%(?:\s+of\s+(?<size>\S+))?(?:\s+at\s+(?<speed>\S+))?(?:\s+ETA\s+(?<eta>\S+))?", RegexOptions.Compiled);
        private const string GitHubLatestReleaseUrl = "https://api.github.com/repos/Nipstacles/YouTubeDownloader/releases/latest";

        public MainForm()
        {
            InitializeComponent();
            InitializeQualityOptions();
            InitializeEnhancedUi();
            Text = $"YouTube Downloader v{GetCurrentApplicationVersionDisplay()}";
            LoadSettings();
            ApplyTheme();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckSystemStatus();
            LoadLogo();

            if (_checkForUpdatesOnStartup)
            {
                BeginInvoke(new Action(async () => await CheckForApplicationUpdates(showUpToDateMessage: false)));
            }
        }

        private void LoadLogo()
        {
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dl_logo.ico");
                if (File.Exists(iconPath))
                {
                    Icon = new Icon(iconPath);
                }

                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dl_logofull.png");
                if (!File.Exists(logoPath))
                {
                    logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dl_logo.png");
                }

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
                "Best Quality",
                "1440p (2K) or higher",
                "1080p (Full HD) or higher",
                "720p (HD) or higher",
                "480p or higher",
                "360p or higher",
                "Audio Only (Best Quality)",
                "Audio Only (MP3 320k)"
            });
            cmbQuality.SelectedIndex = 0;

            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            txtOutput.Text = downloadsPath;

            _cookiesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookies.txt");
        }

        private void InitializeEnhancedUi()
        {
            cmbFilenameTemplate = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(125, 323),
                Name = "cmbFilenameTemplate",
                Size = new Size(372, 23)
            };
            cmbFilenameTemplate.Items.AddRange(new object[]
            {
                "Title only",
                "Uploader - Title",
                "Upload date - Title",
                "Playlist index - Title",
                "Custom filename field"
            });
            cmbFilenameTemplate.SelectedIndex = 0;

            var lblTemplate = new Label
            {
                AutoSize = true,
                Location = new Point(12, 326),
                Name = "lblTemplate",
                Text = "Filename Template:"
            };

            chkDownloadPlaylist = new CheckBox
            {
                AutoSize = true,
                Location = new Point(414, 228),
                Name = "chkDownloadPlaylist",
                Size = new Size(158, 19),
                Text = "Download full playlist",
                UseVisualStyleBackColor = true
            };

            btnImportCookies = new Button
            {
                Location = new Point(330, 354),
                Name = "btnImportCookies",
                Size = new Size(105, 23),
                Text = "Import cookies",
                UseVisualStyleBackColor = true
            };
            btnImportCookies.Click += btnImportCookies_Click;

            btnOpenOutputFolder = new Button
            {
                Location = new Point(441, 354),
                Name = "btnOpenOutputFolder",
                Size = new Size(131, 23),
                Text = "Open Output Folder",
                UseVisualStyleBackColor = true
            };
            btnOpenOutputFolder.Click += btnOpenOutputFolder_Click;

            btnCheckForUpdates = new Button
            {
                Location = new Point(436, 380),
                Name = "btnCheckForUpdates",
                Size = new Size(136, 23),
                Text = "Check for Updates",
                UseVisualStyleBackColor = true
            };
            btnCheckForUpdates.Click += btnCheckForUpdates_Click;

            chkCheckForUpdatesOnStartup = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Location = new Point(12, 380),
                Name = "chkCheckForUpdatesOnStartup",
                Size = new Size(185, 19),
                Text = "Check for updates on startup",
                UseVisualStyleBackColor = true
            };
            chkCheckForUpdatesOnStartup.CheckedChanged += chkCheckForUpdatesOnStartup_CheckedChanged;

            lblYtDlpStatus = new Label
            {
                AutoSize = true,
                Location = new Point(184, 408),
                Name = "lblYtDlpStatus",
                Text = "yt-dlp: Checking..."
            };

            lblFfmpegStatus = new Label
            {
                AutoSize = true,
                Location = new Point(184, 433),
                Name = "lblFfmpegStatus",
                Text = "ffmpeg: Checking..."
            };

            lblMetadata = new Label
            {
                AutoSize = false,
                Location = new Point(184, 503),
                Name = "lblMetadata",
                Size = new Size(388, 50),
                Text = "Paste a YouTube URL to load title, channel, duration, and resolution info."
            };

            btnCancel = new Button
            {
                Enabled = false,
                Location = new Point(495, 589),
                Name = "btnCancel",
                Size = new Size(77, 23),
                Text = "Cancel",
                Visible = false,
                UseVisualStyleBackColor = true
            };
            btnCancel.Click += btnCancel_Click;

            importCookiesToolStripMenuItem = new ToolStripMenuItem("Import cookies.txt", null, btnImportCookies_Click);
            locateFfmpegToolStripMenuItem = new ToolStripMenuItem("Locate ffmpeg.exe", null, locateFfmpegToolStripMenuItem_Click);
            openAppFolderToolStripMenuItem = new ToolStripMenuItem("Open App Folder", null, openAppFolderToolStripMenuItem_Click);
            portableModeToolStripMenuItem = new ToolStripMenuItem("Portable Mode", null, portableModeToolStripMenuItem_Click) { CheckOnClick = true };
            darkThemeToolStripMenuItem = new ToolStripMenuItem("Dark Theme", null, darkThemeToolStripMenuItem_Click) { CheckOnClick = true };
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem("Check for App Updates", null, checkForUpdatesToolStripMenuItem_Click);
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                importCookiesToolStripMenuItem,
                locateFfmpegToolStripMenuItem,
                openAppFolderToolStripMenuItem,
                new ToolStripSeparator(),
                checkForUpdatesToolStripMenuItem,
                new ToolStripSeparator(),
                portableModeToolStripMenuItem,
                darkThemeToolStripMenuItem
            });

            Controls.Add(lblTemplate);
            Controls.Add(cmbFilenameTemplate);
            Controls.Add(chkDownloadPlaylist);
            Controls.Add(lblAppVersion);
            Controls.Add(btnImportCookies);
            Controls.Add(btnOpenOutputFolder);
            Controls.Add(btnCheckForUpdates);
            Controls.Add(chkCheckForUpdatesOnStartup);
            Controls.Add(lblYtDlpStatus);
            Controls.Add(lblFfmpegStatus);
            Controls.Add(lblMetadata);
            Controls.Add(btnCancel);

            chkBypassRestrictions.Location = new Point(12, 354);
            linkLabelHelp.Location = new Point(290, 355);
            picThumbnail.Location = new Point(12, 408);
            lblNodeStatus.Location = new Point(184, 458);
            lblCookieStatus.Location = new Point(184, 480);
            lblStatus.Location = new Point(12, 559);
            progressBar.Location = new Point(12, 577);
            progressBar.Size = new Size(477, 23);
            btnDownload.Location = new Point(495, 559);
            ClientSize = new Size(584, 624);
        }

        private void CheckSystemStatus()
        {
            bool hasNodeJs = CheckForNodeJs();
            lblNodeStatus.Text = hasNodeJs
                ? "Node.js: ✓ Installed"
                : "Node.js: ✗ Not Found (Required!)";
            lblNodeStatus.ForeColor = hasNodeJs ? Color.Green : Color.Red;

            bool hasCookies = File.Exists(_cookiesPath);
            lblCookieStatus.Text = hasCookies
                ? "cookies.txt: ✓ Found"
                : "cookies.txt: ⚠ Not Found (for age-restricted)";
            lblCookieStatus.ForeColor = hasCookies ? Color.Green : Color.Orange;

            string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
            bool hasYtDlp = File.Exists(ytDlpPath);
            lblYtDlpStatus.Text = hasYtDlp ? $"yt-dlp: ✓ {GetToolVersion(ytDlpPath, "--version")}" : "yt-dlp: ✗ Not Found";
            lblYtDlpStatus.ForeColor = hasYtDlp ? Color.Green : Color.Red;

            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            bool hasFfmpeg = File.Exists(ffmpegPath);
            lblFfmpegStatus.Text = hasFfmpeg ? "ffmpeg: ✓ Found" : "ffmpeg: ✗ Not Found";
            lblFfmpegStatus.ForeColor = hasFfmpeg ? Color.Green : Color.Red;
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
                $"App Version: {GetCurrentApplicationVersionDisplay()}\n" +
                $"App Folder:\n{AppDomain.CurrentDomain.BaseDirectory}\n\n" +
                $"Settings Mode: {(_portableMode ? "Portable" : "User AppData")}",
                "System Status",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async void txtUrl_TextChanged(object sender, EventArgs e)
        {
            _previewCancellation?.Cancel();

            if (string.IsNullOrWhiteSpace(txtUrl.Text) ||
                (!txtUrl.Text.Contains("youtube.com") && !txtUrl.Text.Contains("youtu.be")))
            {
                picThumbnail.Image = null;
                lblMetadata.Text = "Paste a YouTube URL to load title, channel, duration, and resolution info.";
                return;
            }

            _previewCancellation = new CancellationTokenSource();
            var token = _previewCancellation.Token;

            try
            {
                await Task.Delay(500, token);
                await LoadThumbnail(txtUrl.Text);
                await LoadMetadata(txtUrl.Text, token);
            }
            catch (OperationCanceledException) { }
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

        private async Task LoadMetadata(string url, CancellationToken cancellationToken)
        {
            string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
            if (!File.Exists(ytDlpPath))
            {
                lblMetadata.Text = "Metadata unavailable: yt-dlp.exe was not found.";
                return;
            }

            try
            {
                lblMetadata.Text = "Loading video metadata...";
                var startInfo = new ProcessStartInfo
                {
                    FileName = ytDlpPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                startInfo.ArgumentList.Add("--dump-json");
                startInfo.ArgumentList.Add("--skip-download");
                if (!chkDownloadPlaylist.Checked)
                {
                    startInfo.ArgumentList.Add("--no-playlist");
                }
                startInfo.ArgumentList.Add(url);

                using var process = new Process { StartInfo = startInfo };
                process.Start();
                string json = await process.StandardOutput.ReadToEndAsync(cancellationToken);
                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode != 0 || string.IsNullOrWhiteSpace(json))
                {
                    lblMetadata.Text = "Metadata unavailable for this URL.";
                    return;
                }

                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                string title = GetJsonString(root, "title", "Unknown title");
                string uploader = GetJsonString(root, "uploader", "Unknown channel");
                string duration = FormatDuration(GetJsonDouble(root, "duration"));
                int height = GetMaxFormatHeight(root);

                if (!cancellationToken.IsCancellationRequested)
                {
                    lblMetadata.Text = $"Title: {title}\nChannel: {uploader}    Duration: {duration}    Max: {(height > 0 ? height + "p" : "Unknown")}";
                    if (!_isDownloading)
                    {
                        lblStatus.Text = title.Length > 80 ? $"Ready: {title[..80]}..." : $"Ready: {title}";
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch
            {
                lblMetadata.Text = "Metadata unavailable for this URL.";
            }
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

        private static string GetToolVersion(string fileName, string arguments)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    return "Found";
                }

                string output = process.StandardOutput.ReadLine() ?? "Found";
                process.WaitForExit(2000);
                return output.Trim();
            }
            catch
            {
                return "Found";
            }
        }

        private void btnImportCookies_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Import cookies.txt",
                Filter = "Cookies file (*.txt)|*.txt|All files (*.*)|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookies.txt");
                File.Copy(dialog.FileName, destination, overwrite: true);
                _cookiesPath = destination;
                chkBypassRestrictions.Checked = true;
                CheckSystemStatus();
                MessageBox.Show("cookies.txt was imported successfully.", "Import cookies", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not import cookies.txt:\n{ex.Message}", "Import cookies", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenOutputFolder_Click(object? sender, EventArgs e)
        {
            OpenFolder(txtOutput.Text);
        }

        private void openAppFolderToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            OpenFolder(AppDomain.CurrentDomain.BaseDirectory);
        }

        private void locateFfmpegToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Locate ffmpeg.exe",
                Filter = "ffmpeg.exe|ffmpeg.exe|Executable files (*.exe)|*.exe|All files (*.*)|*.*",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
                File.Copy(dialog.FileName, destination, overwrite: true);
                CheckSystemStatus();
                MessageBox.Show("ffmpeg.exe was copied to the app folder.", "Locate ffmpeg", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not copy ffmpeg.exe:\n{ex.Message}", "Locate ffmpeg", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void OpenFolder(string folderPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                {
                    MessageBox.Show("The folder does not exist.", "Open Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open folder:\n{ex.Message}", "Open Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            CancelCurrentDownload();
        }

        private void CancelCurrentDownload()
        {
            if (_currentDownloadProcess == null || _currentDownloadProcess.HasExited)
            {
                return;
            }

            _downloadCancelled = true;
            lblStatus.Text = "Cancelling download...";

            try
            {
                _currentDownloadProcess.Kill(entireProcessTree: true);
            }
            catch { }
        }

        private void portableModeToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _portableMode = portableModeToolStripMenuItem.Checked;
            SaveSettings();
            MessageBox.Show($"Portable mode is now {(_portableMode ? "enabled" : "disabled")}. Settings will be saved to the {(_portableMode ? "app folder" : "user AppData folder")}.", "Portable Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void darkThemeToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _darkTheme = darkThemeToolStripMenuItem.Checked;
            ApplyTheme();
            SaveSettings();
        }

        private async void checkForUpdatesToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            await CheckForApplicationUpdates(showUpToDateMessage: true);
        }

        private async void btnCheckForUpdates_Click(object? sender, EventArgs e)
        {
            await CheckForApplicationUpdates(showUpToDateMessage: true);
        }

        private void chkCheckForUpdatesOnStartup_CheckedChanged(object? sender, EventArgs e)
        {
            _checkForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked;
            SaveSettings();
        }

        private void LoadSettings()
        {
            string portablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouTubeDownloader", "settings.json");
            _settingsPath = File.Exists(portablePath) ? portablePath : appDataPath;

            try
            {
                if (File.Exists(_settingsPath))
                {
                    AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_settingsPath));
                    if (settings != null)
                    {
                        _portableMode = settings.PortableMode;
                        _darkTheme = settings.DarkTheme;
                        if (!string.IsNullOrWhiteSpace(settings.OutputFolder) && Directory.Exists(settings.OutputFolder))
                        {
                            txtOutput.Text = settings.OutputFolder;
                        }
                        cmbQuality.SelectedIndex = Math.Clamp(settings.QualityIndex, 0, cmbQuality.Items.Count - 1);
                        cmbFilenameTemplate.SelectedIndex = Math.Clamp(settings.TemplateIndex, 0, cmbFilenameTemplate.Items.Count - 1);
                        chkBypassRestrictions.Checked = settings.BypassRestrictions;
                        chkDownloadPlaylist.Checked = settings.DownloadPlaylist;
                        _checkForUpdatesOnStartup = settings.CheckForUpdatesOnStartup;
                    }
                }
            }
            catch { }

            portableModeToolStripMenuItem.Checked = _portableMode;
            darkThemeToolStripMenuItem.Checked = _darkTheme;
            chkCheckForUpdatesOnStartup.Checked = _checkForUpdatesOnStartup;
        }

        private void SaveSettings()
        {
            try
            {
                _settingsPath = _portableMode
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json")
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YouTubeDownloader", "settings.json");

                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!);

                var settings = new AppSettings
                {
                    PortableMode = _portableMode,
                    DarkTheme = _darkTheme,
                    OutputFolder = txtOutput.Text,
                    QualityIndex = cmbQuality.SelectedIndex,
                    TemplateIndex = cmbFilenameTemplate.SelectedIndex,
                    BypassRestrictions = chkBypassRestrictions.Checked,
                    DownloadPlaylist = chkDownloadPlaylist.Checked,
                    CheckForUpdatesOnStartup = _checkForUpdatesOnStartup
                };

                File.WriteAllText(_settingsPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch { }
        }

        private async Task CheckForApplicationUpdates(bool showUpToDateMessage)
        {
            if (_isDownloading)
            {
                if (showUpToDateMessage)
                {
                    MessageBox.Show("Please wait for the current download to finish before checking for updates.", "Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            checkForUpdatesToolStripMenuItem.Enabled = false;
            btnCheckForUpdates.Enabled = false;
            string previousStatus = lblStatus.Text;
            lblStatus.Text = "Checking for app updates...";

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("YouTubeDownloader/1.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

                using HttpResponseMessage response = await client.GetAsync(GitHubLatestReleaseUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(CreateGitHubUpdateErrorMessage(response.StatusCode));
                }

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using JsonDocument document = await JsonDocument.ParseAsync(stream);
                JsonElement root = document.RootElement;

                string latestTag = GetJsonString(root, "tag_name", "");
                string latestName = GetJsonString(root, "name", latestTag);
                string releaseUrl = GetJsonString(root, "html_url", "https://github.com/Nipstacles/YouTubeDownloader/releases/latest");

                if (string.IsNullOrWhiteSpace(latestTag))
                {
                    throw new InvalidOperationException("The latest release did not include a version tag.");
                }

                Version currentVersion = GetCurrentApplicationVersion();
                Version? latestVersion = TryParseVersion(latestTag);

                if (latestVersion != null && latestVersion > currentVersion)
                {
                    lblStatus.Text = $"App update available: {latestTag}";
                    DialogResult result = MessageBox.Show(
                        $"A newer version of YouTube Downloader is available.\n\nCurrent version: {currentVersion}\nLatest version: {latestName}\n\nOpen the GitHub release page now?",
                        "Update Available",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = releaseUrl,
                            UseShellExecute = true
                        });
                    }
                }
                else
                {
                    lblStatus.Text = "YouTube Downloader is up to date.";
                    if (showUpToDateMessage)
                    {
                        MessageBox.Show("YouTube Downloader is up to date.", "Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        lblStatus.Text = previousStatus;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = previousStatus;
                if (showUpToDateMessage)
                {
                    MessageBox.Show($"Could not check for app updates:\n{ex.Message}", "Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                checkForUpdatesToolStripMenuItem.Enabled = true;
                btnCheckForUpdates.Enabled = true;
            }
        }

        private static Version GetCurrentApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0);
        }

        private static string GetCurrentApplicationVersionDisplay()
        {
            Version currentVersion = GetCurrentApplicationVersion();
            return currentVersion.Revision > 0
                ? currentVersion.ToString()
                : currentVersion.Build > 0
                    ? currentVersion.ToString(3)
                    : currentVersion.ToString(2);
        }

        private static string CreateGitHubUpdateErrorMessage(System.Net.HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                System.Net.HttpStatusCode.NotFound =>
                    "GitHub returned 404. Confirm the repository is public and that a published GitHub Release exists with a version tag like v1.0.1.",
                System.Net.HttpStatusCode.Unauthorized =>
                    "GitHub rejected the update check request.",
                System.Net.HttpStatusCode.Forbidden =>
                    "GitHub refused the request. The API rate limit may have been reached.",
                _ => $"GitHub returned {(int)statusCode} ({statusCode})."
            };
        }

        private static Version? TryParseVersion(string version)
        {
            string normalized = version.Trim().TrimStart('v', 'V');
            int suffixIndex = normalized.IndexOfAny(new[] { '-', '+' });
            if (suffixIndex >= 0)
            {
                normalized = normalized[..suffixIndex];
            }

            return Version.TryParse(normalized, out Version? parsedVersion) ? parsedVersion : null;
        }

        private void ApplyTheme()
        {
            Color backColor = _darkTheme ? Color.FromArgb(32, 32, 32) : SystemColors.Control;
            Color foreColor = _darkTheme ? Color.WhiteSmoke : SystemColors.ControlText;
            Color inputBackColor = _darkTheme ? Color.FromArgb(45, 45, 45) : SystemColors.Window;
            Color inputForeColor = _darkTheme ? Color.WhiteSmoke : SystemColors.WindowText;

            ApplyThemeToControl(this, backColor, foreColor, inputBackColor, inputForeColor);
            menuStrip.BackColor = backColor;
            menuStrip.ForeColor = foreColor;
        }

        private static void ApplyThemeToControl(Control control, Color backColor, Color foreColor, Color inputBackColor, Color inputForeColor)
        {
            control.BackColor = control is TextBox or ComboBox ? inputBackColor : backColor;
            control.ForeColor = control is TextBox or ComboBox ? inputForeColor : foreColor;

            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child, backColor, foreColor, inputBackColor, inputForeColor);
            }
        }

        private static string GetJsonString(JsonElement element, string propertyName, string fallback)
        {
            return element.TryGetProperty(propertyName, out JsonElement value) && value.ValueKind == JsonValueKind.String
                ? value.GetString() ?? fallback
                : fallback;
        }

        private static double GetJsonDouble(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement value) && value.TryGetDouble(out double result)
                ? result
                : 0;
        }

        private static int GetMaxFormatHeight(JsonElement root)
        {
            int maxHeight = 0;
            if (!root.TryGetProperty("formats", out JsonElement formats) || formats.ValueKind != JsonValueKind.Array)
            {
                return maxHeight;
            }

            foreach (JsonElement format in formats.EnumerateArray())
            {
                if (format.TryGetProperty("height", out JsonElement heightElement) && heightElement.TryGetInt32(out int height))
                {
                    maxHeight = Math.Max(maxHeight, height);
                }
            }

            return maxHeight;
        }

        private static string FormatDuration(double seconds)
        {
            if (seconds <= 0)
            {
                return "Unknown";
            }

            TimeSpan duration = TimeSpan.FromSeconds(seconds);
            return duration.TotalHours >= 1
                ? duration.ToString(@"h\:mm\:ss")
                : duration.ToString(@"m\:ss");
        }

        private async void updateYtDlpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                MessageBox.Show("Please wait for the current download to finish before updating yt-dlp.", "Update yt-dlp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            updateYtDlpToolStripMenuItem.Enabled = false;
            btnDownload.Enabled = false;
            btnListFormats.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            lblStatus.Text = "Updating yt-dlp...";

            try
            {
                string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe.download");
                string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe.bak");

                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("YouTubeDownloader/1.0");

                byte[] latestYtDlp = await client.GetByteArrayAsync("https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe");
                await File.WriteAllBytesAsync(tempPath, latestYtDlp);

                if (new FileInfo(tempPath).Length == 0)
                {
                    throw new InvalidOperationException("Downloaded yt-dlp.exe was empty.");
                }

                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }

                if (File.Exists(ytDlpPath))
                {
                    File.Move(ytDlpPath, backupPath);
                }

                File.Move(tempPath, ytDlpPath);

                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }

                lblStatus.Text = "yt-dlp updated successfully.";
                CheckSystemStatus();
                MessageBox.Show("yt-dlp.exe was updated successfully.", "Update yt-dlp", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe.download");
                string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe.bak");

                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }

                    if (!File.Exists(ytDlpPath) && File.Exists(backupPath))
                    {
                        File.Move(backupPath, ytDlpPath);
                    }
                }
                catch { }

                lblStatus.Text = "yt-dlp update failed.";
                MessageBox.Show($"Failed to update yt-dlp.exe:\n{ex.Message}", "Update yt-dlp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                updateYtDlpToolStripMenuItem.Enabled = true;
                btnDownload.Enabled = true;
                btnListFormats.Enabled = true;
            }
        }

        private void ShowDownloadLog()
        {
            Form logForm = new Form
            {
                Text = "Download Log",
                Width = 900,
                Height = 700,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.Sizable
            };

            TextBox txtLog = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9),
                Text = string.Join(Environment.NewLine, _downloadLog),
                ReadOnly = true,
                WordWrap = false
            };

            Button btnCopy = new Button
            {
                Text = "Copy to Clipboard",
                Dock = DockStyle.Bottom,
                Height = 30
            };

            btnCopy.Click += (s, e) =>
            {
                Clipboard.SetText(txtLog.Text);
                MessageBox.Show("Log copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            logForm.Controls.Add(txtLog);
            logForm.Controls.Add(btnCopy);
            logForm.ShowDialog();
        }

        private async void btnListFormats_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Please enter a YouTube URL first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
            if (!File.Exists(ytDlpPath))
            {
                MessageBox.Show("yt-dlp.exe not found in the application directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnListFormats.Enabled = false;
            lblStatus.Text = "Listing available formats...";

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = ytDlpPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                startInfo.ArgumentList.Add("-F");
                startInfo.ArgumentList.Add(txtUrl.Text);

                using var process = new Process { StartInfo = startInfo };
                var output = new System.Text.StringBuilder();

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        output.AppendLine(e.Data);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        output.AppendLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    // Show formats in a message box
                    string formatList = output.ToString();

                    // Create a simple form to display the formats with a textbox for better viewing
                    Form formatForm = new Form
                    {
                        Text = "Available Formats",
                        Width = 800,
                        Height = 600,
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.Sizable
                    };

                    TextBox txtFormats = new TextBox
                    {
                        Multiline = true,
                        ScrollBars = ScrollBars.Both,
                        Dock = DockStyle.Fill,
                        Font = new Font("Consolas", 9),
                        Text = formatList,
                        ReadOnly = true,
                        WordWrap = false
                    };

                    formatForm.Controls.Add(txtFormats);
                    formatForm.ShowDialog();

                    lblStatus.Text = "Ready";
                }
                else
                {
                    MessageBox.Show($"Failed to list formats:\n{output}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Failed to list formats";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error listing formats:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error";
            }
            finally
            {
                btnListFormats.Enabled = true;
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                CancelCurrentDownload();
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
            _downloadCancelled = false;
            btnDownload.Text = "Cancel";
            btnDownload.Enabled = true;
            progressBar.Style = ProgressBarStyle.Blocks;
            progressBar.Value = 0;
            _downloadLog.Clear();
            SaveSettings();

            try
            {
                await DownloadVideo(txtUrl.Text);
                lblStatus.Text = "Download complete!";

                var result = MessageBox.Show("Download complete!\n\nWould you like to view the download log?", 
                    "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    ShowDownloadLog();
                }
            }
            catch (Exception ex)
            {
                if (_downloadCancelled)
                {
                    MessageBox.Show("Download cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Download cancelled.";
                    await Task.Delay(1500);
                    lblStatus.Text = "Ready";
                }
                else
                {
                    MessageBox.Show($"Download failed:\n{ex.Message}\n\nView log for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Download failed.";
                    ShowDownloadLog();
                }
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                btnDownload.Enabled = true;
                btnDownload.Text = "Download";
                _isDownloading = false;
                _currentDownloadProcess = null;
            }
        }

        private string GetOutputTemplate()
        {
            if (cmbFilenameTemplate.SelectedIndex == 4 && !string.IsNullOrWhiteSpace(txtFilename.Text))
            {
                string sanitizedFilename = string.Join("_", txtFilename.Text.Split(Path.GetInvalidFileNameChars()));
                return Path.Combine(txtOutput.Text, $"{sanitizedFilename}.%(ext)s");
            }

            string template = cmbFilenameTemplate.SelectedIndex switch
            {
                1 => "%(uploader)s - %(title)s.%(ext)s",
                2 => "%(upload_date>%Y-%m-%d)s - %(title)s.%(ext)s",
                3 => "%(playlist_index)03d - %(title)s.%(ext)s",
                _ => "%(title)s.%(ext)s"
            };

            return Path.Combine(txtOutput.Text, template);
        }

        private bool UpdateDownloadProgress(string line)
        {
            Match match = DownloadProgressRegex.Match(line);
            if (!match.Success || !double.TryParse(match.Groups["percent"].Value, out double percent))
            {
                return false;
            }

            int progress = Math.Clamp((int)Math.Round(percent), 0, 100);
            string size = match.Groups["size"].Success ? match.Groups["size"].Value : "";
            string speed = match.Groups["speed"].Success ? match.Groups["speed"].Value : "";
            string eta = match.Groups["eta"].Success ? match.Groups["eta"].Value : "";

            progressBar.Value = progress;
            lblStatus.Text = $"Downloading... {progress}%" +
                (!string.IsNullOrEmpty(size) ? $" of {size}" : "") +
                (!string.IsNullOrEmpty(speed) ? $" at {speed}" : "") +
                (!string.IsNullOrEmpty(eta) ? $" ETA {eta}" : "");
            return true;
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

            var selectedQuality = GetSelectedQuality();

            string outputTemplate = GetOutputTemplate();

            var startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            AddDownloadArguments(startInfo, selectedQuality, outputTemplate, url, ffmpegPath, hasNodeJs);

            string commandLine = BuildCommandLineForLog(ytDlpPath, startInfo.ArgumentList);
            _downloadLog.Add($"[COMMAND] {commandLine}");
            Debug.WriteLine($"[yt-dlp COMMAND] {commandLine}");
            lblStatus.Text = $"Starting download with quality: {cmbQuality.SelectedItem}";

            using var process = new Process { StartInfo = startInfo };
            _currentDownloadProcess = process;

            var output = new System.Text.StringBuilder();
            var error = new System.Text.StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    output.AppendLine(e.Data);
                    _downloadLog.Add($"[OUT] {e.Data}");

                    this.Invoke(() =>
                    {
                        bool progressUpdated = UpdateDownloadProgress(e.Data);
                        string displayText = e.Data.Length > 100 ? e.Data.Substring(0, 100) + "..." : e.Data;
                        if (!progressUpdated)
                        {
                            lblStatus.Text = displayText;
                        }

                        // Log format selection to debug output
                        if (e.Data.Contains("format") || e.Data.Contains("resolution") || e.Data.Contains("Downloading"))
                        {
                            Debug.WriteLine($"[yt-dlp] {e.Data}");
                        }
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    error.AppendLine(e.Data);
                    _downloadLog.Add($"[ERR] {e.Data}");

                    this.Invoke(() =>
                    {
                        UpdateDownloadProgress(e.Data);
                    });

                    // Log important messages to debug output
                    if (e.Data.Contains("format") || e.Data.Contains("resolution") || e.Data.Contains("Merging"))
                    {
                        Debug.WriteLine($"[yt-dlp ERROR] {e.Data}");
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (_downloadCancelled)
            {
                throw new OperationCanceledException("Download cancelled.");
            }

            if (process.ExitCode != 0)
            {
                string errorText = error.ToString();
                throw new Exception(CreateFriendlyErrorMessage(errorText, process.ExitCode));
            }
        }

        private string CreateFriendlyErrorMessage(string errorText, int exitCode)
        {
            string lower = errorText.ToLowerInvariant();
            string message = $"yt-dlp exited with code {exitCode}.\n\n";

            if (lower.Contains("requested format is not available"))
            {
                message += "The selected quality is not available from the download client yt-dlp used for this video. Try Best Quality, List Formats, or a nearby quality option.";
            }
            else if (lower.Contains("ffmpeg") || lower.Contains("requested formats are incompatible"))
            {
                message += "ffmpeg is required to merge separate video and audio streams. Use Tools > Locate ffmpeg.exe, then try again.";
            }
            else if (lower.Contains("sign in") || lower.Contains("age-restricted") || lower.Contains("confirm your age"))
            {
                message += "This video requires a signed-in YouTube session. Import cookies.txt, enable bypass restrictions, and try again.";
            }
            else if (lower.Contains("cookies") || lower.Contains("dpapi") || lower.Contains("decrypt"))
            {
                message += "There was a cookie/authentication problem. Import a fresh cookies.txt file from your browser and try again.";
            }
            else if (lower.Contains("n challenge") || lower.Contains("nsig") || lower.Contains("javascript runtime"))
            {
                message += CheckForNodeJs()
                    ? "YouTube anti-bot protection blocked this request. Try updating yt-dlp, waiting a few minutes, or importing cookies."
                    : "Node.js is required for YouTube anti-bot protection. Install Node.js, restart this app, and try again.";
            }
            else if (lower.Contains("unable to download webpage") || lower.Contains("network") || lower.Contains("timed out"))
            {
                message += "A network error occurred. Check your internet connection and try again.";
            }
            else
            {
                message += "The download failed. Check the download log for the exact yt-dlp output.";
            }

            return message + $"\n\nError details:\n{errorText}";
        }

        private void AddDownloadArguments(ProcessStartInfo startInfo, QualitySelection selectedQuality, string outputTemplate, string url, string ffmpegPath, bool hasNodeJs)
        {
            startInfo.ArgumentList.Add("-f");
            startInfo.ArgumentList.Add(selectedQuality.FormatSelector);

            if (hasNodeJs)
            {
                startInfo.ArgumentList.Add("--js-runtimes");
                startInfo.ArgumentList.Add("node");
            }

            startInfo.ArgumentList.Add("--no-check-certificates");
            startInfo.ArgumentList.Add("--verbose");

            if (!chkDownloadPlaylist.Checked)
            {
                startInfo.ArgumentList.Add("--no-playlist");
            }

            if (File.Exists(ffmpegPath))
            {
                startInfo.ArgumentList.Add("--ffmpeg-location");
                startInfo.ArgumentList.Add(ffmpegPath);
            }

            if (selectedQuality.IsAudioOnly)
            {
                startInfo.ArgumentList.Add("--extract-audio");
                startInfo.ArgumentList.Add("--audio-format");
                startInfo.ArgumentList.Add(selectedQuality.AudioFormat);

                if (!string.IsNullOrEmpty(selectedQuality.AudioQuality))
                {
                    startInfo.ArgumentList.Add("--audio-quality");
                    startInfo.ArgumentList.Add(selectedQuality.AudioQuality);
                }
            }
            else
            {
                startInfo.ArgumentList.Add("--merge-output-format");
                startInfo.ArgumentList.Add("mp4");
            }

            if (chkBypassRestrictions.Checked)
            {
                if (File.Exists(_cookiesPath))
                {
                    startInfo.ArgumentList.Add("--cookies");
                    startInfo.ArgumentList.Add(_cookiesPath);
                    lblStatus.Text = "Using cookies.txt for authentication...";
                }
                else
                {
                    string browser = DetectInstalledBrowser();
                    startInfo.ArgumentList.Add("--cookies-from-browser");
                    startInfo.ArgumentList.Add($"{browser}:default");
                    startInfo.ArgumentList.Add("--compat-options");
                    startInfo.ArgumentList.Add("no-cookies-cleanup");
                    lblStatus.Text = $"Attempting to use {browser} cookies...";
                }
            }

            startInfo.ArgumentList.Add("-o");
            startInfo.ArgumentList.Add(outputTemplate);
            startInfo.ArgumentList.Add(url);
        }

        private static string BuildCommandLineForLog(string executablePath, System.Collections.ObjectModel.Collection<string> arguments)
        {
            return QuoteArgument(executablePath) + " " + string.Join(" ", arguments.Select(QuoteArgument));
        }

        private static string QuoteArgument(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            return value.Any(char.IsWhiteSpace) || value.Contains('"')
                ? $"\"{value.Replace("\"", "\\\"")}\""
                : value;
        }

        private QualitySelection GetSelectedQuality()
        {
            return cmbQuality.SelectedIndex switch
            {
                0 => new QualitySelection("bestvideo+bestaudio/best"),
                1 => new QualitySelection("bestvideo[height<=2160][height>=1440]+bestaudio/best[height<=2160][height>=1440]/bestvideo[height>=1440]+bestaudio"),
                2 => new QualitySelection("bestvideo[height<=1440][height>=1080]+bestaudio/best[height<=1440][height>=1080]/bestvideo[height>=1080]+bestaudio"),
                3 => new QualitySelection("bestvideo[height<=1080][height>=720]+bestaudio/best[height<=1080][height>=720]/bestvideo[height>=720]+bestaudio"),
                4 => new QualitySelection("bestvideo[height<=720][height>=480]+bestaudio/best[height<=720][height>=480]/bestvideo[height>=480]+bestaudio"),
                5 => new QualitySelection("bestvideo[height<=480][height>=360]+bestaudio/best[height<=480][height>=360]/bestvideo[height>=360]+bestaudio"),
                6 => new QualitySelection("bestaudio/best", IsAudioOnly: true, AudioFormat: "best", AudioQuality: "0"),
                7 => new QualitySelection("bestaudio/best", IsAudioOnly: true, AudioFormat: "mp3", AudioQuality: "320K"),
                _ => new QualitySelection("bestvideo+bestaudio/best")
            };
        }

        private sealed record QualitySelection(string FormatSelector, bool IsAudioOnly = false, string AudioFormat = "best", string? AudioQuality = null);

        private sealed class AppSettings
        {
            public bool PortableMode { get; set; }
            public bool DarkTheme { get; set; }
            public string OutputFolder { get; set; } = "";
            public int QualityIndex { get; set; }
            public int TemplateIndex { get; set; }
            public bool BypassRestrictions { get; set; }
            public bool DownloadPlaylist { get; set; }
            public bool CheckForUpdatesOnStartup { get; set; } = true;
        }
    }
}
