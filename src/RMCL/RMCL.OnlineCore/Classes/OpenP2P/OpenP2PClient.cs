using System.Text.Json;
using RMCL.OnlineCore.Entry.OpenP2P;

namespace RMCL.OnlineCore.Classes.OpenP2P;

public class OpenP2PClient
{
    private OpenP2PConfigEntry _config = new();
    public OpenP2PClient()
    {
        if(!InitializeEnvironment.IsInitialized) throw new Exception("未初始化环境");
        if(!File.Exists(InitializeEnvironment.OpenP2PCoreFile)) throw new Exception("未找到核心文件，请尝试重新初始化环境 或 关闭杀毒软件后重试");

        _config.Network.Token = InitializeEnvironment.Token;
        _config.Network.User = InitializeEnvironment.User;
        _config.Network.Node = InitializeEnvironment.UUID;
        
        SaveConfig();
    }

    public void SaveConfig()
    {
        var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        File.WriteAllText(Path.Combine(InitializeEnvironment.CorePath, "config.json"), json);
    }
}