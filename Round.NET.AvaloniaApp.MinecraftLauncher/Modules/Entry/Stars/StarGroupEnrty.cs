using System;
using System.Collections.Generic;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;

public class StarGroupEnrty
{
    public string GroupName { get; set; } = string.Empty;
    public string ImageBase64String { get; set; } = string.Empty;
    public string GUID { get; } = Guid.NewGuid().ToString();
    public List<StarItemEnrty> Stars { get; set; } = new();
}