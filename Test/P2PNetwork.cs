using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class P2PNetwork : IDisposable
{
    private readonly object _reqGatewayLock = new object();
    private TcpClient _tcpClient;
    private NetworkStream _networkStream;
    private const int ClientAPITimeout = 5000; // 5 seconds timeout
    private const string LeastSupportVersion = "1.0.0"; // Example minimum version

    public P2PNetwork()
    {
        _tcpClient = new TcpClient();
        _tcpClient.SendTimeout = ClientAPITimeout;
        _tcpClient.ReceiveTimeout = ClientAPITimeout;
    }

    public async Task<ErrorType> RequestPeerInfoAsync(AppConfig config)
    {
        try
        {
            lock (_reqGatewayLock)
            {
                if (!_tcpClient.Connected)
                {
                    var connectTask = _tcpClient.ConnectAsync(config.Network.ServerHost, config.Network.ServerPort);
                    if (!connectTask.Wait(ClientAPITimeout))
                    {
                        Logger.Log(LogLevel.Error, "Connection timeout");
                        return ErrorType.Network;
                    }
                    _networkStream = _tcpClient.GetStream();
                }
            }

            // Create request
            var request = new QueryPeerInfoReq
            {
                peerToken = config.Network.Token,
                PeerNode = config.Network.Node
            };

            // Serialize and send request
            var jsonRequest = JsonConvert.SerializeObject(request);
            var requestBytes = Encoding.UTF8.GetBytes(jsonRequest);

            // Send message length (4 bytes, big-endian)
            var lengthBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(requestBytes.Length));
            await _networkStream.WriteAsync(lengthBytes, 0, 4);

            // Send message
            await _networkStream.WriteAsync(requestBytes, 0, requestBytes.Length);

            // Read response with timeout
            using (var cts = new CancellationTokenSource(ClientAPITimeout))
            {
                // Read message length (4 bytes)
                var lengthBuffer = new byte[4];
                var lengthRead = await ReadFullAsync(_networkStream, lengthBuffer, 0, 4, cts.Token);
                if (lengthRead != 4)
                {
                    Logger.Log(LogLevel.Error, "Failed to read message length");
                    return ErrorType.Network;
                }

                var messageLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBuffer, 0));
                if (messageLength <= 0 || messageLength > 1024 * 1024) // 1MB max
                {
                    Logger.Log(LogLevel.Error, "Invalid message length");
                    return ErrorType.MsgFormat;
                }

                // Read message
                var buffer = new byte[messageLength];
                var messageRead = await ReadFullAsync(_networkStream, buffer, 0, messageLength, cts.Token);
                if (messageRead != messageLength)
                {
                    Logger.Log(LogLevel.Error, "Incomplete message received");
                    return ErrorType.Network;
                }

                var jsonResponse = Encoding.UTF8.GetString(buffer, 0, messageLength);
                var rsp = JsonConvert.DeserializeObject<QueryPeerInfoRsp>(jsonResponse);

                if (rsp == null)
                {
                    Logger.Log(LogLevel.Error, "Failed to deserialize response");
                    return ErrorType.MsgFormat;
                }

                if (rsp.Online == 0)
                {
                    return ErrorType.PeerOffline;
                }

                if (CompareVersion(rsp.Version, LeastSupportVersion) < 0)
                {
                    return ErrorType.VersionNotCompatible;
                }

                // Update config with peer info
                config.PeerVersion = rsp.Version;
                config.PeerLanIP = rsp.LanIP;
                config.HasIPv4 = rsp.HasIPv4 == 1;
                config.PeerIP = rsp.IPv4;
                config.PeerIPv6 = rsp.IPv6;
                config.HasUPNPorNATPMP = rsp.HasUPNPorNATPMP == 1;
                config.PeerNatType = rsp.NatType.ToString();

                Logger.Log(LogLevel.Info, $"Peer IP: {rsp.IPv4}, IPv6: {rsp.IPv6}");
                return ErrorType.None;
            }
        }
        catch (OperationCanceledException)
        {
            Logger.Log(LogLevel.Error, "Request timeout");
            return ErrorType.Network;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Request error: {ex.Message}");
            return ErrorType.Network;
        }
    }

    private async Task<int> ReadFullAsync(NetworkStream stream, byte[] buffer, int offset, int count, CancellationToken ct)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            var read = await stream.ReadAsync(buffer, offset + totalRead, count - totalRead, ct);
            if (read == 0) break; // Connection closed
            totalRead += read;
        }
        return totalRead;
    }

    private int CompareVersion(string version1, string version2)
    {
        try
        {
            var v1 = Version.Parse(version1);
            var v2 = Version.Parse(version2);
            return v1.CompareTo(v2);
        }
        catch
        {
            return -1; // Treat parse errors as incompatible
        }
    }

    public void Dispose()
    {
        _networkStream?.Dispose();
        _tcpClient?.Dispose();
    }
}

public class AppConfig
{
    public NetworkConfig Network { get; set; }
    public List<AppConfig> Apps { get; set; }
    public int LogLevel { get; set; }
    
    // Peer info fields
    public string PeerVersion { get; set; }
    public string PeerLanIP { get; set; }
    public bool HasIPv4 { get; set; }
    public string PeerIP { get; set; }
    public string PeerIPv6 { get; set; }
    public bool HasUPNPorNATPMP { get; set; }
    public string PeerNatType { get; set; }
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

public class QueryPeerInfoReq
{
    public ulong peerToken { get; set; }
    public string PeerNode { get; set; }
}

public class QueryPeerInfoRsp
{
    public int Online { get; set; }
    public string Version { get; set; }
    public string LanIP { get; set; }
    public int HasIPv4 { get; set; }
    public string IPv4 { get; set; }
    public string IPv6 { get; set; }
    public int HasUPNPorNATPMP { get; set; }
    public int NatType { get; set; }
}

public enum ErrorType
{
    None,
    Network,
    MsgFormat,
    PeerOffline,
    VersionNotCompatible
}

public enum LogLevel
{
    Error,
    Info,
    Debug,
    Warning
}

public static class Logger
{
    public static void Log(LogLevel level, string message)
    {
        var color = Console.ForegroundColor;
        try
        {
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine($"[{level}][{DateTime.Now:HH:mm:ss.fff}] {message}");
        }
        finally
        {
            Console.ForegroundColor = color;
        }
    }
}