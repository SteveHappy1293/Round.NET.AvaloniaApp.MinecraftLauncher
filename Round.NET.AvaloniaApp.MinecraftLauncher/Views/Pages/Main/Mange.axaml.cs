using System;
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
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using LaunchJavaEdtion = Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch.LaunchJavaEdtion;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Mange : UserControl
{
    public Mange()
    {
        InitializeComponent();
        foreach (var dir in Config.MainConfig.GameFolders)
        {
            GameDirBox.Items.Add($"[{dir.Name}] {dir.Path}");
        }
        GameDirBox.SelectedIndex = Config.MainConfig.SelectedGameFolder;

        Task.Run(() =>
        {
            int count = 0;
            while (true)
            {
                // Dispatcher.UIThread.Invoke(() => Modules.Message.Message.Show("Hello World!", "Title", InfoBarSeverity.Success));
                var path = $"{Config.MainConfig.GameFolders[GameDirBox.SelectedIndex].Path}/versions";
                try
                {
                    if (Directory.GetDirectories(path).Length != count)
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            VersionBox.Items.Clear();
                            foreach (var ver in Directory.GetDirectories(path))
                            {
                                var launc = new Button()
                                {
                                    Content = new HeroIcon()
                                    {
                                        Foreground = Brushes.White,
                                        Type = IconType.RocketLaunch,
                                        Min = true
                                    },
                                    Margin = new Thickness(5)
                                };
                                launc.Click += (_, __) =>
                                {
                                    var dow = new LaunchJavaEdtion();
                                    dow.Version = Path.GetFileName(ver);
                                    SystemMessageTaskMange.AddTask(dow, SystemMessageTaskMange.TaskType.Launch);
                                };
                                VersionBox.Items.Add(new ListBoxItem()
                                {
                                    Content = new Grid()
                                    {
                                        Height = 65,
                                        Children =
                                        {
                                            new Label()
                                            {
                                                Content = Path.GetFileName(ver),
                                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                                VerticalContentAlignment = VerticalAlignment.Top,
                                                Margin = new Thickness(5),
                                                FontSize = 22
                                            },
                                            new Label()
                                            {
                                                Content = "无描述文件...",
                                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                                VerticalContentAlignment = VerticalAlignment.Bottom,
                                                Margin = new Thickness(5),
                                                FontSize = 15,
                                                FontStyle = FontStyle.Italic,
                                                Foreground = Brushes.DimGray,
                                            },
                                            new DockPanel()
                                            {

                                                HorizontalAlignment = HorizontalAlignment.Right,
                                                VerticalAlignment = VerticalAlignment.Center,
                                                Children =
                                                {
                                                    new Button()
                                                    {
                                                        Content = new HeroIcon()
                                                        {
                                                            Foreground = Brushes.White,
                                                            Type = IconType.Cog8Tooth,
                                                            Min = true
                                                        },
                                                        Margin = new Thickness(5)
                                                    },
                                                    launc
                                                }
                                            }
                                        }
                                    },
                                });
                            }

                            VersionBox.SelectedIndex = Config.MainConfig.GameFolders[GameDirBox.SelectedIndex]
                                .SelectedGameIndex;
                        });
                        count = Directory.GetDirectories(path).Length;
                    }
                }
                catch (Exception ex)
                {
                    Modules.Message.Message.Show("版本管理","此文件夹无效！",InfoBarSeverity.Error);
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        GameDirBox.SelectedIndex = 0;
                        Config.MainConfig.SelectedGameFolder = 0;
                        count = 0;
                        VersionBox.Items.Clear();
                    });
                }
                Thread.Sleep(100);
            }
        });
    }

    private void GameDirBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GameDirBox.SelectedIndex = (sender as ComboBox).SelectedIndex;
        Config.MainConfig.SelectedGameFolder = GameDirBox.SelectedIndex;
        Config.SaveConfig();
    }

    private void VersionBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Config.MainConfig.GameFolders[GameDirBox.SelectedIndex].SelectedGameIndex = VersionBox.SelectedIndex;
        Config.SaveConfig();
    }

    private async void AddDitButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog();
        dialog.Title = "选择 Minecraft 文件夹";
        var result = await dialog.ShowAsync(Core.MainWindow);
        Console.WriteLine(result);
        if (result != string.Empty)
        {
            var TextB = new TextBox()
            {
                Height = 32,
                Width = 120,
                Text = "新的文件夹"
            };
            var con = new ContentDialog()
            {
                Title = "添加 Minecraft 文件夹",
                PrimaryButtonText = "取消",
                CloseButtonText = "确定",
                DefaultButton = ContentDialogButton.Close,
                Content = new DockPanel()
                {
                    Children =
                    {
                        new Label()
                        {
                            Content = "文件夹名称",
                            VerticalContentAlignment = VerticalAlignment.Center
                        },
                        TextB
                    }
                }
            };
            con.CloseButtonClick += (_, __) =>
            {
                Config.MainConfig.GameFolders.Add(new()
                {
                    Path = result,
                    Name = TextB.Text,
                    SelectedGameIndex = 0
                });
                
                GameDirBox.Items.Clear();
                foreach (var dir in Config.MainConfig.GameFolders)
                {
                    GameDirBox.Items.Add($"[{dir.Name}] {dir.Path}");
                }
                GameDirBox.SelectedIndex = Config.MainConfig.SelectedGameFolder;
            };
            con.ShowAsync(Core.MainWindow);
            Config.SaveConfig();
        }
    }
}