using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Manges;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class PlugMange : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{ControlHelper.CreateMenuItem("添加插件",(() => AddPlug_OnClick(null,null)))});
    }
    public PlugMange()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            PlugsBox.Items.Clear();
            foreach (var pl in PlugLoaderNeo.Plugs)
            {
                var sets = new Button()
                {
                    Content = new FluentIcon()
                    {
                        Icon = FluentIconSymbol.Settings20Regular
                    },
                    Margin = new Thickness(5),
                    Height = 32,
                    Width = 32
                };
                sets.Click += (_, __) =>
                {
                    /*var con = new ContentDialog()
                    {
                        PrimaryButtonText = "取消",
                        CloseButtonText = "确定",
                        DefaultButton = ContentDialogButton.Close,
                        Title = $"插件 {pl.Name} 设置",
                        Content = new SettingPlug(pl)
                    };
                    con.ShowAsync(Core.MainWindow);*/
                };
                PlugsBox.Items.Add(new ListBoxItem()
                {
                    Padding = new Thickness(5),
                    Content = new DockPanel()
                    {
                        Margin = new Thickness(5,0,0,0),
                        Children =
                        {
                            new DockPanel()
                            {
                                Children =
                                {
                                    new Border()
                                    {
                                        Background = new ImageBrush()
                                        {
                                            Source = pl.Icon
                                        },
                                        Width = 40,
                                        Height = 40,
                                        CornerRadius = new CornerRadius(6),
                                    },
                                    new StackPanel()
                                    {
                                        Margin = new Thickness(10,0),
                                        Children =
                                        {
                                            new Label()
                                            {
                                                Content = pl.Name,
                                                FontSize = 22,
                                                Margin = new Thickness(0,-3,0,0),
                                            },
                                            new Label()
                                            {
                                                Content = pl.Notes,
                                                FontSize = 15,
                                                FontStyle = FontStyle.Italic,
                                                Foreground = Brushes.DimGray,
                                                Margin = new Thickness(0,-10,0,0),
                                            },
                                        }
                                    }
                                }
                            },
                            new DockPanel()
                            {
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Center,
                                Children =
                                {
                                    sets
                                }
                            }
                        }
                    },
                });
            }
        };
    }

    private void AddPlug_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter()
                {
                    Name = "RMCL 插件包文件",
                    Extensions = new List<string>(){"rplk"}
                }
            }
        };

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Task.Run(() =>
            {
                string[] fileNames = openFileDialog.ShowAsync(desktop.MainWindow).Result;
                if (fileNames != null && fileNames.Length > 0)
                {
                    string fileName = fileNames[0];
                    try
                    {
                        RLogs.WriteLog(Path.Combine(PlugLoaderNeo.PlugMainDirPath,
                            $"Neo\\PlugPacks\\{Path.GetFileName(fileName)}"));
                        File.Copy(fileName,
                            Path.Combine(PlugLoaderNeo.PlugMainDirPath,
                                $"Neo\\PlugPacks\\{Path.GetFileName(fileName)}"));
                    }
                    catch (Exception ex)
                    {
                        Message.Show("插件管理", $"载入插件包识别！\n{ex.Message}", InfoBarSeverity.Error);
                    }
                }
            });
        }
    }
}