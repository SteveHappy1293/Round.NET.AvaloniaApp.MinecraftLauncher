using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.NetWork.Lssues;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.ExceptionMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Tips;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe;

public partial class LssuesPage : UserControl
{
    public LssuesPage()
    {
        InitializeComponent();
        
        this.HomeIcon1.Click = (sender, args) =>
        {
            ((MainView)Core.MainWindow.Content).CortentFrame.Opacity = 0;
            ((MainView)Core.MainWindow.Content).CortentFrame.Content = new Grid();
            ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };
        
        this.Loaded += ((sender, args) => Load());
    }

    public void Load()
    {
        if (LssuesCore.Lssues.Count == 0)
        {
            Task.Run(() =>
            {
                LssuesCore.Load();
                Dispatcher.UIThread.Invoke(() =>
                {
                    reLoad();
                });
            });
        }
        else
        {
            reLoad();
        }
    }

    public void reLoad()
    {
        LssuesAnimatedStackPanel.Children.Clear();
        LoadingControl.IsVisible = true;
        NullControl.IsVisible = false;
        LssuesCore.Lssues.ForEach(x =>
        {
            var doc = new DockPanel()
            {
                
            };
            x.labels.ForEach(la =>
            {
                doc.Children.Add(new LssuesTipBox()
                {
                    LssuesText = la.name,
                    LssuesType = la.name switch
                    {
                        "Bug" => LssuesTypeEnum.Bug,
                        "Feature" => LssuesTypeEnum.Feature,
                        "Default" => LssuesTypeEnum.Default,
                        "不错的第一个" => LssuesTypeEnum.不错的第一个,
                        "处理中" => LssuesTypeEnum.处理中,
                        "已完成" => LssuesTypeEnum.已完成,
                        "已放弃" => LssuesTypeEnum.已放弃,
                        "已查看" => LssuesTypeEnum.已查看,
                        "投票中" => LssuesTypeEnum.投票中,
                        "文档" => LssuesTypeEnum.文档,
                        "新功能" => LssuesTypeEnum.新功能,
                        "无效" => LssuesTypeEnum.无效,
                        "未查看" => LssuesTypeEnum.未查看,
                        "漏洞" => LssuesTypeEnum.漏洞,
                        "重复的" => LssuesTypeEnum.重复的,
                        "问题" => LssuesTypeEnum.问题,
                        "需要帮助" => LssuesTypeEnum.需要帮助,
                    },
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5,0,0,0)
                });
            });
            var liteam = new ListBoxItem()
            {
                Padding = new Thickness(15, 10),
                Content = new StackPanel()
                {
                    Children =
                    {
                        new DockPanel()
                        {
                            Children =
                            {
                                new TextBlock()
                                {
                                    Text = $"{x.title}",
                                    FontSize = 18,
                                    Margin = new Thickness(0,5),
                                    FontWeight = FontWeight.Bold
                                },
                                doc
                            },
                        },
                        new DockPanel()
                        {
                            Children =
                            {
                                new LssuesTipBox()
                                {
                                    LssuesType = x.type.name switch
                                    {
                                        "Bug" => LssuesTypeEnum.Bug,
                                        "Feature"=>LssuesTypeEnum.Feature,
                                        _ => LssuesTypeEnum.Default,
                                    } ,
                                    LssuesText = x.type.name,
                                    Height = 28,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                },
                                new TextBlock()
                                {
                                    Text = $"#{x.number} · {x.user.login} · 于 {x.created_at.ToShortDateString()} 打开",
                                    FontSize = 11,
                                    VerticalAlignment = VerticalAlignment.Center,
                                    Margin = new Thickness(5),
                                }
                            }
                        },
                    }
                }
            };
            LssuesAnimatedStackPanel.Children.Add(liteam);
        });
        if (LssuesCore.Lssues.Count == 0)
        {
            NullControl.IsVisible = true;
            LoadingControl.IsVisible = false;
        }
        LoadingControl.IsVisible = false;
    }
}