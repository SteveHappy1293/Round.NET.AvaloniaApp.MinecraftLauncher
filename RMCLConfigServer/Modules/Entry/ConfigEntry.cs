namespace RMCLConfigServer.Modules.Entry;

public class ConfigEntry
{
    public int ServerPort { get; set; } = 4537;
    public string ServerName { get; set; } = "RMCL 配置服务器";
    public bool IsHotReload { get; set; } = true;
    public LauncherConfigEntry LauncherConfig { get; set; } = new();
}