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
using Avalonia.Styling;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Converters;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Mange.TilesMange;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Initialize;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Launcher : UserControl,IParentPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>());
    }

    public void AddNewCard(string Title, Control Content, string Tag)
    {
        var border = new Border { CornerRadius = new CornerRadius(5) ,Background = new SolidColorBrush(Colors.Black, .4),Margin = new Thickness(0,0,0,4)};
        var grid = new Grid { Margin = new Thickness(12)};
        grid.Children.Add(new Button
        {
            Content = new SymbolIcon
            {
                Symbol = Symbol.Cancel, VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            },
            Width = 32, Height = 32, HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top, BorderThickness = new Thickness(0), Background = null,
            Margin = new Thickness(2, 0)
        });
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(new Label
            { FontSize = 24, FontWeight = FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4), Content = Title });
        stackPanel.Children.Add(Content);
        stackPanel.Tag = Tag;
        grid.Children.Add(stackPanel);
        border.Child = grid;
        MainScreen.Children.Add(border);
    }

    public void RemoveCard(string Tag)
    {
        foreach (var c in MainScreen.Children)
        {
            if (c.Tag == Tag)
                MainScreen.Children.Remove(c);
        }
    }
    public void InitHomePage()
    {
        if (Config.MainConfig.HomeBody == MainHomeType.Card)
        {
            AddNewCard("Minecraft 新闻", new FlipView { Height = 350 }, "NewsCard");
            /*
            var group1 = TilesMange.RegisterTileGroup();
            foreach (GameFolderConfig game in Config.Config.MainConfig.GameFolders)
            {
                foreach (var VARIABLE in Directory.GetDirectories($"{game.Path}/versions"))
                {
                    TilesMange.RegisterTile(group1, new TilesMange.TileItem()
                    {
                        Text = Path.GetFileName(VARIABLE),
                        TiteEvent = ()=>{

                        },
                        TiteStyle = TilesMange.TileItem.TiteStyleType.Normal,
                        Content = new FluentIcon()
                        {
                            Foreground = Brushes.White,
                            Icon = FluentIconSymbol.Folder20Regular,
                            Width = 16,
                            Height = 16
                        }
                    });
                }
            }
            */
        }
    }
    public Launcher()
    {
        InitializeComponent();
        /*
#if DEBUG
        //DebugBox.IsVisible = true;
#else
        //DebugBox.IsVisible = false;
#endif
        TilesMange.TilesPanel = this.WrapPanel;
        /*Task.Run(() =>
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
            //Thread.Sleep(800);
            //Dispatcher.UIThread.Invoke(() =>
              //  LaunchBored.Margin = new Thickness(0));
           // Dispatcher.UIThread.Invoke(() =>
             //   LaunchBored.Opacity = 1);
        });
        IsEdit = true;
        Task.Run(() =>
        {
            while (true)
            {
                //var time = DateTime.Now.ToString("HH:mm:ss");
               // Dispatcher.UIThread.Invoke(()=>TimeBox.Content = time);
                Thread.Sleep(100);
            }
        });
        Task.Run(() =>
        {
            Thread.Sleep(1000);
            Dispatcher.UIThread.Invoke(HomeBodyMange.Load);
        });*/
    }
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