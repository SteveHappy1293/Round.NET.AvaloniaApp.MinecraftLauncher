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
    public static List<StarGroupEnrty> StarGroups = new(){new StarGroupEnrty()
    {
        GroupName = "默认收藏夹",ImageBase64String = "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAYAAABccqhmAAABhWlDQ1BJQ0MgcHJvZmlsZQAAKJF9kT1Iw0AcxV9TtUUqDhYRcchQxcEuKuJYqlgEC6Wt0KqDyaUfQpOGJMXFUXAtOPixWHVwcdbVwVUQBD9AnB2cFF2kxP8lhRYxHhz34929x907QGhUmGp2xQBVs4x0Ii7m8iti4BVBDKIH4whKzNSTmYUsPMfXPXx8vYvyLO9zf44+pWAywCcSx5huWMTrxDObls55nzjMypJCfE48YdAFiR+5Lrv8xrnksMAzw0Y2PUccJhZLHSx3MCsbKvE0cURRNcoXci4rnLc4q5Uaa92TvzBU0JYzXKc5ggQWkUQKImTUsIEKLERp1Ugxkab9uId/2PGnyCWTawOMHPOoQoXk+MH/4He3ZnFq0k0KxYHuF9v+GAUCu0Czbtvfx7bdPAH8z8CV1vZXG8DsJ+n1thY5Avq3gYvrtibvAZc7wNCTLhmSI/lpCsUi8H5G35QHBm6B3lW3t9Y+Th+ALHW1dAMcHAJjJcpe83h3sLO3f8+0+vsBj+xysjwLdy0AAAAGYktHRAAHAHUAABlZzdQAAAAJcEhZcwAACxMAAAsTAQCanBgAAAAHdElNRQfoCBYGKRO3tNJKAAAAGXRFWHRDb21tZW50AENyZWF0ZWQgd2l0aCBHSU1QV4EOFwAAA4lJREFUeNrt3b1qFFEcxuGszO4aszqbImJjZ4o0Fn6AlSZlCisJiG0ae8GLEOwN6W2ChZVgJWnERiSNoKW1sJPsRzYbWK/B6mU4z3MD/zNnzvw43XTu31lfrhRs0KtWKNf44jI6/zw8/4ojAOUSABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAA4H9VzXgWXUA9WI3OT/8fftCrij6A9t8NABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQABAAAABAAQAEABAAAABAAQAEABAAAABAAQAEACgJap+rxtdwMsnj6Lzb93cyL6Aa9ej8y+nZ9H5s/kiOr9pRtH5B8ff3AAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQABsAUgAIAAAAIACAAgAIAAAAIACAAgAIAAAAIACADQDp3D/d1lcgF1PYxuwGq/W/QBmM0XvoKgphm5AQACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACAAJgC0AAAAEABAAQAEAAAAEABAAQAEAAAAEABAAQAKAdqroeRhew9/Z9dP6zx2+i858//BGdn/4//adf29H5H45fR+cfvXrhBgAIACAAgAAAAgAIACAAgAAAAgAIACAAgAAAAgAIACAAgAAAAgAIAAiALQABAAQAEABAAAABAAQAEABAAAABAAQAEABAAIB26Bzu7y6TC6jrYdEv4Mv3n0U///a9raKfv2lGbgCAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAAACAAgAIACAAIAA2AIQAEAAAAEABAAQAEAAAAEABAAQAEAAAAEABABoh2oynUYXMF5MovM/fz2Jzr97e7PoA/ju6GN0/tOdB9H5p5OFGwAgAIAAAAIACAAgAIAAAAIACAAgAIAAAAIACAAgAIAAAAIACAAgACAAtgAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQDaoar6y+gC/p5l/48+P7+Izj/587vo5+9f7UXnn06y5+/GWtcNABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAQAAAAQAEABAAEABbAAIACAAgAIAAAAIACAAgAIAAAAIACAAgAIAAAO3wD32jWKz9hJX4AAAAAElFTkSuQmCC"
    }};
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

    public static StarGroupEnrty GetStarGroup(string GUID)
    {
        return StarGroups.Find(x => x.GUID == GUID);
    }

    public static void RegisterStarGroup(StarGroupEnrty enrty)
    {
        StarGroups.Add(enrty);
    }

    public static void AddStar(string GroupGUID, StarItemEnrty item)
    {
        StarGroups[StarGroups.FindIndex(x => { return x.GUID == GroupGUID;})].Stars.Add(item);
        SaveStars();
    }
}