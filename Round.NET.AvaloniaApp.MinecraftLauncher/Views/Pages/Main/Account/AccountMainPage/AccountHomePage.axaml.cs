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
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountSubPage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountMainPage;

public partial class AccountHomePage : UserControl
{
    public AccountHomePage()
    {
        InitializeComponent();

        Core.API.RegisterAccountPage(new Core.API.NavigationRouteConfig()
        {
            Title = "我的账户",
            Route = "MyAccount",
            Page = MyAccount
        });
        Core.API.RegisterAccountPage(new Core.API.NavigationRouteConfig()
        {
            Title = "大厅",
            Route = "MyHall",
            Page = MyHall
        });
    }
    public MyAccount MyAccount { get; set; } = new();
    public MyHall MyHall { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        ControlChange.ChangeLabelText(PageTitleLabel,((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content.ToString());
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,49,10,-10));
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
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,29,10,10));
        });
    }
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "MyAccount")
        {
            isthis = true;
        }

        var item = new NavigationViewItem()
        {
            Tag = config.Route,
            Content = config.Title,
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