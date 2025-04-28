using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using RMCLConfigServer.Modules.Entry;

namespace RMCLConfigServer.Modules.Classes;

public class Config
{
    public static ConfigEntry MainConfig { get; set; } = new();
    public static void Load()
    {
        if(!File.Exists("Config.json")) Save();
        MainConfig = JsonSerializer.Deserialize<ConfigEntry>(File.ReadAllText("Config.json"));
    }

    public static void Save()
    {
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(MainConfig, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath("Config.json"), jsresult);
    }
}