using Avalonia.Controls;

namespace RMCL.Controls.ControlHelper;

public class BottomBarNavigationEntry
{
    public Control Title { get; set; }
    public string Tag { get; set; }
    public UserControl Page { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsNoButton { get; set; } = false;

    /// <summary>
    /// 页面工厂函数，用于懒加载页面实例
    /// 如果设置了此属性，将优先使用工厂函数创建页面
    /// </summary>
    public Func<UserControl> PageFactory { get; set; }
}

public class BottomBarNavigationItemEntry : BottomBarNavigationEntry
{
    public Avalonia.Controls.Button NavItem { get; set; }
    public bool IsThis { get; set; } = false;
}