using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using LaunchJavaEdtion = Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch.LaunchJavaEdtion;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Mange : UserControl
{
    public Mange()
    {
        InitializeComponent();
        
    }

    private GameMange GameMange { get; set; } = new();
    private UserMange UserMange { get; set; } = new();
    private JavaMange JavaMange { get; set; } = new();
    private PlugMange PlugMange { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(30,70,30,-10));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                switch (((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag)
                {
                    case "GameMange":
                        MangeFrame.Content = this.GameMange;
                        break;
                    case "UserMange":
                        MangeFrame.Content = UserMange;
                        break;
                    case "JavaMange":
                        MangeFrame.Content = JavaMange;
                        break;
                    case "PlugMange":
                        MangeFrame.Content = PlugMange;
                        break;
                }
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(10,50,10,10));
        });
    }
}