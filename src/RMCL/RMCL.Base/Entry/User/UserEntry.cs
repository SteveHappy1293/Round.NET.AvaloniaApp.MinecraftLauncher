using OverrideLauncher.Core.Modules.Entry.AccountEntry;

namespace RMCL.Base.Entry.User;

public class UserEntry
{
    public AccountEntry Account { get; set; }
    public string Skin { get; set; } = String.Empty;
    public string SkinUrl { get; set; } = String.Empty;
}