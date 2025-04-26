using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Initialize;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
        //this.SystemNavigationBar.NavTo("Launcher");
        
        //Dispatcher.UIThread.Invoke(() => Show());
        //this.SystemNavigationBar.Show();
        ThisRipplesControl.CircleShow(0.3);
        MainSearchBox.CloseAction = new Action(() =>
        {
            ThisRipplesControl.CircleShow(0.3);
            SearchGoButton.IsVisible = true;
        });
        
        /*Core.API.RegisterSearchItem(new Core.API.SearchItemConfig()
        {
            Title = "返回主页",
            Type = Core.API.SearchItemConfig.ItemType.More,
            Key = new ()
            {
                "主",
                "页",
                "主页",
                "页面",
                "返回",
                "返",
                "回"
            },
            Action = (key) =>
            {
                MainSearchBox.Show();
            }
        });
        Core.API.RegisterSearchItem(new Core.API.SearchItemConfig()
        {
            Title = "管理页",
            Type = Core.API.SearchItemConfig.ItemType.More,
            Key = new ()
            {
                "管",
                "理",
                "管理",
                "页",
                "页面",
                "核心",
                "游戏"
            },
            Action = (key) =>
            {
                MainSearchBox.Show();
                Task.Run(() =>
                {
                    Thread.Sleep(800);
                    Dispatcher.UIThread.Invoke(() => SystemNavigationBar.NavTo("Mange"));
                });
            }
        });
        Core.API.RegisterSearchItem(new Core.API.SearchItemConfig()
        {
            Title = "设置页",
            Type = Core.API.SearchItemConfig.ItemType.More,
            Key = new ()
            {
                "设",
                "理",
                "设置",
                "页",
                "页面",
                "个性化",
                "主题",
                "游戏",
                "本体"
            },
            Action = (key) =>
            {
                MainSearchBox.Show();
                Task.Run(() =>
                {
                    Thread.Sleep(800);
                    Dispatcher.UIThread.Invoke(() => SystemNavigationBar.NavTo("Setting"));
                });
            }
        });
        Core.API.RegisterSearchItem(new Core.API.SearchItemConfig()
        {
            Title = "下载页",
            Type = Core.API.SearchItemConfig.ItemType.More,
            Key = new ()
            {
                "下",
                "载",
                "下载",
                "安装",
                "资源",
                "核心",
                "本体",
                "游戏",
                "实例",
                "页",
                "页面",
                
            },
            Action = (key) =>
            {
                MainSearchBox.Show();
                Task.Run(() =>
                {
                    Thread.Sleep(800);
                    Dispatcher.UIThread.Invoke(() => SystemNavigationBar.NavTo("Download"));
                });
            }
        });*/

        BottomBar.ContentFrame = MainContent;
        BottomBar.RegisterNavigationItem(new BottomBarNavigationEntry()
        {
            Title = new Label(){Content = "主页"},
            Page = Core.MainHome,
            Tag = "Launch",
            IsDefault = true,
            IsNoButton = true
        });
        BottomBar.RegisterNavigationItem(new BottomBarNavigationEntry()
        {
            Title = new Label(){Content = "下载"},
            Page = Core.DownloadPage,
        });
        BottomBar.RegisterNavigationItem(new BottomBarNavigationEntry()
        {
            Title = new Label(){Content = "管理"},
            Page = Core.MangePage,
        });
        BottomBar.RegisterNavigationItem(new BottomBarNavigationEntry()
        {
            Title = new Label(){Content = "设置"},
            Page = Core.SettingPage,
        });
        
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var Sel = Config.MainConfig.SelectedGameFolder;
                    try
                    {
                        SmTitle.Content = $"{Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex])}";
                    }catch{ }
                });
                Thread.Sleep(100);
            }
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
    
    public void laun()
    {
        var Sel = Config.MainConfig.SelectedGameFolder;
        var dow = new LaunchJavaEdtion();
        dow.Version = Path.GetFileName(Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex]));
        dow.Tuid = SystemMessageTaskMange.AddTask(dow);
        dow.Launch();
    }
    private void LaunchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        laun();
    }

}
