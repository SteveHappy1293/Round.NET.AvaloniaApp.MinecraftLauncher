using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal class MinecraftClient
{
    public static string LastMessage { get; private set; } = "";
    
    // 存储服务器列表（Key: IP+Port，Value: 服务器信息 + 最后更新时间）
    private static readonly ConcurrentDictionary<string, (MinecraftServerInfo Info, DateTime LastUpdate)> 
        ServerList = new ConcurrentDictionary<string, (MinecraftServerInfo, DateTime)>();
    
    // 清理过期服务器的定时器
    private static Timer? _cleanupTimer;

    public static void StartListeningMinecraftBroadcast()
    {
        var thread = new Thread(ListenForMinecraftBroadcast)
        {
            IsBackground = true,
            Name = "Minecraft LAN Listener"
        };
        thread.Start();

        // 启动定时清理任务（每 500ms 检查一次）
        _cleanupTimer = new Timer(CleanupExpiredServers, null, 1200, 500);
    }

    private static void ListenForMinecraftBroadcast()
    {
        const string multicastGroup = "224.0.2.60";
        const int multicastPort = 4445;

        try
        {
            using var udpClient = new UdpClient(multicastPort);
            var multicastAddress = IPAddress.Parse(multicastGroup);
            udpClient.JoinMulticastGroup(multicastAddress);

            Console.WriteLine("[Minecraft LAN] Listening for broadcasts...");

            var remoteEp = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    byte[] receivedBytes = udpClient.Receive(ref remoteEp);
                    string receivedData = Encoding.UTF8.GetString(receivedBytes);

                    if (LastMessage == receivedData)
                        continue;

                    LastMessage = receivedData;
                    Console.WriteLine($"[Minecraft LAN] Received: {receivedData}");

                    var serverInfo = ParseMinecraftBroadcast(receivedData);
                    
                    // 更新服务器列表（Key 可以是 IP+Port 或 MOTD，这里用 IP+Port 作为唯一标识）
                    string serverKey = $"{remoteEp.Address}:{serverInfo.Port}";
                    
                    ServerList.AddOrUpdate(
                        serverKey,
                        (serverInfo, DateTime.Now), // 如果不存在，添加新条目
                        (key, oldValue) => (serverInfo, DateTime.Now) // 如果存在，更新信息和时间
                    );

                    // 在 UI 线程更新显示
                    Dispatcher.UIThread.Invoke(() => UpdateServerListUI());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Minecraft LAN] Error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Minecraft LAN] Fatal Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 解析 Minecraft LAN 广播数据（格式通常为 JSON）
    /// </summary>
    private static MinecraftServerInfo ParseMinecraftBroadcast(string broadcastData)
    {
        try
        {
            // Minecraft LAN 广播通常是 JSON 格式，例如：
            // {"motd":"A Minecraft Server","port":25565,"version":"1.20.1"}
            var json = JObject.Parse(broadcastData);

            return new MinecraftServerInfo
            {
                Motd = json["motd"]?.ToString() ?? "Unknown Server",
                Port = json["port"]?.ToObject<int>() ?? 25565,
                Version = json["version"]?.ToString() ?? "Unknown",
                PlayerCount = json["players"]?["online"]?.ToObject<int>() ?? 0,
                MaxPlayers = json["players"]?["max"]?.ToObject<int>() ?? 0,
                Icon = json["favicon"]?.ToString() // Base64 编码的服务器图标
            };
        }
        catch (JsonException)
        {
            // 如果不是 JSON，可能是旧格式（如 [MOTD]...[/MOTD]）
            return ParseLegacyMOTD(broadcastData);
        }
    }
    
    /// <summary>
    /// 解析旧版 MOTD 格式（如 [MOTD]...[/MOTD]）
    /// </summary>
    private static MinecraftServerInfo ParseLegacyMOTD(string motdData)
    {
        var serverInfo = new MinecraftServerInfo { Motd = "Unknown Server" };

        try
        {
            // 旧格式示例：[MOTD]My Server[/MOTD][AD]25565[/AD]
            var motdMatch = Regex.Match(motdData, @"\[MOTD\](.*?)\[/MOTD\]");
            if (motdMatch.Success)
                serverInfo.Motd = motdMatch.Groups[1].Value;

            var portMatch = Regex.Match(motdData, @"\[AD\](\d+)\[/AD\]");
            if (portMatch.Success && int.TryParse(portMatch.Groups[1].Value, out int port))
                serverInfo.Port = port;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Minecraft LAN] Failed to parse legacy MOTD: {ex.Message}");
        }

        return serverInfo;
    }

    
    /// <summary>
    /// 清理超过 1200ms 未更新的服务器
    /// </summary>
    private static void CleanupExpiredServers(object? state)
    {
        var now = DateTime.Now;
        var expiredServers = new List<string>();

        // 找出所有超过 1000ms 未更新的服务器
        foreach (var server in ServerList)
        {
            if ((now - server.Value.LastUpdate).TotalMilliseconds > 1200)
            {
                expiredServers.Add(server.Key);
            }
        }

        // 移除过期服务器
        foreach (var serverKey in expiredServers)
        {
            ServerList.TryRemove(serverKey, out _);
        }

        // 如果有服务器被移除，更新 UI
        if (expiredServers.Count > 0)
        {
            Dispatcher.UIThread.Invoke(() => UpdateServerListUI());
        }
    }

    /// <summary>
    /// 更新 UI 显示当前服务器列表
    /// </summary>
    private static void UpdateServerListUI()
    {
        // 这里可以绑定到 Avalonia 的 ItemsControl（如 ListBox）
        // 例如：
        // ServerListBox.Items = ServerList.Values.Select(x => x.Info).ToList();
        
        // 示例：打印当前服务器列表
        Console.WriteLine("当前服务器列表:");
        foreach (var server in ServerList.Values)
        {
            Console.WriteLine($"{server.Info.Motd} (v{server.Info.Version}), 玩家: {server.Info.PlayerCount}/{server.Info.MaxPlayers}");
        }
    }

    // 其他方法（ParseMinecraftBroadcast、ParseLegacyMOTD）保持不变...
}

public class MinecraftServerInfo
{
    public string Motd { get; set; } = "Unknown Server";
    public int Port { get; set; } = 25565;
    public string Version { get; set; } = "Unknown";
    public int PlayerCount { get; set; } = 0;
    public int MaxPlayers { get; set; } = 0;
    public string? Icon { get; set; }
}