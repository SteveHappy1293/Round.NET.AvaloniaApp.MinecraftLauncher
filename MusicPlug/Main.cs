using FluentAvalonia.FluentIcons;
using MusicPlug.Views.Pages;
using MusicPlug.Views.Windows;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace MusicPlug
{
    class Main
    {
        public static void InitPlug()
        {
            Core.API.RegisterNavigationRoute(new Core.API.NavigationRouteConfig()
            {
                Icon = FluentIconSymbol.MusicNote120Regular,
                ParentPage = new MusicPage(),
                Route = "Music",
                Title = "音乐"
            });
        }
    }
}