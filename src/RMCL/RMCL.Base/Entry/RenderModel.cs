using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace RMCL.Base.Entry;

public class RenderModel
{
    public TextRenderingMode TextRenderingMode { get; set; } = TextRenderingMode.SubpixelAntialias;
    public BitmapInterpolationMode BitmapInterpolationMode { get; set; } = BitmapInterpolationMode.MediumQuality;
    public EdgeMode EdgeMode { get; set; } = EdgeMode.Antialias;
}