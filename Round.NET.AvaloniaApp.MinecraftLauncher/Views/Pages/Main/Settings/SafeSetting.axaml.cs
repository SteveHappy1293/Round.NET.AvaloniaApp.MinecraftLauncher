using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe.Exceptions;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class SafeSetting : UserControl
{
    public bool IsEdit { get; set; } = false;
    public SafeSetting()
    {
        InitializeComponent();
        
        ShowErrorWindowButton.IsChecked = Config.MainConfig.ShowErrorWindow;
        IsEdit = true;
    }

    private void ExceptionButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.MainWindow.MainView.CortentFrame.Content = new ExceptionPage();
        Core.MainWindow.MainView.CortentFrame.Opacity = 1;
        Core.MainWindow.MainView.MainCortent.Opacity = 0;
        
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
        Core.MainWindow.MainView.CortentFrame.Content = new IssuesPage();
        Core.MainWindow.MainView.CortentFrame.Opacity = 1;
        Core.MainWindow.MainView.MainCortent.Opacity = 0;
        
        Core.NavigationBar.Opacity = 0;
    }
}