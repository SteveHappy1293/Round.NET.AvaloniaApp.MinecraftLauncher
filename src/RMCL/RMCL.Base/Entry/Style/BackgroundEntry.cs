using RMCL.Base.Enum;

namespace RMCL.Base.Entry.Style;

public class BackgroundEntry
{
    public BackgroundModelEnum ChooseModel { get; set; } = BackgroundModelEnum.None;

    public object?[] Data { get; set; } = new object?[7]
        { null, null, null, new ColorGlassEntry(), new ImageEntry(), new ColorEntry(), new PackEntry() };
}

public class ColorGlassEntry
{
    public double Opacity { get; set; } = 0.7;
    public string HtmlColor { get; set; } = "#000000";
}

public class ColorEntry
{
    public string HtmlColor { get; set; } = "#161616";
}

public class ImageEntry
{
    public string ImagePath { get; set; } = String.Empty;
    public ImageFilledModel Model { get; set; } = ImageFilledModel.UniformToFill;
}

public class PackEntry
{
    public string StylePackPath { get; set; } = String.Empty;
}