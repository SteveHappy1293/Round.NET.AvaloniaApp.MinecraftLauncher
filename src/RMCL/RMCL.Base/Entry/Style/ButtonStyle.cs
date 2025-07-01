using RMCL.Base.Enum.ButtonStyle;

namespace RMCL.Base.Entry.Style;

public class ButtonStyle
{
    public OrdinaryButtonStyle HomeButton { get; set; } = OrdinaryButtonStyle.Default;
    public OrdinaryButtonStyle QuickChoosePlayerButton { get; set; } = OrdinaryButtonStyle.None;
}