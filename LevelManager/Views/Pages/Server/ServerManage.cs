using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Extensions;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.VersionServerMange.Library.Entry;
using Round.NET.VersionServerMange.Library.Modules;

namespace LevelManager.Views.Pages.Server;

public class ServerManage
{
    public static List<ServerEntry> Servers = new List<ServerEntry>();
    public static readonly string ConfigFile = "../RMCL/RMCL.Config/Server.json";

    public static void Load()
    {
        if (!File.Exists(ConfigFile))
        {
            Save();
            Load();
        }
        var json = File.ReadAllText(Path.GetFullPath(ConfigFile));
        if (string.IsNullOrEmpty(json))
        {
            Save();
        }
        else
        {
            try
            {
                Servers = JsonSerializer.Deserialize<List<ServerEntry>>(json);
            }
            catch
            {
                Save();
            }
        }
    }
    public static void Save()
    {
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(Servers, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(ConfigFile), jsresult);
    }

    public static int ScannedVersion(string versionPath)
    {
        var result = 0;
        var sers = new ServerMangeCore();
        sers.Path = $"{versionPath}\\servers.dat";
        if (File.Exists(sers.Path))
        {
            sers.Load();
            foreach (var ser in sers.Servers)
            {
                if (!Servers.Exists(x=>x.IP==ser.IP))
                {
                    Servers.Add(ser);
                    result++;
                }
            }
            Save();   
        }
        return result;
    }
    public static void UpServer(string uuid)
    {
        var ind = Servers.FindIndex(x => x.SUID == uuid);
        var temp = Servers[ind-1];
        Servers[ind-1] = Servers[ind];
        Servers[ind] = temp;
        
        Save();
    }

    public static  void DownServer(string uuid)
    {
        var ind = Servers.FindIndex(x => x.SUID == uuid);
        var temp = Servers[ind+1];
        Servers[ind+1] = Servers[ind];
        Servers[ind] = temp;
        
        Save();
    }

    public static void AddServer(string name, string ip)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(ip))
        {
            Servers.Add(new ()
            {
                IP = ip,
                Name = name
            });
            Save();
        }
    }

    public static void SetServerIcon(string suid,string icon)
    {
        var ind = Servers.FindIndex(x => x.SUID == suid);
        Servers[ind].Icon = icon;
        Save();
    }
}