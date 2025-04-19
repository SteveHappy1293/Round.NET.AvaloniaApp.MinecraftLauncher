using System.Diagnostics;
using FluentAvalonia.FluentIcons;
using OnlinePlug.Views.Pages;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace OnlinePlug
{
    class Main
    {
        public static void InitPlug()
        {
            var openp2p = global::OnlinePlug.Properties.Resources.openp2p;
            Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.OnlinePlug"));
            Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.OnlinePlug/Config"));
            Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.OnlinePlug/Console"));
            File.WriteAllBytes(Path.GetFullPath("../RMCL/RMCL.OnlinePlug/Console/openp2p.exe"), openp2p);
            try
            {
                foreach (var process in Process.GetProcesses("openp2p.exe"))
                {
                    process.Kill(true);
                } // 删除旧进程
            }catch { }
            
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