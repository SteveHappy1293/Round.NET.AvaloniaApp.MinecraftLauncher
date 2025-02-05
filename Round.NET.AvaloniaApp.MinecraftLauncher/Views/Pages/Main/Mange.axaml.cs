using System;
using System.Diagnostics;
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
    bool IsEdit = false;
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
                            UpdateVersionList();
                            
                            VersionBox.SelectedIndex = Config.MainConfig.GameFolders[GameDirBox.SelectedIndex]
                                .SelectedGameIndex;
                        });
                        count = Directory.GetDirectories(path).Length;
                    }
                }
                catch (Exception ex)
                {
                    Directory.CreateDirectory(path);
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        IsEdit = false;
                        GameDirBox.SelectedIndex = 0;
                        Config.MainConfig.SelectedGameFolder = 0;
                        count = 0;
                        VersionBox.Items.Clear();
                        UpdateVersionList();
                        
                        IsEdit = true;
                    });
                }
                Thread.Sleep(100);
            }
        });
        
        IsEdit = true;
    }

    private void GameDirBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit) // 确保是用户主动选择，而不是程序初始化
        {
            IsEdit = false;
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                int newIndex = comboBox.SelectedIndex;
                if (newIndex >= 0 && newIndex < Config.MainConfig.GameFolders.Count)
                {
                    // 更新选中的游戏文件夹索引
                    Config.MainConfig.SelectedGameFolder = newIndex;

                    // 更新版本列表
                    UpdateVersionList();

                    // 保存配置文件
                    Config.SaveConfig();
                }
            }
            IsEdit = true;
        }
    }
    private void UpdateVersionList()
    {
        Dispatcher.UIThread.Invoke(() =>
        {

            var selectedFolder = Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder];
            var versionsPath = Path.Combine(selectedFolder.Path, "versions");

            if (Directory.Exists(versionsPath))
            {
                IsEdit = false;
                VersionBox.Items.Clear(); // 清空当前版本列表
                foreach (var ver in Directory.GetDirectories(versionsPath))
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

                // 设置默认选中项
                VersionBox.SelectedIndex = selectedFolder.SelectedGameIndex;
                IsEdit = true;
            }
        });
    }
    private void VersionBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.GameFolders[GameDirBox.SelectedIndex].SelectedGameIndex = VersionBox.SelectedIndex;
            Config.SaveConfig();
        }
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
                // 添加新文件夹到配置
                Config.MainConfig.GameFolders.Add(new()
                {
                    Path = result,
                    Name = TextB.Text,
                    SelectedGameIndex = 0
                });

                // 更新 GameDirBox 的显示内容
                GameDirBox.Items.Clear();
                foreach (var dir in Config.MainConfig.GameFolders)
                {
                    GameDirBox.Items.Add($"[{dir.Name}] {dir.Path}");
                }
                GameDirBox.SelectedIndex = Config.MainConfig.SelectedGameFolder;

                // 保存配置文件
                Config.SaveConfig();
            };

            con.PrimaryButtonClick += (_, __) =>
            {
                // 如果用户点击取消，不保存配置
                con.Hide();
            };

            await con.ShowAsync(Core.MainWindow);
        }
    }

    private void OpenDirButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var path = $"{Config.MainConfig.GameFolders[GameDirBox.SelectedIndex].Path}";
        
        var startInfo = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true // 允许操作系统处理文件夹路径
        };

        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Modules.Message.Message.Show("版本管理",$"打开文件夹失败: {ex.Message}",InfoBarSeverity.Error);
        }
    }
}