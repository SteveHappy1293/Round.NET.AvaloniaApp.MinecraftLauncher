using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnlinePlug.Views.Pages.SubPages;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace OnlinePlug.Views.Pages;

public partial class OnlineMain : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{new MenuItem{Header = "添加用户"},new MenuItem{Header = "刷新"}});
    }
    public OnlineMain()
    {
        InitializeComponent();

        MainFrame.Content = new OnlineHome();
    }
}