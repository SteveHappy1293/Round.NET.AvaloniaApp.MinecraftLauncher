using RMCL.Base.Enum;

namespace RMCL.Base.Entry.Style;

public class BackgroundEntry
{
    public BackgroundModelEnum ChooseModel { get; set; } = BackgroundModelEnum.None;

    public ColorGlassEntry ColorGlassEntry { get; set; } = new();
    public ImageEntry ImageEntry { get; set; } = new();
    public ColorEntry ColorEntry { get; set; } = new();
    public PackEntry PackEntry { get; set; } = new();
}

public class ColorGlassEntry
{
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