namespace RMCL.PathsDictionary;

public class PathDictionary
{
    public static readonly string ConfigPath = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Config/Config.json";
    public static readonly string GameDrawerConfigPath = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Config/GameDrawer.json";
    public static readonly string PlayerConfigPath = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Config/Player.json";
    public static readonly string JavaConfigPath = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Config/Java.json";
    public static readonly string DefaultGameFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Minecraft";
    public static readonly string LogsPath = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Logs";
    public static readonly string UpdateZipFileFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Update/FileZips";
    public static readonly string UpdateFileFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Update/Files";
    public static readonly string PluginFileFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Plugin/Files";
    public static readonly string ExceptionFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Exception";
    
    public static readonly string ClientModsFolderName = "mods";
    public static readonly string ClientModCacheFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.ModCache";
    public static readonly string ClientModDisablePostfix = ".disabled";
    public static readonly string ClientModEnablePostfix = ".jar";

    public static readonly string SkinRootFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Style";
    public static readonly string SkinFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Style/Files";
    public static readonly string SkinTempFolder = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Style/Temp";
    public static readonly string SkinFolderExtract = $"{SystemHelper.SystemHelper.GetAppConfigDirectory()}/RMCL/RMCL.Style/Extract";
}