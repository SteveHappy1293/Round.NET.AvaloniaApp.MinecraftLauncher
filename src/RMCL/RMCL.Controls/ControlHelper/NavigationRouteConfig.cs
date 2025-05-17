using Avalonia.Controls;
using FluentAvalonia.FluentIcons;

namespace RMCL.Controls.ControlHelper;

public class NavigationRouteConfig
{
    public string Route { get; set; }
    public string Title { get; set; }
    public FluentIconSymbol Icon { get; set; }
    public UserControl Page { get; set; }
    public bool IsFoot { get; set; } = false;
}