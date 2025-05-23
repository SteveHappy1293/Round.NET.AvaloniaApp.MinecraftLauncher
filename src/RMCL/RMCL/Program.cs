using Avalonia;
using System;

namespace RMCL;

sealed class Program
{
    public static void Main(string[] args)
    {
        Config.Config.LoadConfig();
        JavaManager.JavaManager.LoadConfig();
        
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