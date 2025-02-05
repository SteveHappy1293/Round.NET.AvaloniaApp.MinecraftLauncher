using FluentAvalonia.UI.Windowing;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Message;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Models;

public class Core
{
    public static AppWindow MainWindow { get; set; }
    public static SystemTaskBox SystemTask { get; set; }
    public static SystemMessageBox SystemMessage { get; set; }
    public static Download DownloadPage { get; set; } = new();
}