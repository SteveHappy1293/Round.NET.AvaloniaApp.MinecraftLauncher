using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using MusicPlug.Views.Pages.SubPages;
using MusicPlug.Views.Windows;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace MusicPlug.Views.Pages;

public partial class MusicPage : UserControl,IParentPage
{
    public PlayerWindow PlayerWindow = new PlayerWindow();
    public void Open()
    {
        ContentPagesPanel.ChangeSelectItemMenu();
    }
    public MusicPage()
    {
        InitializeComponent();
        PlayerWindow.Show();
        
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = new SearchMusic(),
            Route = "SearchMusic",
            Title = "搜索音乐",
            Icon = FluentIconSymbol.BoxSearch20Filled
        });
        
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = new LocalMusic(),
            Route = "LocalMusic",
            Title = "本地音乐",
            Icon = FluentIconSymbol.BoxCheckmark20Filled
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        PlayerWindow.Close();
        PlayerWindow = new PlayerWindow();
        PlayerWindow.Show();
    }
}