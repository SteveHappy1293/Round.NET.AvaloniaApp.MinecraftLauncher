using System.Text.Json;
using System.Text.RegularExpressions;

namespace RMCL.Config;

public class Config
{
    private const string ConfigFileName = "../RMCL/RMCL.Config/Config.json";
    public static ConfigRoot MainConfig = new();
    public static void LoadConfig()
    {
        if (!File.Exists(Path.GetFullPath(ConfigFileName)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigFileName));
            SaveConfig();
            return;
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