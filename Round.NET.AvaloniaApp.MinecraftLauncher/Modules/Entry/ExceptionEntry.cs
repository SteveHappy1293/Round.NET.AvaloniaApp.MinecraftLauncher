using System;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;

public class ExceptionEntry
{
    public ConfigRoot MainConfig { get; set; } = Config.Config.MainConfig;
    public DateTime RecordTime { get; set; } = DateTime.Now;
    public string Exception { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string SystemVersion { get; set; } = string.Empty;
    public string SystemLanguage { get; set; } = string.Empty;
    public string SystemTimeZone { get; set; } = string.Empty;
}