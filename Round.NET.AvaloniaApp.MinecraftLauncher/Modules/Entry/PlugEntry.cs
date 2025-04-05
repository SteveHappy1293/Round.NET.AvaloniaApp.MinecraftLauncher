using Avalonia.Media;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;

public class PlugEntry
{
    public IImageBrushSource? Icon { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Writer { get; set; }
    public string PlugPackFile { get; set; }
}