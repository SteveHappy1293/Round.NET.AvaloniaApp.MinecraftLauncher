using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;
using RMCL.Controls.ControlHelper;
using RMCL.Controls.Item;
using RMCL.Controls.View;
using RMCL.Core.Models.Classes;
using RMCL.Core.Views.Pages.Main.DownloadPages.DownloadAssets.DownloadAssetsSubPages;

namespace RMCL.Core.Views.Pages.Main.DownloadPages.DownloadAssets;

public partial class DownloadAssetsDetailsPage : UserControl
{
    private ModInfo _modInfo;
    public DownloadAssetsDetailsPage(ModInfo info)
    {
        _modInfo = info;
        InitializeComponent();
        
        info.Screenshots. ForEach(x =>
        {
            ScreenshotPanel.Children.Add(new WebImage() { ImageUrl = x.Url, Height = 200 });
        });
        
        var iconUrl = info.Logo.Url;
        ProfileBox.Text = info.Summary;
        NameBox.Text = info.Name;

        LabelsBox.Children.Add(new LabelBox() { Text = $"下载量：{info.DownloadCount}" });
        info.Categories.ForEach(x =>
        {
            LabelsBox.Children.Add(new LabelBox() { Text = x.Name });
        });
        
        Task.Run(() =>
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // 下载图片数据
                    var response = httpClient.GetAsync(iconUrl).Result;
                    response.EnsureSuccessStatusCode();

                    // 先读取所有数据到内存中
                    var imageData = response.Content.ReadAsByteArrayAsync().Result;
            
                    // 第一个流 - 用于 IconBox
                    using (var stream1 = new MemoryStream(imageData))
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            var bitmap = Bitmap.DecodeToWidth(stream1, 100);
                            IconBox.Background = new ImageBrush()
                            {
                                Source = bitmap,
                                Stretch = Stretch.UniformToFill
                            };
                            IconBox.Child = null;
                        });
                    }

                    // 第二个流 - 用于 BackBox
                    using (var stream2 = new MemoryStream(imageData))
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            BackBox.Source = new Bitmap(stream2);
                            BackBox.Effect = new BlurEffect
                            {
                                Radius = 200 // 模糊半径，值越大越模糊
                            };
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理异常，例如显示默认图片或错误消息
                Console.WriteLine($"加载图片失败: {ex.Message}");
            }
        });
    }

    private void GetAssets_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }

    private void OpenWebURL_OnClick(object? sender, RoutedEventArgs e)
    {
        SystemHelper.SystemHelper.OpenUrl(_modInfo.Links.WebsiteUrl);
    }
}