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
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Setting : UserControl
{
    public Setting()
    {
        InitializeComponent();
        Core.SettingPage = this;    
        
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = GameSetting,
            Title = "游戏",
            Route = "GameSetting",
            Icon = FluentIconSymbol.Games20Filled
        });   
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = StyleSetting,
            Title = "个性化",
            Route = "StyleSetting",
            Icon = FluentIconSymbol.StyleGuide20Filled
        });   
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = JavaSetting,
            Title = "Java 与内存",
            Route = "JavaSetting",
            Icon = FluentIconSymbol.CursorHover20Filled
        });  
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = InternetSetting,
            Title = "网络",
            Route = "InternetSetting",
            Icon = FluentIconSymbol.Desktop20Filled
        });   
        
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = SeniorSetting,
            Title = "高级",
            Route = "SeniorSetting",
            Icon = FluentIconSymbol.Clover20Filled
        });  
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = SafeSetting,
            Title = "安全",
            Route = "SafeSetting",
            Icon = FluentIconSymbol.QuestionCircle20Filled
        });
        
        
        RegisterRoute(new Core.API.NavigationRouteConfig()
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
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var item = ((DockPanel)((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content);
        var text = ((TextBlock)(item.Children[1])).Text;
        ControlChange.ChangeLabelText(PageTitleLabel,text);
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,100,10,-10));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                MangeFrame.Content = RouteConfigs.Find((config =>
                {
                    return config.Route == ((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag;
                })).Page;
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,80,10,10));
        });
    }

    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "GameSetting")
        {
            isthis = true;
        }

        var doc = new DockPanel()
        {
            Children =
            {
                new FluentIcon() { Margin = new Thickness(-5,0,0,0),Icon = config.Icon,Width = 15,Height = 15 },
                new TextBlock() { Margin = new Thickness(10,0,0,0),Text = config.Title,Name = "TitleLabel"}
            }
        };
        var item = new NavigationViewItem()
        {
            Tag = config.Route,
            Content = doc,
            IsSelected = isthis
        };
        if (config.IsFoot)
        {
            View.FooterMenuItems.Add(item);
        }
        else
        {
            View.MenuItems.Add(item);
        }
    }
}