using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Controls.ControlHelper;
using RMCL.Models.Classes;
using RMCL.Views.Pages.Main;

namespace RMCL.Views.Pages;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        BottomBar.ContentFrame = MainFrame;

        BottomBar.RegisterNavigationItem(new BottomBarNavigationEntry()
        {
            IsDefault = true,
            Tag = "Home",
            Title = new Label() { Content = "主页" },
            IsNoButton = true,
            Page = new Home()
        });
    }
}