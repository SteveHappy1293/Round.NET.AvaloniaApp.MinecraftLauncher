using RMCL.Base.Enum.BackCall;

namespace RMCL.Base.Entry.BackCall;

public class BackCallEntry
{
    public Action CallAction { get; set; }
    public BackCallType Type { get; set; }
}