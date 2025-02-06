using System.Collections.Generic;
using System.Threading.Tasks;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Components.Fetcher;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Assets;

public interface FindAssets
{
    public static async Task<IEnumerable<CurseForgeResourceEntry>> GetFindAssets(string searchString)
    {
        var find = new CurseForgeFetcher("$2a$10$kId1ZiMP/aZ3YAfD.Ls/5.0xK5IuOrurhFyyqb5lJDqCwrOnSgn9S");
        var result = await find.SearchResourcesAsync(searchString);
        
        return result;
    }
}