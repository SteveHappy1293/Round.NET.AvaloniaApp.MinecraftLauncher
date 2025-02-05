using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Game.JavaEdtion;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Download : UserControl
{
    public Download()
    {
        InitializeComponent();
        RefreshList();
    }

    private ListBoxItem GetListBoxItem(string content,string time = null,string type = null)
    {
        var button = new HyperlinkButton()
        {
            Content = new HeroIcon()
            {
                Foreground = Brushes.White,
                Type = IconType.InformationCircle,
                Min = true,
                Width = 16,
                Height = 16
            },
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(5),
            VerticalAlignment = VerticalAlignment.Center,
            NavigateUri = new Uri($"https://zh.minecraft.wiki/w/Java版{content}")
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
                new ProgressRing(),
                new Label()
                {
                    Content = "Loading...",
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            }
        });
        Task.Run(() =>
        {
            var Versions = UpdateVersions.GetVersions();
            Dispatcher.UIThread.Invoke(() =>
            {
                MainPanel.Children.Clear();
                MainPanel.Children.Add(new Expander()
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
                    Margin = new Thickness(5,0,5,5)
                });

                var rel = new Expander()
                {
                    Header = "正式版",
                    Content = new ScrollViewer()
                    {
                        Height = 300,
                        Content = new StackPanel()
                    },
                    Margin = new Thickness(5)
                };

                var shot = new Expander()
                {
                    Header = "快照版",
                    Content = new ScrollViewer()
                    {
                        Height = 300,
                        Content = new StackPanel()
                    },
                    Margin = new Thickness(5)
                };

                var old = new Expander()
                {
                    Header = "远古版",
                    Content = new ScrollViewer()
                    {
                        Height = 300,
                        Content = new StackPanel()
                    },
                    Margin = new Thickness(5)
                };

                foreach (var version in Versions.Versions)
                {
                    if (version.Type == "release")
                    {
                        ((StackPanel)((ScrollViewer)rel.Content).Content).Children.Add(GetListBoxItem(version.Id,version.ReleaseTime.ToString(),"release"));
                    }
                    else if (version.Type == "snapshot")
                    {
                        ((StackPanel)((ScrollViewer)shot.Content).Content).Children.Add(GetListBoxItem(version.Id,version.ReleaseTime.ToString(),"snapshot"));
                    }
                    else
                    {
                        ((StackPanel)((ScrollViewer)old.Content).Content).Children.Add(GetListBoxItem(version.Id,version.ReleaseTime.ToString(),"old"));
                    }
                }

                MainPanel.Children.Add(rel);
                MainPanel.Children.Add(shot);
                MainPanel.Children.Add(old);
            });
        });
    }
}