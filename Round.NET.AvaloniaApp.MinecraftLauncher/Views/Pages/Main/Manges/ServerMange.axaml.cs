using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.VersionServerMange.Library.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class ServerMange : UserControl
{
    public ServerMange()
    {
        InitializeComponent();
        this.Loaded += (sender, args) => RefreshUI();
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
    public void RefreshUI()
    {
        var serverMange = new ServerMangeCore();
        serverMange.Path = @"D:\Games\.minecraft\versions\1.14.3\servers.dat";
        serverMange.Load();
        ServerBox.Children.Clear();
        foreach (var ser in serverMange.Servers)
        {
            var labms = new Label()
            {
                Content = $"??? ms",
                VerticalAlignment = VerticalAlignment.Center
            };
            Task.Run(() =>
            {
                try
                {
                    var ms = new Ping().SendPingAsync(ser.IP).Result.RoundtripTime;
                    var color = Brushes.Green;
                    var mstext = $"{ms} ms";
                    if (ms > 200)
                    {
                        color = Brushes.Orange;
                    }else if (ms == 0)
                    {
                        color = Brushes.Red;
                        mstext = $"(也许是...本地服务器?) 无连接";
                    }
                    ControlChange.ChangeLabelText(labms,mstext,color);
                }
                catch
                {
                    ControlChange.ChangeLabelText(labms,$"无连接",Brushes.Red);
                }
                // Dispatcher.UIThread.Invoke(() => labms.Content = $"{ms} ms");
            });
            ServerBox.Children.Add(new ListBoxItem()
            {
                Content = new Grid()
                {
                    Height = 64,
                    Children =
                    {
                        new Image()
                        {
                            Source = DisplayBase64Image(ser.Icon),
                            Height = 52,
                            Width = 52,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(-10,5,5,5),
                        },
                        new Grid()
                        {
                            Margin = new Thickness(45,0,0,0),
                            Height = 64,
                            Children =
                            {
                                new Label()
                                {
                                    Content = ser.Name,
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    VerticalContentAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(5),
                                    FontSize = 22
                                },
                                new Label()
                                {
                                    Content = $"服务器地址：{ser.IP}",
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
                                        labms,
                                        new Button()
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
                                        }
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
}