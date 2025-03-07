using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class BackIcon : UserControl
{
    public IconType Icon { get; set; } = IconType.ArrowLeft;
    public BackIcon()
    {
        InitializeComponent();
        this.HeroIcon.Type = Icon;
    }
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        ((MainView)Core.MainWindow.Content).SystemNavigationBar.NavTo("Launcher");
    }
}