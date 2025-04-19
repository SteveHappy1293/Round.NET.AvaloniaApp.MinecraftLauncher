namespace OnlinePlug.Modules;

public class OnlineConfigEntry
{
    public NetworkConfig Network { get; set; }
    public List<AppConfig> Apps { get; set; }
    public int LogLevel { get; set; }
}

public class NetworkConfig
{
    public ulong Token { get; set; }
    public string Node { get; set; }
    public string User { get; set; }
    public int ShareBandwidth { get; set; }
    public string ServerHost { get; set; }
    public int ServerPort { get; set; }
    public int UDPPort1 { get; set; }
    public int UDPPort2 { get; set; }
    public int TCPPort { get; set; }
}

public class AppConfig
{
    public string AppName { get; set; }
    public string Protocol { get; set; }
    public string UnderlayProtocol { get; set; }
    public int PunchPriority { get; set; }
    public string Whitelist { get; set; }
    public int SrcPort { get; set; }
    public string PeerNode { get; set; }
    public int DstPort { get; set; }
    public string DstHost { get; set; }
    public string PeerUser { get; set; }
    public string RelayNode { get; set; }
    public int ForceRelay { get; set; }
    public int Enabled { get; set; }
}