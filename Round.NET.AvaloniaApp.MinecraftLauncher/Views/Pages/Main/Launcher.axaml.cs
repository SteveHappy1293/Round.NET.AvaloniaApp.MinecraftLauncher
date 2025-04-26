using System;
using System.Collections.Generic;
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
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Mange.TilesMange;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Initialize;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Launcher : UserControl,IParentPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>());
    }

    public Launcher()
    {
        InitializeComponent();
#if DEBUG
        DebugBox.IsVisible = true;
#else
        DebugBox.IsVisible = false;
#endif
        TilesMange.TilesPanel = this.WrapPanel;
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (MessageBox.Width != 300)
                    {
                        MessageBox.Width = 300;
                        MessageBox.Description = SystemMessageTaskMange.Message;
                    }
                    else
                    {
                        MessageBox.Width = 280;   
                        MessageBox.Description = "全新一代 RMCL，不太一样，大不一样。";
                    }
                });
                Thread.Sleep(5000);
            }
        });
        Task.Run(() =>
        {
            Thread.Sleep(800);
            Dispatcher.UIThread.Invoke(() =>
                LaunchBored.Margin = new Thickness(0));
            Dispatcher.UIThread.Invoke(() =>
                LaunchBored.Opacity = 1);
        });
        IsEdit = true;
        Task.Run(() =>
        {
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                Dispatcher.UIThread.Invoke(()=>TimeBox.Content = time);
                Thread.Sleep(100);
            }
        });
        Task.Run(() =>
        {
            Thread.Sleep(1000);
            Dispatcher.UIThread.Invoke(HomeBodyMange.Load);
        });
    }

    private bool IsEdit = false;
    private void MessageBox_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.SystemTask.Show();
    }
    
    private void UserButton_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            // Config.MainConfig.SelectedUser = UserButton.SelectedIndex;
            Config.SaveConfig();
        }
    }

    /*private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.MainWindow.MainView.SystemNavigationBar.Show();
    }*/

    private void GotoAccount(object? sender, RoutedEventArgs e)
    {
        Core.MainWindow.MainView.ContentFrame.Content = new Account.Account();
        Core.MainWindow.MainView.ContentFrame.Opacity = 1;
        Core.MainWindow.MainView.MainContent.Opacity = 0;
        
        Core.NavigationBar.Opacity = 0;
    }

    private void ErrorButton(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}