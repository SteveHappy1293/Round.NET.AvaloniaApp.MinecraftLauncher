using System;
using System.Text.Json.Serialization;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;

public class OrganizationEnrty
{
    public class ConfigResponse
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public ConfigData Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ConfigData
    {
        public string ServerName { get; set; }
        public AppConfig Config { get; set; }
    }

    public class AppConfig
    {
        public int StyleModle { get; set; }
        public string StyleURL { get; set; }
        public string LauncherName { get; set; }
        public int MaxMemory { get; set; }
        public int MaxDownloadThreadCount { get; set; }
        public bool GameMonitoring { get; set; }
    }
}