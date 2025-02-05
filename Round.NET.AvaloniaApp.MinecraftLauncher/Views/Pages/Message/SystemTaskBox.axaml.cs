using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Message;

public partial class SystemTaskBox : UserControl
{
    public SystemTaskBox()
    {
        InitializeComponent();
        Core.SystemTask = this;
        Show();
        Task.Run(() =>
        {
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                Dispatcher.UIThread.Invoke(()=>TimeBox.Content = time);
                Thread.Sleep(100);
            }
        });
    }

    public void Show()
    {
        if (IsVisible)
        {
            MainPanel.Margin = new Thickness(0,40,-380,0);
            BackGrid.Opacity = 0;
            TimeBox.Margin = new Thickness(-50,50);
            Trip1Box.Margin = new Thickness(-50,160);
            Task.Run(() =>
            {
                Thread.Sleep(800);
                Dispatcher.UIThread.Invoke(() => this.IsVisible = false);
            });
        }
        else
        {
            MainPanel.Margin = new Thickness(0,40,10,0);
            BackGrid.Opacity = 0.6;
            this.IsVisible = true;
            TimeBox.Margin = new Thickness(50);
            Trip1Box.Margin = new Thickness(50,160);
        }
    }

    private void BackGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Core.SystemTask.Show();
    }
}