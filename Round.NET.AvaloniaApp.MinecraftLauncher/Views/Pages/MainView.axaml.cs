using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download;
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
        
        Core.API.RegisterSearchItem(new Core.API.SearchItemConfig()
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
        });
        
        Task.Run(() =>
        {
            var downloader = new Updater((v,s) =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
        
                // 获取程序集的版本信息
                Version version = assembly.GetName().Version;
                if (v.Replace("v", "").Replace("0","").Replace(".","") != version.ToString().Replace(".","").Replace("0",""))
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var con = new ContentDialog()
                        {
                            PrimaryButtonText = "取消",
                            CloseButtonText = "现在更新",
                            Title = $"更新 RMCL3 - {v.Replace("0","")}",
                            DefaultButton = ContentDialogButton.Close,
                            Content = new StackPanel()
                            {
                                Children =
                                {
                                    new Label()
                                    {
                                        Content = "你好！打扰一下~\nRMCL当前有个更新，需要花费您一些时间，请问您是否更新？"
                                    },
                                    new Label()
                                    {
                                        Content = $"当前版本：v{version.ToString().Replace(".","").Replace("0","")}"
                                    },
                                    new Label()
                                    {
                                        Content = $"更新版本：{v.Replace(".","").Replace("0","")}"
                                    }
                                }
                            }
                        };
                        con.CloseButtonClick += (_, __) =>
                        {
                            var dow = new DownloadUpdate();
                            dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                            dow.URL = s;
                            dow.Version = v.Replace(".","").Replace("0","");
                            dow.Download();
                        };
                        con.ShowAsync();
                    });
                }
            });
            downloader.GetDownloadUrlAsync(
                "https://api.github.com/repos/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/releases");
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
