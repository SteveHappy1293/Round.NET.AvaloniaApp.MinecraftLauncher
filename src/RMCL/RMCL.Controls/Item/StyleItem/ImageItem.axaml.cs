using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace RMCL.Controls.Item.StyleItem;

public partial class ImageItem : UserControl
{
    public string path { get; set; }
    public Action<string> DeleteCallBack { get; set; } = s => { };
    public ImageItem(string Path)
    {
        InitializeComponent();
        path = Path;
        
        if (File.Exists(Path))
        {
            using (var stream = File.OpenRead(Path))
            {
                // 按宽度等比缩放解码（保持纵横比）
                var bitmap = Bitmap.DecodeToWidth(stream, 48);
                ImageShowBox.Background = new ImageBrush()
                {
                    Source = bitmap,
                    Stretch = Stretch.UniformToFill
                };
            }
        }
    }

    public string GetPath()
    {
        return path;
    }

    private void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        DeleteCallBack(path);
    }
}