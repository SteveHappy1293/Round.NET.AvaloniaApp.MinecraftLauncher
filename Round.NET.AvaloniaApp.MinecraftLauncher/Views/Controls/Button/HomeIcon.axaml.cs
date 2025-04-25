using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class HomeIcon : UserControl
{
    public FluentIconSymbol Icon { get; set; } = FluentIconSymbol.Home20Regular;

    public EventHandler<RoutedEventArgs> Click
    {
        get { return _click; }
        set
        {
            this.Button.Click -= _click;
            _click = value;
            this.Button.Click += value;
        }
    }

    private EventHandler<RoutedEventArgs> _click { get; set; }
    public HomeIcon()
    {
        InitializeComponent();
        this.FluentIcon.Icon = Icon;
        Click = Button_OnClick;
    }
    
    private static void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        // Core.MainWindow.MainView.SystemNavigationBar.NavTo("Launcher");
    }
}