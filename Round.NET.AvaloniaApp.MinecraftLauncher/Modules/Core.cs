using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

public class Core
{
    public static AppWindow MainWindow { get; set; }
    public static SystemTaskBox SystemTask { get; set; }
    public static SystemMessageBox SystemMessage { get; set; }
    public static Download DownloadPage { get; set; } = new();
    public static Mange MangePage { get; set; } = new();
    public static Setting SettingPage { get; set; } = new();
    public static SystemNavigationBar NavigationBar { get; set; }
    public class API
    {
        public class NavigationRouteConfig
        {
            public string Route { get; set; }
            public string Title { get; set; }
            public UserControl Page { get; set; }
            public IconType Icon { get; set; }
        }
        public static void RegisterNavigationRoute(NavigationRouteConfig config)
        {
            NavigationBar.RegisterRoute(config);
        }
        public static void RegisterDownloadPage(NavigationRouteConfig config)
        {
            DownloadPage.RegisterRoute(config);
        }
        public static void RegisterMangePage(NavigationRouteConfig config)
        {
            MangePage.RegisterRoute(config);
        }
        public static void RegisterSettingPage(NavigationRouteConfig config)
        {
            SettingPage.RegisterRoute(config);
        }
    }
}