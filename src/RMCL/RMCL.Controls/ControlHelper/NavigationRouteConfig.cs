using FluentAvalonia.FluentIcons;

namespace RMCL.Controls.ControlHelper;

public class NavigationRouteConfig
{
    public string Route { get; set; }
    public string Title { get; set; }
    public FluentIconSymbol Icon { get; set; }
    public bool IsFoot { get; set; } = false;
}