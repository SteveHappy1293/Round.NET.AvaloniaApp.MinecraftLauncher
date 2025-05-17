using OverrideLauncher.Core.Modules.Classes.Download;

namespace RMCL.DownloadService;

public class UpdateMinecraftVersions
{
    public static OverrideLauncher.Core.Modules.Entry.DownloadEntry.VersionManifestEntry.Version[] Load()
    {
        var result = DownloadVersionHelper.GetVersionManifest().Result.Versions;
        return result;
    }
}