namespace RMCLConfigServer.Modules.Entry;

public class LauncherConfigEntry
{
    public int StyleModle { get; set; } = 0;
    public string StyleURL { get; set; } = "";
    public string LauncherName { get; set; } = "RMCL 3.0";
    public int MaxMemory { get; set; } = 4096;
    public int MaxDownloadThreadCount { get; set; } = 512;
    public bool GameMonitoring { get; set; } = true;
}