using RMCL.Base.Entry;

namespace RMCL.Core.Models.Classes.Code;

public class CodeInterface
{
    public static void LaunchGame(string GameFolder, string GameID)
    {
        Launch.LaunchService.Launch(new LaunchClientInfo()
        {
            GameFolder = GameFolder,
            GameName = GameID
        });
    }
}