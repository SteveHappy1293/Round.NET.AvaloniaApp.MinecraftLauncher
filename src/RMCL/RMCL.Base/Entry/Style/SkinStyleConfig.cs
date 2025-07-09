namespace RMCL.Base.Entry.Style;

public class SkinStyleConfig
{
    public double Opacity { get; set; } = 100;
    public string Background { get; set; } = String.Empty;
    public BackgroundEntry BackgroundModel { get; set; } = new();
    public ColorEntry ThemeColors { get; set; } = new();
    public ButtonStyle ButtonStyle { get; set; } = new();
    public BackMusicEntry BackMusicEntry { get; set; } = new();
    public int PackVersion { get; set; } = -1;
    public string Writer { get; set; } = "RMCL Skin Pack Build Tools";
    public string Info { get; set; } = "RMCL Skin Pack";
    
    public bool IsBackground { get; set; } = true;
    public bool IsButton { get; set; } = true;
    public bool IsColor { get; set; } = true;
    public bool IsMusic { get; set; } = true;

    public int Model { get; set; } = 2; // 兼容旧版 RMCL
}