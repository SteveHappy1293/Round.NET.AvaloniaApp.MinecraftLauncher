using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class SearchBox : UserControl
{
    public Action CloseAction { get; set; }
    public SearchBox()
    {
        InitializeComponent();
        IsVisible = false;
        Opacity = 0;
    }

    public void Show()
    {
        if (this.IsVisible)
        {
            Margin = new Thickness(0, 0, 0, 0);
            Opacity = 0;
            ((MainView)Core.MainWindow.Content).SystemNavigationBar.NavTo("BackSearch");
            Task.Run(() =>
            {
                Dispatcher.UIThread.Invoke(() => CloseAction());
                Thread.Sleep(500);
                Dispatcher.UIThread.Invoke(() => this.IsVisible = false);
            });
        }
        else
        {
            this.IsVisible = true;
            this.Opacity = 1;
            ((MainView)Core.MainWindow.Content).SystemNavigationBar.NavTo("Clear",true);
            Margin = new Thickness(0, 0, 0, 0);
        }
    }
    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // 搜索类型选择回调
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        // 搜索框修改输入回调
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Show();
    }
}