using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.NetWork.Issues;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.ExceptionMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Tips;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe;

public partial class IssuesPage : UserControl
{
    public IssuesPage()
    {
        InitializeComponent();
        
        this.HomeIcon1.Click = (sender, args) =>
        {
            Core.MainWindow.MainView.ContentFrame.Opacity = 0;
            Core.MainWindow.MainView.ContentFrame.Content = new Grid();
            Core.MainWindow.MainView.MainContent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };
        
        this.Loaded += ((sender, args) => Load());
    }

    public void Load()
    {
        if (IssuesCore.Issues.Count == 0)
        {
            Task.Run(() =>
            {
                IssuesCore.Load();
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
        IssuesAnimatedStackPanel.Children.Clear();
        LoadingControl.IsVisible = true;
        NullControl.IsVisible = false;
        IssuesCore.Issues.ForEach(x =>
        {
            var doc = new DockPanel()
            {
                
            };
            x.labels.ForEach(la =>
            {
                doc.Children.Add(new IssuesTipBox()
                {
                    IssuesText = la.name,
                    IssuesType = la.name switch
                    {
                        "Bug" => IssuesTypeEnum.Bug,
                        "Feature" => IssuesTypeEnum.Feature,
                        "Default" => IssuesTypeEnum.Default,
                        "不错的第一个" => IssuesTypeEnum.不错的第一个,
                        "处理中" => IssuesTypeEnum.处理中,
                        "已完成" => IssuesTypeEnum.已完成,
                        "已放弃" => IssuesTypeEnum.已放弃,
                        "已查看" => IssuesTypeEnum.已查看,
                        "投票中" => IssuesTypeEnum.投票中,
                        "文档" => IssuesTypeEnum.文档,
                        "新功能" => IssuesTypeEnum.新功能,
                        "无效" => IssuesTypeEnum.无效,
                        "未查看" => IssuesTypeEnum.未查看,
                        "漏洞" => IssuesTypeEnum.漏洞,
                        "重复的" => IssuesTypeEnum.重复的,
                        "问题" => IssuesTypeEnum.问题,
                        "需要帮助" => IssuesTypeEnum.需要帮助,
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
                                new IssuesTipBox()
                                {
                                    IssuesType = x.type.name switch
                                    {
                                        "Bug" => IssuesTypeEnum.Bug,
                                        "Feature"=>IssuesTypeEnum.Feature,
                                        _ => IssuesTypeEnum.Default,
                                    } ,
                                    IssuesText = x.type.name,
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
            IssuesAnimatedStackPanel.Children.Add(liteam);
        });
        if (IssuesCore.Issues.Count == 0)
        {
            NullControl.IsVisible = true;
            LoadingControl.IsVisible = false;
        }
        LoadingControl.IsVisible = false;
    }
}