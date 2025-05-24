using OverrideLauncher.Core.Modules.Classes.Version;
using RMCL.Base.Entry.Java;

namespace RMCL.Base.Entry;

public class LaunchClientInfo
{
    public JavaInfoEntry Java { get; set; }
    public string GameFolder { get; set; }
    public string GameName { get; set; }
}