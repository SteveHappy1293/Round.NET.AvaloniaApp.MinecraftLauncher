namespace Round.NET.VersionServerMange.Library.Entry;

public class ServerEntry
{
    public string IP { get; set; } = String.Empty;
    public string Name { get; set; } = "Minecraft Server";
    public string Icon { get; set; } = "";
    public string SUID { get; set; } = Guid.NewGuid().ToString();
}