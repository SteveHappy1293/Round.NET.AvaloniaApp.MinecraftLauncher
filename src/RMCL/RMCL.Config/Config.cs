using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using RMCL.PathsDictionary;

namespace RMCL.Config;

public class Config
{
    public static ConfigRoot MainConfig = new();
    public static void LoadConfig()
    {
        if (!File.Exists(Path.GetFullPath(PathDictionary.ConfigPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathDictionary.ConfigPath));
            SaveConfig();
            return;
        }
        
        var json = File.ReadAllText(Path.GetFullPath(PathDictionary.ConfigPath));
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
        string jsresult = JsonSerializer.Serialize(MainConfig, new JsonSerializerOptions() { WriteIndented = true }); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(PathDictionary.ConfigPath), jsresult);
    }
}