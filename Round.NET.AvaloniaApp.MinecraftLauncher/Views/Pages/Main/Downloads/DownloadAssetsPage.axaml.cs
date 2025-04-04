using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls; 
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Assets;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Downloads;

public partial class DownloadAssetsPage : UserControl
{
    public DownloadAssetsPage()
    {
        InitializeComponent();
    }
    public async Task<Stream?> DownloadImageAsync(string imageUrl)
    {
        HttpClient HttpClient = new();
        try
        {
            // 下载图片数据
            var imageData = await HttpClient.GetByteArrayAsync(imageUrl);
            // 将字节数组转换为内存流
            return new MemoryStream(imageData);
        }
        catch (Exception ex)
        {
            // 处理异常，例如网络错误或无效的URL
            RLogs.WriteLog($"Error downloading image: {ex.Message}");
            return null;
        }
    }
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        AssetsBox.Children.Clear();
        AssetsBox.Children.Add(new LoadingControl()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(50)
        });
        var key = KeyBox.Text;
        bool Clear = false;

        Task.Run(async () =>
        {
            foreach (var re in await FindAssets.GetFindAssets(key))
            {
                
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (!Clear)
                    {
                        Clear = true;
                        AssetsBox.Children.Clear();
                    }
                    var image =new Image()
                    { 
                        Margin = new Thickness(-10,5),  
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    var ring = new ProgressRing()
                    {
                        Margin = new Thickness( 3),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    Task.Run(async () =>
                    {
                        var imageStream = await DownloadImageAsync(re.IconUrl);
                        if (imageStream != null)
                        {
                            //将图片流加载为Bitmap
                            var bitmap = new Bitmap(imageStream);
        
                            //在UI线程中更新Image控件
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                image.Source = bitmap;
                                ring.IsVisible = false;
                            });
                        }
                    });
                    AssetsBox.Children.Add(new ListBoxItem()
                    {
                        Content = new Grid()
                        {
                            Height = 65,
                            Children =
                            {
                                image,
                                ring,
                                new Label()
                                {
                                    Content = re.Name,
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    VerticalContentAlignment = VerticalAlignment.Top,
                                    Margin = new Thickness(50,5),
                                    FontSize = 22
                                },
                                new Label()
                                {
                                    Content = re.Summary,
                                    HorizontalContentAlignment = HorizontalAlignment.Left,
                                    VerticalContentAlignment = VerticalAlignment.Bottom,
                                    Margin = new Thickness(50,5),
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
                                            Content = new FluentIcon()
                                            {
                                                Icon = FluentIconSymbol.Settings20Regular
                                            },
                                            Margin = new Thickness(5),
                                            Height = 32,
                                            Width = 32
                                        }
                                    }
                                }
                            }
                        },
                    });
                });
            }
        });
    }
}