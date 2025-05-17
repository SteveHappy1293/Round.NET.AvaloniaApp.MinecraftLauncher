using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Controls.ControlHelper;
using RMCL.Views.Pages.Main.DownloadPages;

namespace RMCL.Views.Pages.Main;

public partial class Download : UserControl
{
    public Download()
    {
        InitializeComponent();
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "DownloadGame",
            Title = "游戏本体",
            Page = new DownloadGame()
        });
    }
}