using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using fNbt;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges.VersionSettings;

namespace LevelManager.Views.Pages;

public partial class LevelSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{new MenuItem{Header = "添加用户"},new MenuItem{Header = "刷新"}});
    }
    public string nbtfilepath = "";
    public NbtFile _nbt;

    public NbtFile nbt
    {
        get { return _nbt; }
        set
        {
            _nbt = value;
            var Setting = new GenericSetting(nbtfilepath,value);
            var gamerule = new GameRuleSetting(nbtfilepath,value);
            var player = new PlayerSetting(nbtfilepath,value);
            RegisterRoute(new Core.API.NavigationRouteConfig()
            {
                Page = Setting,
                Title = "存档设置",
                Route = "GenericSetting",
            });
            RegisterRoute(new Core.API.NavigationRouteConfig()
            {
                Page = gamerule,
                Title = "游戏规则",
                Route = "GameRuleSetting",
            });
            RegisterRoute(new Core.API.NavigationRouteConfig()
            {
                Page = player,
                Title = "玩家设置",
                Route = "PlayerSetting",
            });
        }
    }

    public LevelSetting()
    {
        InitializeComponent();
    }
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        ControlChange.ChangeLabelText(PageTitleLabel,((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content.ToString());
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                MangeFrame.Content = RouteConfigs.Find((config =>
                {
                    return config.Route == ((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag;
                })).Page;
            });
        });
    }
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "GenericSetting")
        {
            isthis = true;
        }
        View.MenuItems.Add(new NavigationViewItem()
        {
            Tag = config.Route,
            Content = config.Title,
            IsSelected = isthis
        });
    }
}