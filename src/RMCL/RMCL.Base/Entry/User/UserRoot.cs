using OverrideLauncher.Core.Modules.Entry.AccountEntry;

namespace RMCL.Base.Entry.User;

public class UserRoot
{
    public List<UserEntry> Accounts { get; set; } = new();
    public int SelectIndex { get; set; }
}