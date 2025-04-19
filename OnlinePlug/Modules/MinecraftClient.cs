using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class UdpClientExtensions
{
    // 扩展方法：为UDP接收添加取消支持
    public static async Task<UdpReceiveResult> WithCancellation(
        this Task<UdpReceiveResult> task,
        CancellationToken cancellationToken)
    {
        try
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetCanceled(), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }
        }catch{ }
        return await task.ConfigureAwait(false);
    }
}

internal class MinecraftClient : IDisposable
{
    private readonly List<MinecraftServerInfo> _serverList = new();
    private CancellationTokenSource? _cancellationTokenSource;
    private UdpClient? _udpClient;
    private const int ScanTimeoutMs = 500;
    private bool _disposed;

    public async Task<List<MinecraftServerInfo>> PerformSingleScanAsync()
    {
        try
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MinecraftClient));

            _serverList.Clear();

            // 取消之前的操作并清理
            await CleanupResourcesAsync();
        }catch{ }

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _udpClient = new UdpClient();
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var listenTask = ListenForMinecraftBroadcastAsync(_cancellationTokenSource.Token);

            // 设置超时
            var timeoutTask = Task.Delay(ScanTimeoutMs, _cancellationTokenSource.Token);
            await Task.WhenAny(listenTask, timeoutTask);

            // 取消操作
            _cancellationTokenSource.Cancel();

            // 等待任务完成（不抛出取消异常）
            await listenTask.ContinueWith(_ => { }, TaskContinuationOptions.ExecuteSynchronously);
        }
        finally
        {
            await CleanupResourcesAsync();
        }

        return new List<MinecraftServerInfo>(_serverList);
    }

    private async Task ListenForMinecraftBroadcastAsync(CancellationToken cancellationToken)
    {
        const string multicastGroup = "224.0.2.60";
        const int multicastPort = 4445;

        try
        {
            if (_udpClient == null) return;

            var localEndPoint = new IPEndPoint(IPAddress.Any, multicastPort);
            _udpClient.Client.Bind(localEndPoint);

            var multicastAddress = IPAddress.Parse(multicastGroup);
            _udpClient.JoinMulticastGroup(multicastAddress);

            Console.WriteLine("[Minecraft LAN] Scanning for servers...");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync()
                        .WithCancellation(cancellationToken);

                    ProcessReceivedData(result.Buffer, result.RemoteEndPoint);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Minecraft LAN] Error receiving data: {ex.Message}");
                }
            }
        }
        finally
        {
            try
            {
                if (_udpClient != null)
                {
                    _udpClient.DropMulticastGroup(IPAddress.Parse(multicastGroup));
                    _udpClient.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Minecraft LAN] Error cleaning up: {ex.Message}");
            }
        }
    }

    private void ProcessReceivedData(byte[] data, IPEndPoint remoteEndPoint)
    {
        try
        {
            string receivedData = Encoding.UTF8.GetString(data);
            Console.WriteLine($"[Minecraft LAN] Received from {remoteEndPoint}: {receivedData}");

            var serverInfo = ParseMinecraftBroadcast(receivedData);
            lock (_serverList)
            {
                _serverList.Add(serverInfo);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Minecraft LAN] Error processing data: {ex.Message}");
        }
    }

    private static MinecraftServerInfo ParseMinecraftBroadcast(string broadcastData)
    {
        try
        {
            var json = JObject.Parse(broadcastData);
            return new MinecraftServerInfo
            {
                Motd = json["motd"]?.ToString() ?? "Unknown Server",
                Port = json["port"]?.ToObject<int>() ?? 25565,
                Version = json["version"]?.ToString() ?? "Unknown",
                PlayerCount = json["players"]?["online"]?.ToObject<int>() ?? 0,
                MaxPlayers = json["players"]?["max"]?.ToObject<int>() ?? 0,
                Icon = json["favicon"]?.ToString(),
                IPAddress = json["ip"]?.ToString()
            };
        }
        catch (JsonException)
        {
            return ParseLegacyMOTD(broadcastData);
        }
    }

    private static MinecraftServerInfo ParseLegacyMOTD(string motdData)
    {
        var serverInfo = new MinecraftServerInfo { Motd = "Unknown Server" };

        try
        {
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

    private async Task CleanupResourcesAsync()
    {
        try
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            if (_udpClient != null)
            {
                try
                {
                    // 给一点时间让操作完成
                    await Task.Delay(100);
                    _udpClient.Close();
                    _udpClient.Dispose();
                }
                finally
                {
                    _udpClient = null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Minecraft LAN] Error during cleanup: {ex.Message}");
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        CleanupResourcesAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }
}

public class MinecraftServerInfo
{
    public string Motd { get; set; } = "Unknown Server";
    public int Port { get; set; } = 25565;
    public string Version { get; set; } = "Unknown";
    public int PlayerCount { get; set; } = 0;
    public int MaxPlayers { get; set; } = 0;
    public string? Icon { get; set; }
    public string? IPAddress { get; set; }
}