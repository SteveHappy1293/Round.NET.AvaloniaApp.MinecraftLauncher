using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Flurl.Util;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;
using SixLabors.ImageSharp;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Setting : UserControl,IParentPage
{
    public void Open()
    {
        ContentPagesPanel.ChangeSelectItemMenu();
    }


    public Setting()
    {
        InitializeComponent();
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = GameSetting,
            Title = "游戏",
            Route = "GameSetting",
            Icon = FluentIconSymbol.Games20Filled
        });   
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = StyleSetting,
            Title = "个性化",
            Route = "StyleSetting",
            Icon = FluentIconSymbol.StyleGuide20Filled
        });   
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = JavaSetting,
            Title = "Java 与内存",
            Route = "JavaSetting",
            Icon = FluentIconSymbol.CursorHover20Filled
        });  
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = InternetSetting,
            Title = "网络",
            Route = "InternetSetting",
            Icon = FluentIconSymbol.Desktop20Filled
        });   
        
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = SeniorSetting,
            Title = "高级",
            Route = "SeniorSetting",
            Icon = FluentIconSymbol.Clover20Filled
        });
#if DEBUG
        /*RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = new ConfigSetting(),
            Title = "配置",
            Route = "ConfigSetting",
            Icon = FluentIconSymbol.Organization20Filled
        });*/
        RLogs.WriteLog("当前开发版已移除组织更新");
#endif
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = SafeSetting,
            Title = "安全",
            Route = "SafeSetting",
            Icon = FluentIconSymbol.QuestionCircle20Filled
        });
        
        
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = AboutRMCL,
            Title = "关于 RMCL",
            Route = "AboutRMCL",
            IsFoot = true,
            Icon = FluentIconSymbol.Info20Filled
        });  
    }
    private GameSetting GameSetting { get; set; } = new();
    private StyleSetting StyleSetting { get; set; } = new();
    private SeniorSetting SeniorSetting { get; set; } = new();
    private InternetSetting InternetSetting { get; set; } = new();
    private JavaSetting JavaSetting { get; set; } = new();
    private AboutRMCL AboutRMCL { get; set; } = new();
    private SafeSetting SafeSetting { get; set; } = new();
}