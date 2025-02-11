using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Game;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using SkiaSharp;
using User = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.User;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

public class ConfigRoot
{
    public List<GameFolderConfig> GameFolders { get; set; } = new();
    public int SelectedGameFolder { get; set; } = 0;
    public int SelectedJava { get; set; } = 0;
    public int SelectedUser { get; set; } = 0;
    public int BackModlue { get; set; } = 0;
    public string BackImage { get; set; } = string.Empty;
    public double BackOpacity { get; set; } = 50;
    public string StyleFile { get; set; } = string.Empty;
    public int WindowWidth { get; set; } = 850;
    public int WindowHeight { get; set; } = 450;
    public bool IsUsePlug { get; set; } = true;
    public int MessageLiveTimeMs { get; set; } = 5000;
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
                Path = Path.GetFullPath("../RMCL/RMCL.Minecraft")
            }
        }
    };
    
    public const string ConfigFileName = "../RMCL/RMCL.Config/Config.json";
    public static void LoadConfig()
    {
        if (!File.Exists(ConfigFileName))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigFileName));
            SaveConfig();
        }
        var json = File.ReadAllText(Path.GetFullPath(ConfigFileName));
        if (string.IsNullOrEmpty(json))
        {
            SaveConfig();
        }
        else
        {
            try
            {
                MainConfig = JsonSerializer.Deserialize<ConfigRoot>(json);
            }
            catch
            {
                SaveConfig();
            }
        }
    }

    public static void SaveConfig()
    {
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(MainConfig, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(ConfigFileName), jsresult);
    }
}