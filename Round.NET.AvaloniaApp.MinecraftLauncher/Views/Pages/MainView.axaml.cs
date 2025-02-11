using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
        
        this.SystemNavigationBar.NavTo("Launcher");
        
        //Dispatcher.UIThread.Invoke(() => Show());
        this.SystemNavigationBar.Show();
        ThisRipplesControl.CircleShow(0.3);
        MainSearchBox.CloseAction = new Action(() =>
        {
            ThisRipplesControl.CircleShow(0.3);
            SearchGoButton.IsVisible = true;
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        this.MainSearchBox.Show();
        ThisRipplesControl.CircleShow(0.3);
        if (SearchGoButton.IsVisible)
        {
            SearchGoButton.IsVisible = false;
        }
    }
}
