using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using RMCL.Controls.Container;
using RMCL.Controls.ControlHelper;
using RMCL.Core.Views.Pages.Main.SettingPages;
using RMCL.Core.Views.Pages.Main.ManagePages;

namespace RMCL.Core.Views.Pages.Main;

public partial class Setting : UserControl
{
    public Setting()
    {
        InitializeComponent();
        
        var PublicClientSetting = new LaunchSetting();
        PublicClientSetting.Config = Config.Config.MainConfig.PublicClietConfig;
        PublicClientSetting.OnSave = config =>
        {
            Config.Config.MainConfig.PublicClietConfig = config;
            Config.Config.SaveConfig();
        };
        PublicClientSetting.OnLoaded();
        
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "LaunchSetting",
            Title = "全局游戏设置",
            Page = PublicClientSetting,
            Icon = FluentIconSymbol.Rocket20Regular,
        });
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "StyleSetting",
            Title = "个性化",
            Page = new StyleSetting(),
            Icon = FluentIconSymbol.StyleGuide20Regular,
        });
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "DownloadSetting",
            Title = "下载",
            Page = new DownloadSetting(),
            Icon = FluentIconSymbol.ArrowDownload20Regular,
        });
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "JavaSetting",
            Title = "Java 虚拟机和内存",
            Page = new JavaSetting(),
            Icon = FluentIconSymbol.Memory16Regular,
        });
        
        
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "AboutUs",
            Title = "关于我们",
            Page = new AboutUs(),
            Icon = FluentIconSymbol.Info20Regular,
            IsFoot = true
        });
    }
}