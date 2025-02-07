using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class SystemNavigationBar : UserControl
{
    private double hei = 64;
    public SystemNavigationBar()
    {
        InitializeComponent();
    }
    private Launcher Launcher { get; } = new();
    private Download Download { get; } = Core.DownloadPage;
    private Mange Mange { get; } = new();
    private Setting Setting { get; } = new();
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        NavTo(((NavigationViewItem)sender!).Tag.ToString());
    }
    public void Show()
    {
        if (this.Opacity == 0)
        {
            Margin = new(0);
            Opacity = 1;
        }
        else
        {
            Margin = new(-80);
            Opacity = 0;
        }
    }
    public void NavTo(string Tag)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 0);
            //Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(50,50,50,50));
            Thread.Sleep(380);
            Dispatcher.UIThread.Invoke(() => Show());
            //Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0,90,0,0));
            Thread.Sleep(100);
            Dispatcher.UIThread.Invoke(() =>
            {
                switch (Tag)
                {
                    case "Download":
                        ((MainView)Core.MainWindow.Content).MainCortent.Content = Download;
                        break;
                    case "Mange":
                        ((MainView)Core.MainWindow.Content).MainCortent.Content = Mange;
                        break;
                    case "Setting":
                        ((MainView)Core.MainWindow.Content).MainCortent.Content = Setting;
                        break;
                    default:
                        ((MainView)Core.MainWindow.Content).MainCortent.Content = Launcher;
                        break;
                }
            });
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0));
        });
    }
}