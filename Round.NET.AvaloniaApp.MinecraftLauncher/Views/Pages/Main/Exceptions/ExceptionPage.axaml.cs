using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountMainPage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountSubPage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Exceptions;

public partial class ExceptionPage : UserControl
{
    public ExceptionPage()
    {
        InitializeComponent();
        this.HomeIcon1.Click = (sender, args) =>
        {
            ((MainView)Core.MainWindow.Content).CortentFrame.Opacity = 0;
            ((MainView)Core.MainWindow.Content).CortentFrame.Content = new Grid();
            ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };
    }
}