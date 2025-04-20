namespace OnlinePlug.Modules;

public class OnlineConfigEntry
{
    public NetworkConfig network { get; set; } = new();
    public List<AppConfig> apps { get; set; } = new();
    public int LogLevel { get; set; }
}

public class NetworkConfig
{
    public ulong Token { get; set; } = 17190022896174664900;
    public string Node { get; set; } = Guid.NewGuid().ToString();
    public string User { get; set; } = "MinecraftYJQ_";
    public int ShareBandwidth { get; set; } = 10;
    public string ServerHost { get; set; } = "api.openp2p.cn";
    public int ServerPort { get; set; } = 27183;
    public int UDPPort1 { get; set; } = 27182;
    public int UDPPort2 { get; set; } = 27183;
    public int TCPPort { get; set; } = 50448;
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