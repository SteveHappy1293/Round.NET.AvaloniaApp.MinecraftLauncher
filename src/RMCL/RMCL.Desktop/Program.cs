using Avalonia;
using RMCL;
using RMCL.Config;
using RMCL.JavaManager;
using RMCL.Logger;
using RMCL.PathsDictionary;

sealed class Program
{
    public static void Main(string[] args)
    {
        var redirector = new ConsoleRedirector(Path.GetFullPath(Path.Combine(PathDictionary.LogsPath,$"[RMCL.Logger] {DateTime.Now.ToString("yyyy.MM.dd HHmmss.fff")}.log")));
        
        Console.WriteLine("Program Starting!");
        Config.LoadConfig();
        JavaManager.LoadConfig();
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