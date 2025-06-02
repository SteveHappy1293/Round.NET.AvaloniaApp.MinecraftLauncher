using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace RMCL.Controls.View;

public partial class WebImage : UserControl
{
    private string _imageUrl;
    public string ImageUrl
    {
        get
        {
            return _imageUrl;
        }
        set
        {
            _imageUrl = value;
            Update();
        }
    }

    public WebImage()
    {
        InitializeComponent();
    }

    public void Update()
    {
        Task.Run(() =>
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // 下载图片数据
                    var response = httpClient.GetAsync(_imageUrl).Result;
                    response.EnsureSuccessStatusCode();

                    // 将流转换为 Bitmap
                    using (var stream = response.Content.ReadAsStreamAsync())
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            ImageBox.Source = new Bitmap(stream.Result);
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