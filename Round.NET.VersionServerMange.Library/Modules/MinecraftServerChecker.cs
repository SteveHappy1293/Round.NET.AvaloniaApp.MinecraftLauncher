using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Round.NET.VersionServerMange.Library.Entry;

namespace Round.NET.VersionServerMange.Library.Modules;

public class MinecraftServerChecker
{
    // 属性：服务器地址和端口
    public string ServerAddress { get; set; }
    public int ServerPort { get; set; }
    // 构造函数
    public MinecraftServerChecker(string serverAddress, int serverPort)
    {
        ServerAddress = serverAddress;
        ServerPort = serverPort;
    }
    // 获取服务器信息
    public async Task<ServerInfoEntry> GetServerInfo()
    {
        try
        {
            // 获取服务器信息
            var serverInfo = await GetServerInfoInternalAsync();

            var text = "";
            if (serverInfo.description is string)
            {
                text = serverInfo.description.ToString();
            }
            else
            {
                text = serverInfo.description.text;
            }

            return new()
            {
                ServerIP = $"{ServerAddress}:{ServerPort}",
                Icon = GetFavicon(serverInfo.favicon),
                IconBase64 = serverInfo.favicon,
                Text = text,
                MaxPlayers = serverInfo.players.max,
                OnlinePlayers = serverInfo.players.online
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法获取服务器信息: {ex.Message}");
        }
        return null;
    }

    // 内部方法：获取服务器信息
    private async Task<MinecraftServerInfo> GetServerInfoInternalAsync()
    {
        using (var client = new TcpClient())
        {
            // 设置连接超时
            var connectTask = client.ConnectAsync(ServerAddress, ServerPort);
            var timeoutTask = Task.Delay(5000); // 5秒超时
            var completedTask = await Task.WhenAny(connectTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                throw new Exception("连接服务器超时。");
            }

            using (var stream = client.GetStream())
            {
                // 发送握手包
                SendHandshake(stream, ServerAddress, ServerPort);

                // 发送状态请求包
                SendStatusRequest(stream);

                // 读取服务器响应
                var response = await ReadResponseAsync(stream);

                // 修复 JSON 数据中的非法字符
                response = FixJson(response);

                // 保存 JSON 数据到文件（用于调试）
                File.WriteAllText("server_info.json", response);

                // 解析 JSON 响应
                return ParseServerInfo(response);
            }
        }
    }

    // 解析服务器信息
    private MinecraftServerInfo ParseServerInfo(string json)
    {
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;

        var serverInfo = new MinecraftServerInfo
        {
            version = JsonSerializer.Deserialize<VersionInfo>(root.GetProperty("version").GetRawText()),
            players = JsonSerializer.Deserialize<PlayersInfo>(root.GetProperty("players").GetRawText()),
            favicon = root.TryGetProperty("favicon", out var favicon) ? favicon.GetString() : null
        };

        // 动态处理 description 字段
        if (root.TryGetProperty("description", out var descriptionElement))
        {
            if (descriptionElement.ValueKind == JsonValueKind.String)
            {
                // 如果 description 是字符串类型
                serverInfo.description = new description { text = descriptionElement.GetString() };
            }
            else if (descriptionElement.ValueKind == JsonValueKind.Object)
            {
                // 如果 description 是对象类型
                serverInfo.description = JsonSerializer.Deserialize<description>(descriptionElement.GetRawText());
            }
        }

        return serverInfo;
    }

    // 修复 JSON 数据中的非法字符
    private string FixJson(string json)
    {
        // 移除控制字符（如 \0, \b, \f, \n, \r, \t 等）
        json = Regex.Replace(json, @"[\u0000-\u001F]", string.Empty);

        // 修复 description 字段中的 Minecraft 格式代码
        var descriptionStart = json.IndexOf("\"description\":\"") + "\"description\":\"".Length;
        var descriptionEnd = json.IndexOf("\"", descriptionStart);

        if (descriptionStart >= 0 && descriptionEnd >= 0)
        {
            var descriptionValue = json.Substring(descriptionStart, descriptionEnd - descriptionStart);
            var fixedDescriptionValue = CleanMinecraftFormatting(descriptionValue); // 清理 Minecraft 格式代码
            json = json.Substring(0, descriptionStart) + fixedDescriptionValue + json.Substring(descriptionEnd);
        }

        return json;
    }

    // 清理 Minecraft 格式代码
    private string CleanMinecraftFormatting(string input)
    {
        // 移除 Minecraft 格式代码（例如 §a, §c 等）
        // return Regex.Replace(input, @"§[0-9a-fk-or]", string.Empty);
        return input;
    }

    // 发送握手包
    private void SendHandshake(NetworkStream stream, string address, int port)
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write((byte)0x00); // 数据包 ID（握手）
            WriteVarInt(writer, 0); // 协议版本（47 是 1.8.9 的协议版本）
            WriteString(writer, address); // 服务器地址
            writer.Write((ushort)port); // 服务器端口
            WriteVarInt(writer, 1); // 下一步状态（1 表示请求状态）

            // 发送数据包
            SendPacket(stream, ms.ToArray());
        }
    }

    // 发送状态请求包
    private void SendStatusRequest(NetworkStream stream)
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write((byte)0x00); // 数据包 ID（状态请求）

            // 发送数据包
            SendPacket(stream, ms.ToArray());
        }
    }

    // 读取服务器响应
    private async Task<string> ReadResponseAsync(NetworkStream stream)
    {
        var length = ReadVarInt(stream); // 读取数据包长度
        var packetId = ReadVarInt(stream); // 读取数据包 ID
        var jsonLength = ReadVarInt(stream); // 读取 JSON 数据长度

        var buffer = new byte[jsonLength];
        int bytesRead = 0;
        while (bytesRead < jsonLength)
        {
            bytesRead += await stream.ReadAsync(buffer, bytesRead, jsonLength - bytesRead);
        }

        return Encoding.UTF8.GetString(buffer);
    }

    // 发送数据包
    private void SendPacket(NetworkStream stream, byte[] data)
    {
        var length = data.Length;
        var lengthBytes = EncodeVarInt(length);

        stream.Write(lengthBytes, 0, lengthBytes.Length); // 发送数据包长度
        stream.Write(data, 0, data.Length); // 发送数据包内容
    }

    // 写入 VarInt
    private void WriteVarInt(BinaryWriter writer, int value)
    {
        var bytes = EncodeVarInt(value);
        writer.Write(bytes);
    }

    // 写入字符串
    private void WriteString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteVarInt(writer, bytes.Length);
        writer.Write(bytes);
    }

    // 读取 VarInt
    private int ReadVarInt(NetworkStream stream)
    {
        int value = 0;
        int length = 0;
        byte currentByte;

        while (true)
        {
            currentByte = (byte)stream.ReadByte();
            value |= (currentByte & 0x7F) << (length * 7);

            length++;
            if (length > 5)
                throw new Exception("VarInt 太大");

            if ((currentByte & 0x80) != 0x80)
                break;
        }

        return value;
    }

    // 编码 VarInt
    private byte[] EncodeVarInt(int value)
    {
        var bytes = new System.Collections.Generic.List<byte>();
        while ((value & -128) != 0)
        {
            bytes.Add((byte)(value & 127 | 128));
            value = (int)(((uint)value) >> 7);
        }
        bytes.Add((byte)value);
        return bytes.ToArray();
    }

    // 保存服务器图标
    private IImage GetFavicon(string faviconBase64)
    {
        // 去掉 Base64 前缀
        var base64Data = faviconBase64.Split(',')[1];

        // 解码 Base64 数据
        var iconData = Convert.FromBase64String(base64Data);
        using (var memoryStream = new MemoryStream(iconData))
        {
            var bitmap = new Bitmap(memoryStream);

            // 将 Bitmap 设置为 Image 控件的 Source
            return bitmap;
        }
    }
    

// 定义服务器信息类
    private class MinecraftServerInfo
    {
        public VersionInfo version { get; set; }
        public PlayersInfo players { get; set; }
        public description description { get; set; }
        public string favicon { get; set; } // 服务器图标（Base64 编码）
    }

    private class VersionInfo
    {
        public string name { get; set; }
        public int protocol { get; set; }
    }

    private class PlayersInfo
    {
        public int max { get; set; }
        public int online { get; set; }
    }

    private class description
    {
        public string text { get; set; }
    }
}