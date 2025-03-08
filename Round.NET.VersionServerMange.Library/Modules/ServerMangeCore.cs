using fNbt;
using Round.NET.VersionServerMange.Library.Entry;

namespace Round.NET.VersionServerMange.Library.Modules;

public class ServerMangeCore
{
    public List<ServerEntry> Servers { get; set; } = new();
    public string Path { get; set; } = string.Empty;
    private NbtFile nbtFile = new ();
    public void Load()
    {
        string filePath = Path;

        if (File.Exists(filePath))
        {
            // 加载 NBT 文件
            nbtFile.LoadFromFile(filePath);

            // 获取根标签
            NbtCompound rootTag = nbtFile.RootTag;

            // 检查是否存在服务器列表
            if (rootTag.Contains("servers"))
            {
                NbtList serversList = rootTag.Get<NbtList>("servers");

                // 遍历服务器列表
                foreach (NbtCompound serverTag in serversList)
                {
                    var icons =
                        "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";
                    string name = serverTag.Get<NbtString>("name").Value;
                    string ip = serverTag.Get<NbtString>("ip").Value;
                    try
                    {
                        string icon = serverTag.Get<NbtString>("icon").Value;
                        icons = icon;
                    }
                    catch
                    {
                    }

                    Servers.Add(new ServerEntry()
                    {
                        Name = name,
                        IP = ip,
                        Icon = icons
                    });
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }

    public void UpServer(string uuid)
    {
        var ind = Servers.FindIndex(x => x.SUID == uuid);
        var temp = Servers[ind-1];
        Servers[ind-1] = Servers[ind];
        Servers[ind] = temp;
        
        Save();
    }

    public void DownServer(string uuid)
    {
        var ind = Servers.FindIndex(x => x.SUID == uuid);
        var temp = Servers[ind+1];
        Servers[ind+1] = Servers[ind];
        Servers[ind] = temp;
        
        Save();
    }

    public void Save()
    {
        // 获取根标签
        NbtCompound rootTag = nbtFile.RootTag;

        // 创建一个新的服务器列表
        NbtList serversList = new NbtList("servers");

        // 遍历 Servers 列表，将每个服务器添加到 NBT 列表中
        foreach (var server in Servers)
        {
            NbtCompound newServer = new NbtCompound
            {
                new NbtString("name", server.Name), // 服务器名称
                new NbtString("ip", server.IP),    // 服务器 IP 或域名
                new NbtString("icon", server.Icon) // 服务器图标
            };

            serversList.Add(newServer);
        }

        // 清空现有的服务器列表（如果存在）
        if (rootTag.Contains("servers"))
        {
            rootTag.Remove("servers");
        }

        // 将新的服务器列表添加到根标签
        rootTag.Add(serversList);

        // 保存修改后的 NBT 文件
        nbtFile.SaveToFile(Path, NbtCompression.None);
    }

    public void AddServer(ServerEntry server)
    {
        NbtCompound newServer = new NbtCompound
        {
            new NbtString("name", server.Name), // 服务器名称
            new NbtString("ip", server.IP),    // 服务器 IP 或域名
            new NbtString("icon", server.Icon)
        };

        // 获取服务器列表
        NbtList serversList = nbtFile.RootTag.Get<NbtList>("servers");

        // 将新服务器添加到服务器列表
        serversList.Add(newServer);
    }

    public void DeleteServer(ServerEntry server)
    {
        foreach (var ser in Servers)
        {
            if (ser.SUID == server.SUID)
            {
                Servers.Remove(ser);
                break;
            }
        }
    }
}