using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges.VersionSettings;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges;

public partial class GameVersionSetting : UserControl
{
    public string _version;

    public string version
    {
        get { return _version; }
        set
        {
            _version = value;
            var vas = new VersionAttributesSetting();
            vas.version = _version;
            RegisterRoute(new Core.API.NavigationRouteConfig()
            {
                Page = vas,
                Title = "版本概述",
                Route = "VersionAttributesSetting",
            });
        }
    }

    public GameVersionSetting()
    {
        InitializeComponent();
    }
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        ControlChange.ChangeLabelText(PageTitleLabel,((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content.ToString());
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(130,50,10,10));
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
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(130,30,10,10));
        });
    }
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "VersionAttributesSetting")
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