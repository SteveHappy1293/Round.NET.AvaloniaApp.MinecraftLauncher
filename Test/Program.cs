using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCPlayerTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Minecraft 玩家信息监控工具");
            Console.WriteLine("--------------------------");

            // 配置参数
            int localPort = 5000;    // 本地监听端口
            string remoteHost = "127.0.0.1";
            int remotePort = 25565;   // 目标MC服务器端口

            var listener = new TcpListener(IPAddress.Any, localPort);
            listener.Start();

            Console.WriteLine($"监听本地端口 {localPort}，转发到 {remoteHost}:{remotePort}");
            Console.WriteLine("仅显示玩家登录/登出信息...\n");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client, remoteHost, remotePort);
            }
        }

        static async Task HandleClientAsync(TcpClient localClient, string remoteHost, int remotePort)
        {
            using (localClient)
            using (var remoteClient = new TcpClient())
            {
                try
                {
                    await remoteClient.ConnectAsync(remoteHost, remotePort);
                    var localStream = localClient.GetStream();
                    var remoteStream = remoteClient.GetStream();

                    var localToRemote = TrackPlayerEvents(localStream, remoteStream);
                    var remoteToLocal = TrackPlayerEvents(remoteStream, localStream);

                    await Task.WhenAny(localToRemote, remoteToLocal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"连接错误: {ex.Message}");
                }
            }
        }

        static async Task TrackPlayerEvents(NetworkStream source, NetworkStream destination)
        {
            byte[] buffer = new byte[4096];
            var parser = new MCPlayerParser();

            try
            {
                while (true)
                {
                    int bytesRead = await source.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // 解析玩家信息
                    parser.ParsePlayerEvents(buffer, bytesRead);

                    // 正常转发数据
                    await destination.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"数据转发错误: {ex.Message}");
            }
        }
    }

    // Minecraft玩家信息解析器
    public class MCPlayerParser
    {
        public void ParsePlayerEvents(byte[] data, int length)
        {
            try
            {
                using (var ms = new System.IO.MemoryStream(data, 0, length))
                using (var br = new System.IO.BinaryReader(ms))
                {
                    // 读取数据包长度 (VarInt)
                    int packetLength = ReadVarInt(br);
                    
                    // 读取数据包ID (VarInt)
                    if (ms.Position < length)
                    {
                        int packetId = ReadVarInt(br);

                        // 登录成功 (包ID 0x02)
                        if (packetId == 0x02)
                        {
                            Guid uuid = new Guid(br.ReadBytes(16));
                            string username = ReadString(br);
                            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 玩家加入: {username} (UUID: {uuid})");
                        }
                        // 玩家列表项 (包ID 0x38 - 1.19+版本)
                        else if (packetId == 0x38)
                        {
                            // 读取动作类型: 0=添加玩家, 1=更新游戏模式, 2=更新延迟, 3=更新显示名称, 4=移除玩家
                            int action = ReadVarInt(br);
                            int count = ReadVarInt(br);
                            
                            for (int i = 0; i < count; i++)
                            {
                                Guid uuid = new Guid(br.ReadBytes(16));
                                
                                if (action == 0) // 添加玩家
                                {
                                    string username = ReadString(br);
                                    int properties = ReadVarInt(br);
                                    // 跳过属性数据
                                    for (int p = 0; p < properties; p++)
                                    {
                                        ReadString(br); // 属性名
                                        ReadString(br); // 属性值
                                        bool isSigned = br.ReadBoolean();
                                        if (isSigned) ReadString(br); // 签名
                                    }
                                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 玩家加入: {username} (UUID: {uuid})");
                                }
                                else if (action == 4) // 移除玩家
                                {
                                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 玩家离开: UUID: {uuid}");
                                }
                                else
                                {
                                    // 跳过其他动作的数据
                                    if (action == 3) 
                                    {
                                        bool hasDisplayName = br.ReadBoolean();
                                        if (hasDisplayName) ReadString(br); // 显示名称
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // 忽略解析错误
            }
        }

        private int ReadVarInt(System.IO.BinaryReader br)
        {
            int value = 0;
            int size = 0;
            byte b;

            do
            {
                b = br.ReadByte();
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5) throw new Exception("VarInt too big");
            } while ((b & 0x80) == 0x80);

            return value;
        }

        private string ReadString(System.IO.BinaryReader br)
        {
            int length = ReadVarInt(br);
            byte[] stringData = br.ReadBytes(length);
            return Encoding.UTF8.GetString(stringData);
        }
    }
}