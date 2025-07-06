using System.Text.Json;
using System.Text.Json.Serialization;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using RMCL.Base.Entry;
using RMCL.Base.Entry.Game.Client;

namespace RMCL.ClientManager;

public class ClientSelfConfig
{
    public static ClientConfig GetClientConfig(LaunchClientInfo info)
    {
        var configFile = Path.Combine(info.GameFolder, "versions", info.GameName, "RMCL", "Client.json");

        if (!File.Exists(configFile)) return Config.Config.MainConfig.PublicClietConfig;
        else return JsonSerializer.Deserialize<ClientConfig>(File.ReadAllText(configFile));
    }

    public static void SaveClientConfig(LaunchClientInfo info,ClientConfig config)
    {
        var configFile = Path.Combine(info.GameFolder, "versions", info.GameName, "RMCL", "Client.json");
        if(!File.Exists(configFile)) Directory.CreateDirectory(Path.GetDirectoryName(configFile));
        
        File.WriteAllText(configFile, JsonSerializer.Serialize(config));
    }
}