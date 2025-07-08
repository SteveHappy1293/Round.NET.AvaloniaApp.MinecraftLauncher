using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace RMCL.Controls.View;

public partial class MCSkinViewer2D : UserControl
{
    public MCSkinViewer2D()
    {
        InitializeComponent();
    }
    
    public void ShowSkin(string Base64)
    {
        try
        {
            var skin = SkinHelper.Base64ToBitmap(Base64);
            Bitmap leftarm = null;
            Bitmap rightarm = null;
            Bitmap leftfoot = null;
            Bitmap rightfoot = null;
            Bitmap body = null;
            Bitmap header = null;

            try { leftarm = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(44, 20, 4, 12), 8); } catch { }
            try { rightarm = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(36, 52, 4, 12), 8); } catch { }
            try { leftfoot = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(4, 20, 4, 12), 8); } catch { }
            try { rightfoot = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(20, 52, 4, 12), 8); } catch { }
            try { body = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(20, 20, 8, 12), 8); } catch { }
            try { header = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(8, 8, 8, 8), 8); } catch { }

            try { LeftArm.Background = new ImageBrush() { Source = leftarm }; } catch { }
            try { LeftFoot.Background = new ImageBrush() { Source = leftfoot }; } catch { }
            try { RightArm.Background = new ImageBrush() { Source = rightarm }; } catch { }
            try { RightFoot.Background = new ImageBrush() { Source = rightfoot }; } catch { }
            try { Body.Background = new ImageBrush() { Source = body }; } catch { }
            try { Header.Background = new ImageBrush() { Source = header }; } catch { }
        }
        catch { }
    }
}