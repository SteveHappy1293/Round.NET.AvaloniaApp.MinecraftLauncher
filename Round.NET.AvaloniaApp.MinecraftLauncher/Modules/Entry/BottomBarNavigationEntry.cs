using Avalonia.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;

public class BottomBarNavigationEntry
{
    public Control Title { get; set; }
    public string Tag { get; set; }
    public IParentPage Page { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsNoButton { get; set; } = false;
}

public class BottomBarNavigationItemEntry : BottomBarNavigationEntry
{
    public Button NavItem { get; set; }
    public bool IsThis { get; set; } = false;
}