using RMCLConfigServer.Modules.Classes;
using RMCLConfigServer.Modules.Classes.HotReload;
using RMCLConfigServer.Modules.Classes.Network;
using Serilog;

namespace RMCLConfigServer.Modules;

public class Init
{
    public static Server Server { get; set; }
    public static void InitServer()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();
        
        
        Log.Information("Starting Config Server!");
        Config.Load();
        ConsoleInfo();
        Log.Information("The configuration is loaded.");
        
        Load();

        if (Config.MainConfig.IsHotReload)
        {
            var hotreload = new HotReload("Config.json");
            hotreload.HotReloadAction = () =>
            {
                Log.Information("Files have been reloaded.");
                Server.Stop();
                Config.Load();
                Load();
            };
            hotreload.Start();
        }
    }

    public static void Load()
    {
        Console.Title = $"RMCL3.0 组织配置集控服务端 - {Config.MainConfig.ServerName}";
        Log.Debug($"Server Port: {Config.MainConfig.ServerPort}");
        Log.Debug($"Server Name: {Config.MainConfig.ServerName}");
        Server = new Server(Config.MainConfig.ServerPort);
        Server.Start();
    }
    public static void ConsoleInfo()
    {
        // 启用ANSI颜色支持（对于Windows 10+）
        EnableANSI();
        // 定义RMCL3的艺术字，每行对应一个字符的5部分
        string[] rArt = {
            @"██████╗ ",
            @"██╔══██╗",
            @"██████╔╝",
            @"██╔══██╗",
            @"██║  ██║",
            @"██║  ██║",
            @"╚═╝  ╚═╝"
        };
        
        string[] mArt = {
            @"███╗   ███╗",
            @"████╗ ████║",
            @"██╔████╔██║",
            @"██║╚██╔╝██║",
            @"██║ ╚═╝ ██║",
            @"██║     ██║",
            @"╚═╝     ╚═╝"
        };
        
        string[] cArt = {
            @" ██████╗",
            @"██╔════╝",
            @"██║     ",
            @"██║     ",
            @"██║     ",
            @"╚██████╗",
            @" ╚═════╝"
        };
        
        string[] lArt = {
            @"██╗     ",
            @"██║     ",
            @"██║     ",
            @"██║     ",
            @"██║     ",
            @"███████╗",
            @"╚══════╝"
        };
        
        string[] threeArt = {
            @"██████╗ ",
            @"╚════██╗",
            @" █████╔╝",
            @" ╚═══██╗",
            @"██████╔╝",
            @"╚═════╝ ",
            @"Round Studio"
        };

        Console.Write("\n");
        // 输出彩色艺术字
        for (int i = 0; i < 7; i++)
        {
            Console.Write("\x1b[38;2;255;105;180m"+ "  " + rArt[i]);
            Console.Write("\x1b[38;2;255;105;180m"+ "  " + mArt[i]);
            Console.Write("\x1b[38;2;255;105;180m"+ "  " + cArt[i]);
            Console.Write("\x1b[38;2;255;105;180m"+ "  " + lArt[i]);
            Console.Write("\x1b[38;5;33m" + "  " +threeArt[i] + "\x1b[0m");
            Console.WriteLine();
        }
    }
    static void EnableANSI()
    {
        try
        {
            // 对于Windows系统启用ANSI转义码
            var os = Environment.OSVersion;
            if (os.Platform == PlatformID.Win32NT)
            {
                var handle = GetStdHandle(-11);
                GetConsoleMode(handle, out uint mode);
                SetConsoleMode(handle, mode | 0x4);
            }
        }
        catch
        {
            // 如果失败，继续尝试（可能颜色不支持）
        }
    }
    
    // Windows API声明
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int handle);
    
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr handle, out uint mode);
    
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr handle, uint mode);
}