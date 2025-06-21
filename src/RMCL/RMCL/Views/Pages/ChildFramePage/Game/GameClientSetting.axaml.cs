using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using RMCL.Controls.ControlHelper;
using RMCL.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

namespace RMCL.Views.Pages.ChildFramePage.Game;

public partial class GameClientSetting : UserControl
{
    public GameClientSetting()
    {
        InitializeComponent();
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Airplane20Regular,
            Title = "启动设置",
            Route = "GameClientSetting",
            Page = new ClientLaunchSetting()
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Archive20Regular,
            Title = "存档",
            Route = "ClientArchiveSetting",
            Page = new ClientArchiveSetting()
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Screenshot20Regular,
            Title = "截图",
            Route = "ClientScreenshotSetting",
            Page = new ClientScreenshotSetting()
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.PlugConnected20Regular,
            Title = "模组",
            Route = "ClientModSetting",
            Page = new ClientModSetting()
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Backpack20Regular,
            Title = "资源包",
            Route = "ClientResourcePackSetting",
            Page = new ClientResourcePackSetting()
        });
    }
}