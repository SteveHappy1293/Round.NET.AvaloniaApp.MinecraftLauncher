using Avalonia;
using System;
using System.IO;
using RMCL.Logger;

namespace RMCL;

sealed class Program
{
    public static void Main(string[] args)
    {
        var redirector = new ConsoleRedirector(Path.GetFullPath($"../RMCL/RMCL.Logs/[RMCL.Logger] {DateTime.Now.ToString("yyyy.MM.dd HHmmss.fff")}.log"));
        
        Console.WriteLine("Program Starting!");
        Config.Config.LoadConfig();
        JavaManager.JavaManager.LoadConfig();
        Console.WriteLine("Program Init...");
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}