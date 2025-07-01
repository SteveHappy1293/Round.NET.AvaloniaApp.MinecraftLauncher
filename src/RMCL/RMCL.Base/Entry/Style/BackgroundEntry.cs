using RMCL.Base.Enum;

namespace RMCL.Base.Entry.Style;

public class BackgroundEntry
{
    public BackgroundModelEnum ChooseModel { get; set; } = BackgroundModelEnum.None;
    public ColorGlassEntry ColorGlassEntry { get; set; } = new();
    public ImageEntry ImageEntry { get; set; } = new();
    public PackEntry PackEntry { get; set; } = new();
}

public class ColorGlassEntry
{
    public string HtmlColor { get; set; } = "#000000";
}

public class ImageEntry
{
    public int ChooseIndex { get; set; } = -1;
    public ImageFilledModel Model { get; set; } = ImageFilledModel.UniformToFill;
    public List<string> ImagePaths { get; set; } = new();
    public int Opacity { get; set; } = 50;
}

public class PackEntry
{
    public string StylePackPath { get; set; } = String.Empty;
    public int SelectedIndex { get; set; } = -1;
}