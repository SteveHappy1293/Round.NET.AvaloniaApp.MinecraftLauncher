using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Downloads;

public partial class DownloadGamePage : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{ControlHelper.CreateMenuItem("刷新",RefreshList)});
    }
    public DownloadGamePage()
    {
        InitializeComponent();
        RefreshList();
    }
    
    private ListBoxItem GetListBoxItem(string content,string time = null,string type = null)
    {
        var button = new HyperlinkButton()
        {
            Content = new FluentIcon()
            {
                Icon = FluentIconSymbol.Info20Regular,
                Width = 16,
                Height = 16
            },
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5),
            VerticalAlignment = VerticalAlignment.Center,
            NavigateUri = new Uri($"https://zh.minecraft.wiki/w/Java版{content}"),
            Width = 32,
            Height = 32
        };
        var result = new ListBoxItem();
        if (time != null)
        {
            result = new ListBoxItem()
            {
                Margin = new Thickness(10,5),
                Height = 64,
                Content = new Grid()
                {
                    Children =
                    {
                        new Label()
                        {
                            VerticalContentAlignment = VerticalAlignment.Top,
                            FontSize = 18,
                            Content = content
                        },
                        new Label()
                        {
                            Content = time,
                            FontSize = 11,
                            Foreground = Brushes.Gray,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                        },
                        button
                    } 
                }
            };
        }
        else
        {
            result = new ListBoxItem()
            {
                Margin = new Thickness(10,5),
                Content = new Grid()
                {
                    Children =
                    {
                        new Label()
                        {
                            VerticalContentAlignment = VerticalAlignment.Center,
                            Content = content
                        },
                        button
                    } 
                }
            };
        }

        result.PointerPressed += (s, e) => InstallVersion(content);
        return result;
    }
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshList();
    }
    private void InstallVersion(string version)
    {
        // ContentDialog ins = new ContentDialog()
        // {
        //     CloseButtonText = "取消",
        //     Title = $"安装 {version}",
        //     Content = new NewGame(version)
        // };
        // ins.ShowAsync(Core.MainWindow);
        MainFrame.Content = new NewGame(version);
        MainFrame.IsVisible = true;
        MainGrid.IsVisible = false;
    }
    private void RefreshList()
    {
        MainPanel.Children.Clear();
        MainPanel.Children.Add(new StackPanel()
        {
            Children =
            {
                new LoadingControl()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(80),
                }
            }
        });
        Task.Run(() =>
        {
            try
            {
                var Versions = UpdateVersions.GetVersions();
                Dispatcher.UIThread.Invoke(() =>
                {
                    var news = new Expander()
                    {
                        IsExpanded = true,
                        Header = "最新版本",
                        Content = new StackPanel()
                        {
                            Children =
                            {
                                GetListBoxItem(Versions.Latest.Release),
                                GetListBoxItem(Versions.Latest.Snapshot)
                            }
                        },
                        // Margin = new Thickness(5, 0, 5, 5),
                        Margin = new Thickness(50),
                        Opacity = 0,
                    };

                    var rel = new Expander()
                    {
                        Header = "正式版",
                        Content = new ScrollViewer()
                        {
                            Height = 300,
                            Content = new StackPanel()
                        },
                        Margin = new Thickness(50),
                        Opacity = 0,
                    };

                    var shot = new Expander()
                    {
                        Header = "快照版",
                        Content = new ScrollViewer()
                        {
                            Height = 300,
                            Content = new StackPanel()
                        },
                        Margin = new Thickness(50),
                        Opacity = 0,
                    };

                    var old = new Expander()
                    {
                        Header = "远古版",
                        Content = new ScrollViewer()
                        {
                            Height = 300,
                            Content = new StackPanel()
                        },
                        Margin = new Thickness(50),
                        Opacity = 0,
                    };

                    Task.Run(() =>
                    {
                        foreach (var version in Versions.Versions)
                        {
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                if (version.Type == "release")
                                {
                                    ((StackPanel)((ScrollViewer)rel.Content).Content).Children.Add(GetListBoxItem(
                                        version.Id,
                                        version.ReleaseTime.ToString(), "release"));
                                }
                                else if (version.Type == "snapshot")
                                {
                                    ((StackPanel)((ScrollViewer)shot.Content).Content).Children.Add(GetListBoxItem(
                                        version.Id,
                                        version.ReleaseTime.ToString(), "snapshot"));
                                }
                                else
                                {
                                    ((StackPanel)((ScrollViewer)old.Content).Content).Children.Add(GetListBoxItem(
                                        version.Id,
                                        version.ReleaseTime.ToString(), "old"));
                                }
                            });
                        }

                        Dispatcher.UIThread.Invoke(() => MainPanel.Children.Clear());

                        Dispatcher.UIThread.Invoke(() =>
                        {
                            MainPanel.Children.Add(news);
                            news.Margin = new Thickness(5, 0, 5, 5);
                            news.Opacity = 1;
                        });
                        Thread.Sleep(200);
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            MainPanel.Children.Add(rel);
                            rel.Margin = new Thickness(5);
                            rel.Opacity = 1;
                        });
                        Thread.Sleep(200);
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            MainPanel.Children.Add(shot);
                            shot.Margin = new Thickness(5);
                            shot.Opacity = 1;
                        });
                        Thread.Sleep(200);
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            MainPanel.Children.Add(old);
                            old.Margin = new Thickness(5);
                            old.Opacity = 1;
                        });
                    });
                });
            }
            catch (Exception e)
            {
                Modules.Message.Message.Show("版本下载",$"加载版本出错：{e.Message}",InfoBarSeverity.Error);
                Dispatcher.UIThread.Invoke(() =>
                {
                    MainPanel.Children.Clear();
                    MainPanel.Children.Add(new StackPanel()
                    {
                        Children =
                        {
                            new NullControl()
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Margin = new Thickness(80),
                            }
                        }
                    });
                });
            }
        });
    }
}