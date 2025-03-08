using Avalonia.Media;

namespace Round.NET.VersionServerMange.Library.Entry;

public class ServerInfoEntry
{
    public string ServerName { get; set; }
    public string ServerIP { get; set; }
    public IImage Icon { get; set; }
    public string IconBase64 { get; set; }
    public string Text { get; set; }
    public int MaxPlayers { get; set; }
    public int OnlinePlayers { get; set; }
}