using System.IO;
using System.Text.Json;
using RMCL.Base.Entry.User;
using RMCL.Config;
using RMCL.PathsDictionary;

namespace RMCL.Models.Classes.Manager.UserManager;

public class PlayerManager
{
    public static UserRoot Player { get; set; }

    public static void LoadConfig()
    {
        if (!File.Exists(Path.GetFullPath(PathDictionary.PlayerConfigPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathDictionary.PlayerConfigPath));
            SaveConfig();
            return;
        }
        
        var json = File.ReadAllText(Path.GetFullPath(PathDictionary.PlayerConfigPath));
        if (string.IsNullOrEmpty(json))
        {
            SaveConfig();
        }
        else
        {
            try
            {
                Player = JsonSerializer.Deserialize<UserRoot>(json);
                if (Player == null)
                {
                    Player = new UserRoot();
                }
            }
            catch
            {
                SaveConfig();
            }
        }
    }

    public static void SaveConfig()
    {
        string jsresult = JsonSerializer.Serialize(Player, new JsonSerializerOptions() { WriteIndented = true }); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(PathDictionary.PlayerConfigPath), jsresult);
    }
}