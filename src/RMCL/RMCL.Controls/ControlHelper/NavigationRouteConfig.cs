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

    /// <summary>
    /// 页面工厂函数，用于懒加载页面实例
    /// 如果设置了此属性，将优先使用工厂函数创建页面
    /// </summary>
    public Func<UserControl> PageFactory { get; set; }
}