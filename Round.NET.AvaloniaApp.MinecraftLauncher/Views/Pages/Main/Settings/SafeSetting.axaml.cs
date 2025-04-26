using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe.Exceptions;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class SafeSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{new MenuItem{Header = "添加用户"},new MenuItem{Header = "刷新"}});
    }
    public bool IsEdit { get; set; } = false;
    public SafeSetting()
    {
        InitializeComponent();
        
        ShowErrorWindowButton.IsChecked = Config.MainConfig.ShowErrorWindow;
        IsEdit = true;
    }

    private void ExceptionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.MainWindow.MainView.ContentFrame.Content = new ExceptionPage();
        Core.MainWindow.MainView.ContentFrame.Opacity = 1;
        Core.MainWindow.MainView.MainContent.Opacity = 0;
        
        Core.NavigationBar.Opacity = 0;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.ShowErrorWindow = (bool)ShowErrorWindowButton.IsChecked;
            Config.SaveConfig();
        }
    }

    private void IssuesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.MainWindow.MainView.ContentFrame.Content = new IssuesPage();
        Core.MainWindow.MainView.ContentFrame.Opacity = 1;
        Core.MainWindow.MainView.MainContent.Opacity = 0;
        
        Core.NavigationBar.Opacity = 0;
    }
}