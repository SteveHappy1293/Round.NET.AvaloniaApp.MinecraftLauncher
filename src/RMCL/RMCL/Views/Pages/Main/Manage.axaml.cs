using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using RMCL.Controls.Container;
using RMCL.Controls.ControlHelper;
using RMCL.Views.Pages.Main.DownloadPages;
using RMCL.Views.Pages.Main.ManagePages;

namespace RMCL.Views.Pages.Main;

public partial class Manage : UserControl
{
    public Manage()
    {
        InitializeComponent();
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "ManageGame",
            Title = "版本列表",
            Page = new ManageGame(),
            Icon = FluentIconSymbol.List20Regular,
        });
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "ManageAccount",
            Title = "账户管理",
            Page = new ManageAccount(),
            Icon = FluentIconSymbol.PersonAccounts20Regular,
        });
    }
}