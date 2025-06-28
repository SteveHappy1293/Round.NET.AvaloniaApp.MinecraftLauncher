using System;
using OverrideLauncher.Core.Modules.Classes.Account;
using OverrideLauncher.Core.Modules.Classes.Launch.Client;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using OverrideLauncher.Core.Modules.Entry.JavaEntry;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using OverrideLauncher.Core.Modules.Enum.Launch;
using RMCL.Base.Entry;
using RMCL.Core.Models.Classes.Manager.UserManager;

namespace RMCL.Core.Models.Classes.Launch;

public class LaunchService
{
    public static JavaInfo MatchingOptimumJava(VersionParse parse)
    { 
        var java = parse.GameJson.JavaVersion;
        
        foreach (var x in JavaManager.JavaManager.JavaRoot.Javas)
        {
            if (int.Parse(x.Version.Split('.')[0]) == java.MajorVersion)
            {
                return new JavaInfo()
                {
                    JavaPath = x.JavaPath,
                    Version = x.Version
                };
            }
        }
        
        foreach (var x in JavaManager.JavaManager.JavaRoot.Javas)
        {
            if (int.Parse(x.Version.Split('.')[0]) >= java.MajorVersion)
            {
                return new JavaInfo()
                {
                    JavaPath = x.JavaPath,
                    Version = x.Version
                };
            }
        }

        return null;
    }
    public static void Launch(LaunchClientInfo Info)
    {
        var par = new VersionParse(new ClientInstancesInfo()
        {
            GameCatalog = Info.GameFolder,
            GameName = Info.GameName
        });

        var info = JavaManager.JavaManager.JavaRoot.IsAutomaticSelection
            ? new JavaInfo()
            {
                JavaPath = JavaManager.JavaManager.JavaRoot.Javas[
                    JavaManager.JavaManager.JavaRoot.SelectIndex].JavaPath,
                Version = JavaManager.JavaManager.JavaRoot.Javas[
                    JavaManager.JavaManager.JavaRoot.SelectIndex].Version
            }
            : MatchingOptimumJava(par);
        
        ClientRunner run = new ClientRunner(new ClientRunnerInfo()
        {
            Account = PlayerManager.Player.Accounts[PlayerManager.Player.SelectIndex].Account,
            JavaInfo = info,
            LauncherInfo = "RMCL",
            GameInstances = par,
            WindowInfo = ClientWindowSizeEnum.Default
        });
        run.LogsOutput = s => Console.WriteLine(s);
        run.Start();
    }
}