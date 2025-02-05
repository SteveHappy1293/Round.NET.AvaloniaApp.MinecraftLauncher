using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;

namespace Round.NET.AvaloniaApp.MinecraftLauncher;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        Directory.CreateDirectory(Path.GetFullPath("../RMCL.Minecraft"));
        Config.LoadConfig();
        // CirculateTask.RunThread();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
