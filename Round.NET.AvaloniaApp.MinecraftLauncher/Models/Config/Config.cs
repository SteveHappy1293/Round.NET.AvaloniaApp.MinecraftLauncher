using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MinecraftLaunch.Classes.Models.Game;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Java;
using SkiaSharp;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Models.Config;

public class ConfigRoot
{
    public List<GameFolderConfig> GameFolders { get; set; } = new();
    public List<UserConfig> Users { get; set; } = new() { new UserConfig() };
    public List<JavaEntry> Javas { get; set; } = null;
    public int SelectedGameFolder { get; set; } = 0;
    public int SelectedJava { get; set; } = 0;
    public int SelectedUser { get; set; } = 0;
}

public class UserConfig
{
    public string UserName { get; set; } = "Steve";
}
public class GameFolderConfig
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int SelectedGameIndex { get; set; } = 0;
}

public class Config
{
    public static ConfigRoot MainConfig = new()
    {
        GameFolders = new()
        {
            new GameFolderConfig
            {
                Name = "当前文件夹",
                Path = Path.GetFullPath("../RMCL.Minecraft")
            }
        }
    };
    
    public const string ConfigFileName = "../RMCL.Config/Config.json";
    public static void LoadConfig()
    {
        if (!File.Exists(ConfigFileName))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigFileName));
            SaveConfig();
        }
        var json = File.ReadAllText(Path.GetFullPath(ConfigFileName));
        MainConfig = JsonSerializer.Deserialize<ConfigRoot>(json);
    }

    public static void SaveConfig()
    {
        string result = Regex.Unescape(JsonSerializer.Serialize(MainConfig, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(ConfigFileName), result);
    }
}