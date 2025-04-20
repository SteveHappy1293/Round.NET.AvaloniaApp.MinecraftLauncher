using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;

namespace LevelManager.Views.Pages;

public partial class ContentPageDialog : UserControl
{
    public ContentPageDialog()
    {
        InitializeComponent();
    }

    public string Title { get; set; } = "";
    public Control Page { get; set; } = new Label() { Content = "欢迎来到 RMCL 3!" };
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => this.Opacity = 0);
            Thread.Sleep(200);
            Dispatcher.UIThread.Invoke(() => ((Grid)((MainView)Core.MainWindow.Content).Content).Children.Remove(this));
        });
    }

    public void Show()
    {
        TitleLabel.Content = Title;
        this.Dialog.Margin = new Thickness(0, 100, 0, -100);
        this.Opacity = 0;
        Frame.Content = Page;
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((Grid)((MainView)Core.MainWindow.Content).Content).Children.Add(this));
            Dispatcher.UIThread.Invoke(() => this.Dialog.Margin = new Thickness(0,0,0,0));
            Thread.Sleep(200);
            Dispatcher.UIThread.Invoke(() => this.Opacity = 1);
        });
    }
}