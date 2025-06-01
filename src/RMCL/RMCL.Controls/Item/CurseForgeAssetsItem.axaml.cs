using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;

namespace RMCL.Controls.Item;

public partial class CurseForgeAssetsItem : UserControl
{
    public CurseForgeAssetsItem(ModInfo Info)
    {
        InitializeComponent();

        AssetsName.Text = Info.Name;
        AssetsProfile.Text = Info.Summary;
        AssetsCount.Text = $"下载量: {Info.DownloadCount}";
        if (Info.Logo != null)
        {
            var iconUrl = Info.Logo.Url;

            Task.Run(() =>
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        // 下载图片数据
                        var response = httpClient.GetAsync(iconUrl).Result;
                        response.EnsureSuccessStatusCode();

                        // 将流转换为 Bitmap
                        using (var stream = response.Content.ReadAsStreamAsync())
                        {
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                var bitmap = Bitmap.DecodeToWidth(stream.Result, 56);
                                AssetsIcon.Background = new ImageBrush()
                                {
                                    Source = bitmap,
                                    Stretch = Stretch.UniformToFill
                                };
                                AssetsIcon.Child = null;
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
    }
}