#define MyAppName "YouTube Downloader"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Nipstacles"
#define MyAppExeName "YouTubeDownloader.exe"
#define SourceDir "publish\win-x64"

[Setup]
AppId={{B5DDB2E5-33A8-4C67-A25D-CA8A9E75EAC3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=installer
OutputBaseFilename=YouTubeDownloaderSetup-{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
SetupIconFile=yt-dl_logo.ico
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Shortcuts:"; Flags: unchecked
Name: "portablemode"; Description: "Run in portable mode (store settings beside the app)"; GroupDescription: "Install mode:"; Flags: unchecked exclusive
Name: "standardmode"; Description: "Standard install (store settings in AppData)"; GroupDescription: "Install mode:"; Flags: checkedonce exclusive

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
var
  SettingsPath: string;
  SettingsJson: string;
begin
  if CurStep = ssPostInstall then
  begin
	SettingsPath := ExpandConstant('{app}\settings.json');

	if WizardIsTaskSelected('portablemode') then
	begin
	  SettingsJson := '{' + #13#10 +
		'  "PortableMode": true,' + #13#10 +
		'  "DarkTheme": false,' + #13#10 +
		'  "OutputFolder": "",' + #13#10 +
		'  "QualityIndex": 0,' + #13#10 +
		'  "TemplateIndex": 0,' + #13#10 +
		'  "BypassRestrictions": false,' + #13#10 +
		'  "DownloadPlaylist": false,' + #13#10 +
		'  "CheckForUpdatesOnStartup": true' + #13#10 +
		'}';
	  SaveStringToFile(SettingsPath, SettingsJson, False);
	end
	else
	begin
	  if FileExists(SettingsPath) then
		DeleteFile(SettingsPath);
	end;
  end;
end;
