using FluentAvalonia.FluentIcons;
using LevelManager.Views.Pages;
using LevelManager.Views.Pages.Server;
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
            Core.API.RegisterMangePage(new Core.API.NavigationRouteConfig()
            {
                Icon = FluentIconSymbol.Server20Filled,
                Page = new ServerManageUI(),
                Route = "ServerManage",
                Title = "服务器管理"
            });
        }
    }
}