using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class BackIcon : UserControl
{
    public FluentIconSymbol Icon { get; set; } = FluentIconSymbol.Home20Regular;
    public BackIcon()
    {
        InitializeComponent();
        this.FluentIcon.Icon = Icon;
    }
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        // Core.MainWindow.MainView.SystemNavigationBar.NavTo("Launcher");
    }
}