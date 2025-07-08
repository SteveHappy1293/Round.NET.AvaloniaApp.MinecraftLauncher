namespace RMCL.Base.Entry.Skin;

public class SkinRenderEntry
{
    public bool IsUse3D { get; set; } = true;
    public Skin3DRenderConfigEntry Skin3DRenderConfigEntry { get; set; } = new();
}