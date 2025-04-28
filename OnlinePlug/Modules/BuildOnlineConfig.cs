using System.Text.Json;

namespace OnlinePlug.Modules;

public class BuildOnlineConfig
{
    public readonly static string Filep = "../RMCL/RMCL.OnlinePlug/Config/OnlineConfig.json";
    public readonly static string OpenP2PFilePath = "../RMCL/RMCL.OnlinePlug/Console/config.json";
    public static OnlineConfigEntry config = new();
    public static void Read()
    {
        if (!File.Exists(Filep))
        {
            Save();
        }
        var jsonString = File.ReadAllText(Filep);
        config = JsonSerializer.Deserialize<OnlineConfigEntry>(jsonString);
        config.apps.Clear();
    }
    public static void Save()
    {
        var jsonString = JsonSerializer.Serialize(config);
        File.WriteAllText(Filep, jsonString);
        File.WriteAllText(OpenP2PFilePath, jsonString);
    }
}