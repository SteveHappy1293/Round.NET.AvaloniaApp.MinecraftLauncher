using System.IO;
using System.Text.Json;
using RMCL.Base.Entry.Game.GameDrawer;
using RMCL.PathsDictionary;

namespace RMCL.Core.Models.Classes.Manager.GameDrawerManager;

public class GameDrawerManager
{
    public static GameDrawerRootEntrty GameDrawer { get; set; } = new();

    public static void LoadConfig()
    {
        if (!File.Exists(Path.GetFullPath(PathDictionary.GameDrawerConfigPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PathDictionary.GameDrawerConfigPath));
            SaveConfig();
            return;
        }
        
        var json = File.ReadAllText(Path.GetFullPath(PathDictionary.GameDrawerConfigPath));
        if (string.IsNullOrEmpty(json))
        {
            SaveConfig();
        }
        else
        {
            try
            {
                GameDrawer = JsonSerializer.Deserialize<GameDrawerRootEntrty>(json);
                if (GameDrawer == null)
                {
                    GameDrawer = new GameDrawerRootEntrty();
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
        string jsresult = JsonSerializer.Serialize(GameDrawer, new JsonSerializerOptions() { WriteIndented = true }); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(PathDictionary.GameDrawerConfigPath), jsresult);
    }

    public static void RegisterGroup(GameDrawerGroupEntry info)
    {
        GameDrawer.Groups.Add(info);
        SaveConfig();
    }

    public static void RegisterItem(string parentuuid, GameDrawerItem info)
    {
        var index = GameDrawer.Groups.FindIndex(x => x.Uuid == info.Uuid);
        GameDrawer.Groups[index].Children.Add(info);
        SaveConfig();
    }
}