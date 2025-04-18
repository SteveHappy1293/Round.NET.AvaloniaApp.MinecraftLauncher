using FluentAvalonia.FluentIcons;
using OnlinePlug.Views.Pages;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace OnlinePlug
{
    class Main
    {
        public static void InitPlug()
        {
            Core.API.RegisterNavigationRoute(new Core.API.NavigationRouteConfig()
            {
                Icon = FluentIconSymbol.Link16Regular,
                Page = new OnlineMain(),
                Route = "OnlineMain",
                Title = "联机大厅"
            });
        }
    }
}