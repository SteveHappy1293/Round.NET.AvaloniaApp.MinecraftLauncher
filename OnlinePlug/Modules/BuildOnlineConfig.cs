using System.Text.Json;

namespace OnlinePlug.Modules;

public class BuildOnlineConfig
{
    public readonly static string Filep = "../RMCL/RMCL.OnlinePlug/Config/OnlineConfig.json";
    public static void Read()
    {
        if (!File.Exists(Filep))
        {
            
        }
        var jsonString = File.ReadAllText(Filep);
        var config = JsonSerializer.Deserialize<OnlineConfigEntry>(jsonString);
    }
}