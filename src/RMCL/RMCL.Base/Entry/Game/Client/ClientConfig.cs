using RMCL.Base.Enum.Client;

namespace RMCL.Base.Entry.Game.Client;

public class ClientConfig
{
    public LogViewShowEnum LogViewShow { get; set; } = LogViewShowEnum.Auto;
    public LauncherVisibilityEnum LauncherVisibility { get; set; } = LauncherVisibilityEnum.Visibility;
}