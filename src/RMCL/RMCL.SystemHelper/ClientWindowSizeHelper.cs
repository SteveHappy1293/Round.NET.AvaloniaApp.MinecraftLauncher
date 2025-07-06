using Avalonia.Controls;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using OverrideLauncher.Core.Modules.Enum.Launch;

namespace RMCL.SystemHelper;

public class ClientWindowSizeHelper
{
    public static readonly GameWindowInfo Default = ClientWindowSizeEnum.Default;

    public static readonly GameWindowInfo Max = new GameWindowInfo()
    {
        Width = TopLevel.GetTopLevel(null)?.Screens.Primary.WorkingArea.Width ?? 1920, // 默认 1920
        Height = TopLevel.GetTopLevel(null)?.Screens.Primary.WorkingArea.Height ?? 1080 // 默认 1080
    };
}