using System.Diagnostics;
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
        if (args.Length >= 4)
        {
            string bt = "";
            string neobt = "";
            
            for (int i = 0; i < args.Length; i++)
            {
                var se = args[i];
                if (se == "-install") neobt = args[i + 1];
                else if (se == "-body") bt = args[i + 1];
            }
            
            Thread.Sleep(200);
            File.Delete(bt);
            File.Copy(neobt,bt);

            Process.Start(bt);
            Thread.Sleep(100);
            Environment.Exit(0);
        }
        else
        {
            var redirector = new ConsoleRedirector(Path.GetFullPath(Path.Combine(PathDictionary.LogsPath,$"[RMCL.Logger] {DateTime.Now.ToString("yyyy.MM.dd HHmmss.fff")}.log")));
        
            Console.WriteLine("Program Starting!");
            Config.LoadConfig();
            JavaManager.LoadConfig();
            Console.WriteLine("Program Init...");
        
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);   
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}