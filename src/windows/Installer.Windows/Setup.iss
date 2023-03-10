; This script requires Inno Setup Compiler 6.0.0 or later to compile
; The Inno Setup Compiler (and IDE) can be found at http://www.jrsoftware.org/isinfo.php
; General documentation on how to use InnoSetup scripts: http://www.jrsoftware.org/ishelp/index.php

; Ensure minimum Inno Setup tooling version
#if VER < EncodeVer(6,0,0)
  #error Update your Inno Setup version (6.0.0 or newer)
#endif

#ifndef PayloadDir
  #error Payload directory path property 'PayloadDir' must be specified
#endif

#ifndef InstallTarget
  #error Installer target property 'InstallTarget' must be specifed
#endif

#if InstallTarget == "user"
  #define GcmAppId "{{aa76d31d-432c-42ee-844c-bc0bc801cef3}}"
  #define GcmLongName "Git Credential Manager (User)"
  #define GcmSetupExe "gcmuser"
  #define GcmConfigureCmdArgs ""
#elif InstallTarget == "system"
  #define GcmAppId "{{fdfae50a-1bc1-4ead-9228-1e1c275e8d12}}"
  #define GcmLongName "Git Credential Manager"
  #define GcmSetupExe "gcm"
  #define GcmConfigureCmdArgs "--system"
#else
  #error Installer target property 'InstallTarget' must be 'user' or 'system'
#endif

; Define core properties
#define GcmShortName "Git Credential Manager"
#define GcmPublisher "GitHub"
#define GcmVersionInfoDescription "Secure, cross-platform Git credential manager."
#define GcmPublisherUrl "https://www.github.com"
#define GcmCopyright "Copyright (c) GitHub, Inc. and contributors"
#define GcmUrl "https://aka.ms/gcm"
#define GcmReadme "https://github.com/GitCredentialManager/git-credential-manager/blob/main/README.md"
#define GcmRepoRoot "..\..\.."
#define GcmAssets GcmRepoRoot + "\assets"
#define GcmExe "git-credential-manager.exe"
#define GcmArch "x86"

#ifnexist PayloadDir + "\" + GcmExe
  #error Payload files are missing
#endif

; Generate the GCM version version from the CLI executable
#define VerMajor
#define VerMinor
#define VerBuild
#define VerRevision
#expr ParseVersion(PayloadDir + "\" + GcmExe, VerMajor, VerMinor, VerBuild, VerRevision)
#define GcmVersionSimple str(VerMajor) + "." + str(VerMinor) + "." + str(VerBuild)
#define GcmVersion str(GcmVersionSimple) + "." + str(VerRevision)

[Setup]
AppId={#GcmAppId}
AppName={#GcmLongName}
AppVersion={#GcmVersion}
AppVerName={#GcmLongName} {#GcmVersion}
AppPublisher={#GcmPublisher}
AppPublisherURL={#GcmPublisherUrl}
AppSupportURL={#GcmUrl}
AppUpdatesURL={#GcmUrl}
AppContact={#GcmUrl}
AppCopyright={#GcmCopyright}
AppReadmeFile={#GcmReadme}
VersionInfoVersion={#GcmVersion}
LicenseFile={#GcmRepoRoot}\LICENSE
OutputBaseFilename={#GcmSetupExe}-win-{#GcmArch}-{#GcmVersionSimple}
DefaultDirName={autopf}\{#GcmShortName}
Compression=lzma2
SolidCompression=yes
MinVersion=6.1.7600
DisableDirPage=yes
UninstallDisplayIcon={app}\{#GcmExe}
SetupIconFile={#GcmAssets}\gcmicon.ico
WizardImageFile={#GcmAssets}\gcmicon128.bmp
WizardSmallImageFile={#GcmAssets}\gcmicon64.bmp
WizardStyle=modern
WizardImageStretch=no
WindowResizable=no
ChangesEnvironment=yes
#if InstallTarget == "user"
  PrivilegesRequired=lowest
#endif

[Languages]
Name: english; MessagesFile: "compiler:Default.isl";

[Types]
Name: full; Description: "Full installation"; Flags: iscustom;

[Components]
; No individual components

[Run]
Filename: "{app}\{#GcmExe}"; Parameters: "configure {#GcmConfigureCmdArgs}"; Flags: runhidden

[UninstallRun]
Filename: "{app}\{#GcmExe}"; Parameters: "unconfigure {#GcmConfigureCmdArgs}"; Flags: runhidden

[Files]
Source: "{#PayloadDir}\Atlassian.Bitbucket.dll";                        DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Atlassian.Bitbucket.UI.exe";                     DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Atlassian.Bitbucket.UI.exe.config";              DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Atlassian.Bitbucket.UI.Shared.dll";              DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager.exe";                     DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager.exe.config";              DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager-core.exe";                DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager-core.exe.config";         DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager-ui.exe";                  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\git-credential-manager-ui.exe.config";           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitHub.dll";                                     DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitHub.UI.exe";                                  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitHub.UI.exe.config";                           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitHub.UI.Shared.dll";                           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitLab.dll";                                     DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitLab.UI.exe";                                  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitLab.UI.exe.config";                           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\GitLab.UI.Shared.dll";                           DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.AzureRepos.dll";                       DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\gcmcore.dll";                                    DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\gcmcoreui.dll";                                  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\gcmcoreuiwpf.dll";                               DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Identity.Client.Desktop.dll";          DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Identity.Client.dll";                  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Identity.Client.Extensions.Msal.dll";  DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Web.WebView2.Core.dll";                DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Web.WebView2.WinForms.dll";            DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Microsoft.Web.WebView2.Wpf.dll";                 DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\Newtonsoft.Json.dll";                            DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\NOTICE";                                         DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\System.Buffers.dll";                             DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\System.CommandLine.dll";                         DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\System.Memory.dll";                              DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\System.Numerics.Vectors.dll";                    DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\System.Runtime.CompilerServices.Unsafe.dll";     DestDir: "{app}"; Flags: ignoreversion
Source: "{#PayloadDir}\WebView2Loader.dll";                             DestDir: "{app}"; Flags: ignoreversion

[Code]
// Don't allow installing conflicting architectures
function InitializeSetup(): Boolean;
begin
  Result := True;

  #if InstallTarget == "user"
    if not WizardSilent() and IsAdmin() then begin
      if MsgBox('This User Installer is not meant to be run as an Administrator. If you would like to install Git Credential Manager for all users in this system, download the System Installer instead from https://aka.ms/gcm/latest. Are you sure you want to continue?', mbError, MB_OKCANCEL) = IDCANCEL then begin
        Result := False;
      end;
    end;
  #endif
end;
