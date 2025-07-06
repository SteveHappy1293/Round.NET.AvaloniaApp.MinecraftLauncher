using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using OverrideLauncher.Core.Modules.Classes.Version;
using RMCL.Base.Entry;
using RMCL.Controls.ControlHelper;
using RMCL.Core.Models.Classes.Launch;
using RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;
using RMCL.PathsDictionary;
using Path = System.IO.Path;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game;

public partial class GameClientSetting : UserControl
{
    private ClientLaunchSetting ClientLaunchSetting = new ClientLaunchSetting();
    private ClientArchiveSetting ClientArchiveSetting = new ClientArchiveSetting();
    private ClientScreenshotSetting ClientScreenshotSetting = new ClientScreenshotSetting();
    private ClientModSetting ClientModSetting = new ClientModSetting();
    private ClientResourcePackSetting ClientResourcePackSetting = new ClientResourcePackSetting();
    
    public VersionParse _versionParse;
    public GameClientSetting(VersionParse version)
    {
        _versionParse = version;
        InitializeComponent();
        this.Version.Text = version.ClientInstances.GameName;
        var path = Path.Combine(version.ClientInstances.GameCatalog, "versions", version.ClientInstances.GameName);
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Airplane20Regular,
            Title = "启动设置",
            Route = "GameClientSetting",
            Page = ClientLaunchSetting
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Archive20Regular,
            Title = "存档",
            Route = "ClientArchiveSetting",
            Page = ClientArchiveSetting
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Screenshot20Regular,
            Title = "截图",
            Route = "ClientScreenshotSetting",
            Page = ClientScreenshotSetting
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.PlugConnected20Regular,
            Title = "模组",
            Route = "ClientModSetting",
            Page = ClientModSetting
        });
        this.NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Icon = FluentIconSymbol.Backpack20Regular,
            Title = "资源包",
            Route = "ClientResourcePackSetting",
            Page = ClientResourcePackSetting
        });
        
        ClientLaunchSetting.Version = version;
        ClientArchiveSetting.Path = path;
        ClientScreenshotSetting.Path = path;
        ClientModSetting.Path = Path.Combine(path, PathDictionary.ClientModsFolderName);
        ClientResourcePackSetting.Path = path;
        
        ClientLaunchSetting.UpdateUI();
        ClientArchiveSetting.UpdateUI();
        ClientScreenshotSetting.UpdateUI();
        ClientResourcePackSetting.UpdateUI();
    }

    private void TestLaunch_OnClick(object? sender, RoutedEventArgs e)
    {
        LaunchService.LaunchTask(new LaunchClientInfo()
        {
            GameFolder = _versionParse.ClientInstances.GameCatalog,
            GameName = _versionParse.ClientInstances.GameName
        });
    }
}