using FluentAvalonia.FluentIcons;
using LevelManager.Views.Pages;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace LevelManager
{
    class Main
    {
        public static void InitPlug()
        {
            Core.API.RegisterMangePage(new Core.API.NavigationRouteConfig()
            {
                Icon = FluentIconSymbol.Map20Filled,
                Page = new LevelManage(),
                Route = "LevelManage",
                Title = "存档管理"
            });
        }
    }
}