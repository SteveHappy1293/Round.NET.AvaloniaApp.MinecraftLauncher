using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Downloads;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Download : UserControl
{
    public Download()
    {
        InitializeComponent();
    }
    private DownloadGamePage DownloadGamePage { get; set; } = new();
    private DownloadAssetsPage DownloadAssetsPage { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(30,70,30,-10));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                switch (((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag)
                {
                    case "GameDownload":
                        MangeFrame.Content = this.DownloadGamePage;
                        break;
                    case "AssetsDownload":
                        MangeFrame.Content = this.DownloadAssetsPage;
                        break;
                }
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(10,50,10,10));
        });
    }
}