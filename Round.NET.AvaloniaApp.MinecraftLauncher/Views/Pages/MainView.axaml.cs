using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    private Launcher Launcher { get; } = new();
    private Download Download { get; } = Core.DownloadPage;
    private Mange Mange { get; } = new();
    private Setting Setting { get; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => MainCortent.Opacity = 0);
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                switch (((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag)
                {
                    case "Launcher":
                        MainCortent.Content = Launcher;
                        break;
                    case "Download":
                        MainCortent.Content = Download;
                        break;
                    case "Mange":
                        MainCortent.Content = Mange;
                        break;
                    case "Setting":
                        MainCortent.Content = Setting;
                        break;
                }
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MainCortent.Opacity = 1);
        });
    }
}
