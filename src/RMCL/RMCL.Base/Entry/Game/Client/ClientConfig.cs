using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using OverrideLauncher.Core.Modules.Enum.Launch;
using RMCL.Base.Enum.Client;
using RMCL.SystemHelper;

namespace RMCL.Base.Entry.Game.Client;

public class ClientConfig
{
    public LogViewShowEnum LogViewShow { get; set; } = LogViewShowEnum.Auto;
    public LauncherVisibilityEnum LauncherVisibility { get; set; } = LauncherVisibilityEnum.Visibility;
    public GameWindowInfo GameWindowInfo { get; set; } = ClientWindowSizeHelper.Default;
}