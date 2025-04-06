using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Avalonia.Controls.Shapes;
using HarfBuzzSharp;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Mange.StarMange;

public class StarGroup
{
    public static List<StarGroupEnrty> StarGroups = new();
    public readonly static string StarSavePath = Path.GetFullPath("../RMCL/RMCL.Star");
    public readonly static string StarSaveJson = Path.Combine(StarSavePath,"index.json");

    public static void LoadStars()
    {
        if(!Directory.Exists(StarSavePath)) SaveStars();
        if(!File.Exists(StarSaveJson)) SaveStars();

        try
        {
            StarGroups = JsonSerializer.Deserialize<List<StarGroupEnrty>>(File.ReadAllText(StarSaveJson));
        }catch { SaveStars(); LoadStars(); }
    }

    public static void SaveStars()
    {
        if(!Directory.Exists(StarSavePath)) Directory.CreateDirectory(StarSavePath);
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(StarGroups, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(StarSaveJson, jsresult);
    }

    public static void RemoveStarGroup(string GUID)
    {
        StarGroups.RemoveAll(x => x.GUID == GUID);
    }

    public static List<StarItemEnrty> GetStarItems(string GUID)
    {
        return StarGroups.Find(x => x.GUID == GUID).Stars;
    }
}