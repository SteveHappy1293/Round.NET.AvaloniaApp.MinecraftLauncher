using System.Text.Json.Serialization;

namespace RMCL.OnlineCore.Entry.OpenP2P;

public class OpenP2PConfigEntry
{
    public class NetworkEntry
    {
        public ulong Token { get; set; } = 0;
        public string Node { get; set; } = String.Empty;
        public string User { get; set; } = String.Empty;
        public string ServerHost { get; set; } = "api.openp2p.cn";
        public int ShareBandwidth { get; set; } = 10;
        public int ServerPort { get; set; } = 27183;
        public int UDPPort1 { get; set; } = 27182;
        public int UDPPort2 { get; set; } = 27183;
        public int TCPPort { get; set; } = 50448;
    }
    
    public class App
    {
        public string AppName { get; set; } = String.Empty;
        public string Protocol { get; set; } = "tcp";
        public string UnderlayProtocol { get; set; } = String.Empty;
        public string Whitelist { get; set; } = String.Empty;
        public string PeerNode { get; set; } = String.Empty;
        public string DstHost { get; set; } = String.Empty;
        public string PeerUser { get; set; } = String.Empty;
        public string RelayNode { get; set; } = String.Empty;
        public int PunchPriority { get; set; } = 0;
        public int SrcPort { get; set; } = 25565; // 对方端口
        public int DstPort { get; set; } = 25565; // 本地映射端口
        public int ForceRelay { get; set; } = 0;
        public int Enabled { get; set; } = 1;
    }

    [JsonPropertyName("network")]
    public NetworkEntry Network { get; set; } = new ();
    [JsonPropertyName("apps")]
    public List<App> Apps { get; set; } = new ();
    
    public int LogLevel { get; set; } = 2;
}