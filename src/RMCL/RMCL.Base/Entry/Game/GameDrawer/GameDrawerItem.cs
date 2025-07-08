using OverrideLauncher.Core.Modules.Entry.LaunchEntry;

namespace RMCL.Base.Entry.Game.GameDrawer;

public class GameDrawerItem
{
    public LaunchClientInfo ClientInfo { get; set; }
    public string Uuid { get; set; } = Guid.NewGuid().ToString();
}