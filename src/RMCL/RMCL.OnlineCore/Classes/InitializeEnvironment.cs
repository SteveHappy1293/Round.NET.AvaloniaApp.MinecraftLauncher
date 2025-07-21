using Downloader;

namespace RMCL.OnlineCore.Classes;

public class InitializeEnvironment
{
    public static string CorePath { get; private set; } = String.Empty;
    public static string OpenP2PCoreFile { get; private set; } = String.Empty;
    
    public static string ClientID { get; set; } = String.Empty;
    public static string UUID { get; set; } = String.Empty;
    
    public static bool IsInitialized { get; private set; } = false;

    private static string Download32URL { get; set; } = "https://openp2p.cn/download/v1/3.24.13/openp2p32.exe";
    private static string Download64URL { get; set; } = "https://openp2p.cn/download/v1/3.24.13/openp2p64.exe";

    public static bool Initialize(string corePath)
    {
        CorePath = corePath;

        OpenP2PCoreFile = Path.Combine(CorePath, "OpenP2P.exe");
        if(File.Exists(OpenP2PCoreFile)) IsInitialized = true;

        Directory.CreateDirectory(CorePath);
        IsInitialized = false;

        return IsInitialized;
    }

    public static async Task OnDownloadCoreFile()
    {
        string downloadUrl = "";
        bool is64Bit = Environment.Is64BitOperatingSystem;
        var architecture = is64Bit ? "x64" : "x86";
        Console.WriteLine("当前系统是：{0}", architecture);

        if (architecture == "x64") downloadUrl = Download64URL;
        else downloadUrl = Download32URL;
        
        DownloadService downloadService = new DownloadService();
        await downloadService.DownloadFileTaskAsync(downloadUrl, OpenP2PCoreFile);
        IsInitialized = true;
    }
}