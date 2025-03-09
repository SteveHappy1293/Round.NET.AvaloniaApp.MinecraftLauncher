using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
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
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Dialog;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.ServerManges;
using Round.NET.VersionServerMange.Library.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class ServerMange : UserControl
{
    public ServerMange()
    {
        InitializeComponent();
        this.Loaded += (sender, args) => RefreshUI();
        Task.Run(() =>
        {
            var se = -1;
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var Sel = Config.MainConfig.SelectedGameFolder;
                    if (se != Sel)
                    {
                        try
                        {
                            var version =
                                $"{Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex])}";
                            var path = $"{Config.MainConfig.GameFolders[Sel].Path}/versions/{version}";
                            var res = Modules.Server.ServerMange.ScannedVersion(path);
                            if (res != 0)
                            {
                                Message.Show("服务器管理",$"已从 {version} 中添加 {res} 项服务器!",InfoBarSeverity.Success);
                            }
                            se = Sel;
                        }catch{ }
                    }
                });
                Thread.Sleep(500);
            }
        });
    }
    private IImage DisplayBase64Image(string base64String)
    {
        try
        {
            // 将 Base64 字符串转换为字节数组
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // 将字节数组加载为 Bitmap
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                var bitmap = new Bitmap(memoryStream);

                // 将 Bitmap 设置为 Image 控件的 Source
                return bitmap;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading image: " + ex.Message);
        }
        return null;
    }
    public async void RefreshUI()
    {
        ServerBox.Children.Clear();

        foreach (var ser in Modules.Server.ServerMange.Servers)
        {
            // 解析服务器地址和端口
            var serverAddress = ser.IP.Split(':')[0];
            var serverPort = ser.IP.Contains(':') ? int.Parse(ser.IP.Split(':')[1]) : 25565;

            // 创建 UI 元素
            var labms = new Label()
            {
                Content = "检测中...",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            var labplayer = new Label()
            {
                Content = "检测中...",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Foreground = new SolidColorBrush(Colors.Gray),
            };
            var serset = new Button()
            {
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.Cog8Tooth,
                    Min = true
                },
                Margin = new Thickness(5),
                Height = 32,
                Width = 32
            };
            var upbtn = new Button()
            {
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.ArrowUp,
                    Min = true,
                    Width = 15,
                    Height = 15
                },
                IsEnabled = (Modules.Server.ServerMange.Servers.FindIndex(x=>x.SUID == ser.SUID)!=0),
                Margin = new Thickness(5),
                Height = 32,
                Width = 32
            };
            var downbtn = new Button()
            {
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.ArrowDown,
                    Min = true,
                    Width = 15,
                    Height = 15
                },
                Margin = new Thickness(5),
                IsEnabled = (Modules.Server.ServerMange.Servers.FindIndex(x=>x.SUID == ser.SUID)!=Modules.Server.ServerMange.Servers.Count-1),
                Height = 32,
                Width = 32
            };
            var launchbtn = new Button()
            {
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.RocketLaunch,
                    Min = true,
                    Width = 15,
                    Height = 15
                },
                Margin = new Thickness(5),
                Height = 32,
                Width = 32
            };

            launchbtn.Click += (s, e) =>
            {
                var ls = new LaunchServer();
                var con = new ContentDialog()
                {
                    Content = ls,
                    PrimaryButtonText = "取消",
                    CloseButtonText = "启动",
                    DefaultButton = ContentDialogButton.Close,
                    Title = $"启动 - {ser.IP}"
                };
                con.CloseButtonClick += (s, e) =>
                {
                    var dow = new LaunchJavaEdtion();
                    dow.Version = Path.GetFileName(Directory.GetDirectories(Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder].Path+"/versions")[ls.VersionsBox.SelectedIndex]);
                    dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                    dow.Server = ser.IP;
                    dow.Launch();
                };
                con.ShowAsync();
            };
            upbtn.Click += (sender, args) =>
            {
                Modules.Server.ServerMange.UpServer(ser.SUID);
                RefreshUI();
            };
            downbtn.Click += (sender, args) => 
            {
                Modules.Server.ServerMange.DownServer(ser.SUID);
                RefreshUI();
            };
            
            serset.Click += (_, __) =>
            {
                ContentPageDialog dia = new ContentPageDialog()
                {
                    Title = $"{ser.IP} - {ser.Name}",
                    Page = new ServerSetting(ser),
                };
                dia.Show();
            };
            var iconimage = new Image()
            {
                Source = DisplayBase64Image(ser.Icon),
                Height = 52,
                Width = 52,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(-10, 5, 5, 5),
            };
            var ring = new ProgressRing()
            {
                Height = 10,
                Width = 10,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(-11, 0, 0, 6),
            };
            Task.Run(async () =>
            {
                try
                {
                    var ms = new Ping().SendPingAsync(serverAddress).Result.RoundtripTime;
                    var color = Brushes.Green;
                    var mstext = $"{ms} ms";
                    if (ms > 200)
                    {
                        color = Brushes.Orange;
                    }
                    else if (ms == 0)
                    {
                        color = Brushes.Red;
                        mstext = $"(也许是...本地服务器?) 无连接";
                    }
                    ControlChange.ChangeLabelText(labms, mstext, color);
                }
                catch
                {
                    ControlChange.ChangeLabelText(labms, "无连接", Brushes.Red);
                }
            }); // 加载延迟
            Task.Run(async () =>
            {
                // 创建 MinecraftServerChecker 实例
                var checker = new MinecraftServerChecker(serverAddress, serverPort);

                // 获取服务器信息
                var serverInfo = await checker.GetServerInfo();
                try
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        try
                        {
                            iconimage.Source = serverInfo.Icon;
                            Modules.Server.ServerMange.SetServerIcon(ser.SUID, serverInfo.IconBase64);
                        }
                        catch
                        {
                            iconimage.Source = DisplayBase64Image(ser.Icon);
                            Modules.Server.ServerMange.SetServerIcon(ser.SUID, ser.Icon);
                        }
                    });
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        try
                        {
                            ControlChange.ChangeLabelText(labplayer, $"{serverInfo.MaxPlayers}/{serverInfo.OnlinePlayers}", Brushes.Gray);
                        }
                        catch
                        {
                            ControlChange.ChangeLabelText(labplayer, "无连接", Brushes.Red);
                        }
                    });
                    Dispatcher.UIThread.Invoke(() => ring.IsVisible = false);
                }catch{ }
            }); // 添加服务器信息到 UI
            ServerBox.Children.Add(new ListBoxItem()
            {
                Content = new Grid()
                {
                    Height = 64,
                    Children =
                    {
                        iconimage,
                        ring,
                        new Grid()
                        {
                            Margin = new Thickness(45, 0, 0, 0),
                            Height = 64,
                            Children =
                            {
                                new Label()
                                {
                                    Content = ser.Name, // 使用服务器描述或名称
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    VerticalContentAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(5),
                                    FontSize = 22
                                },
                                new Label()
                                {
                                    Content = $"{ser.IP}",
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
                                        new StackPanel()
                                        {
                                            Children =
                                            {
                                                labms,
                                                labplayer
                                            }
                                        },
                                        upbtn,
                                        downbtn,
                                        launchbtn,
                                        serset,
                                    }
                                }
                            }
                        }
                    }
                },
            });
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshUI();
    }

    private void AddServerBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var addcon = new AddServer();
        var con = new ContentDialog()
        {
            Content = addcon,
            Title = $"添加服务器",
            PrimaryButtonText = "取消",
            CloseButtonText = "确定",
            DefaultButton = ContentDialogButton.Close
        };
        con.CloseButtonClick += (_, __) =>
        {
            addcon.Add();
            RefreshUI();
        };
        con.ShowAsync();
    }
}