using System.Collections.Generic;

namespace RMCL.Config;

public class ConfigRoot
{
    public List<GameFolderConfig> GameFolders { get; set; } = new();
    public bool FirstLauncher { get; set; } = true;
    public bool IsUseOrganizationConfig { get; set; } = false;
    public string OrganizationUrl { get; set; } = string.Empty;
    public int SelectedGameFolder { get; set; } = 0;
    public int SelectedJava { get; set; } = 0;
    public int SelectedUser { get; set; } = 0;
    public int BackModlue { get; set; } = 0;
    public int UpdateSourse { get; set; } = 0;
    public string BackImage { get; set; } = string.Empty;
    public double BackOpacity { get; set; } = 50;
    public string StyleFile { get; set; } = string.Empty;
    public bool ShowErrorWindow { get; set; } = true;
    public bool IsAutoUpdate { get; set; } = true;
    public bool IsDebug { get; set; } = false;
    public int WindowWidth { get; set; } = 850;
    public int WindowHeight { get; set; } = 450;
    public bool IsUsePlug { get; set; } = true;
    public int MessageLiveTimeMs { get; set; } = 5000;
    public bool SetTheLanguageOnStartup { get; set; } = true;
    public bool SetTheGammaOnStartup { get; set; } = true;
    public int JavaUseMemory { get; set; } = 4096;
    public bool IsLaunchJavaMemory { get; set; } = true;
    public int GameLogOpenModlue { get; set; } = 0;
    public int DownloadThreads { get; set; } = 256;
    public int WindowX { get; set; } = 0;
    public int WindowY { get; set; } = 0;
    public string RSAccount { get; set; } = string.Empty;
    public ThemeType Theme { get; set; } = ThemeType.System;
}
public class GameFolderConfig
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int SelectedGameIndex { get; set; } = 0;
}

public enum ThemeType
{
    System,
    Dark,
    Light
}