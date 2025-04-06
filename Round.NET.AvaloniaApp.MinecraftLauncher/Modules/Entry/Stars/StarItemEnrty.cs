using System;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;
using SixLabors.ImageSharp.Processing;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;

public class StarItemEnrty
{
    public string SUID { get; } = Guid.NewGuid().ToString();
    public string SourceData { get; set; } = string.Empty; 
    public StarItemTypeEnum Type { get; set; }
}