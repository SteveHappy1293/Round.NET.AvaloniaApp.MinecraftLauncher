using System;
using System.Linq;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Account;
using OverrideLauncher.Core.Modules.Classes.Launch.Client;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using OverrideLauncher.Core.Modules.Entry.JavaEntry;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using OverrideLauncher.Core.Modules.Enum.Launch;
using RMCL.Base.Entry;
using RMCL.Base.Entry.Java;
using RMCL.Base.Enum.BackCall;
using RMCL.Controls.Launch;
using RMCL.Controls.TaskContentControl;
using RMCL.Core.Models.Classes.Manager.BackCallManager;
using RMCL.Core.Models.Classes.Manager.TaskManager;
using RMCL.Core.Models.Classes.Manager.UserManager;
using RMCL.Core.Views.Windows.Main.Client;

namespace RMCL.Core.Models.Classes.Launch;

public class LaunchService
{
    public static JavaInfo MatchingOptimumJava(VersionParse parse)
    {
        if (parse?.GameJson?.JavaVersion == null || JavaManager.JavaManager.JavaRoot?.Javas == null)
        {
            return null;
        }

        var java = parse.GameJson.JavaVersion;
        var availableJavas = JavaManager.JavaManager.JavaRoot.Javas;

        // 尝试找到完全匹配的主版本
        var exactMatch = availableJavas
            .Where(x => TryParseMajorVersion(x.Version, out int major) && major == java.MajorVersion)
            .OrderByDescending(x => ParseFullVersion(x.Version))
            .FirstOrDefault();

        if (exactMatch != null)
        {
            return CreateJavaInfo(exactMatch);
        }

        // 尝试找到更高版本中最接近的
        var higherVersion = availableJavas
            .Where(x => TryParseMajorVersion(x.Version, out int major) && major > java.MajorVersion)
            .OrderBy(x => ParseFullVersion(x.Version))
            .FirstOrDefault();

        if (higherVersion != null)
        {
            return CreateJavaInfo(higherVersion);
        }

        // 尝试找到低版本中最高的
        var lowerVersion = availableJavas
            .Where(x => TryParseMajorVersion(x.Version, out int major) && major < java.MajorVersion)
            .OrderByDescending(x => ParseFullVersion(x.Version))
            .FirstOrDefault();

        return lowerVersion != null ? CreateJavaInfo(lowerVersion) : null;
    }

    // 辅助方法：尝试解析主版本号
    private static bool TryParseMajorVersion(string version, out int major)
    {
        major = 0;
        if (string.IsNullOrEmpty(version))
        {
            return false;
        }

        var parts = version.Split('.');
        if (parts.Length == 0)
        {
            return false;
        }

        return int.TryParse(parts[0], out major);
    }

    // 辅助方法：解析完整版本号用于比较
    private static Version ParseFullVersion(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return new Version(0, 0);
        }

        // 处理可能的非标准版本格式
        version = version.Split('-')[0]; // 去掉可能的后缀如"-ea"
        version = version.Split('_')[0]; // 去掉可能的后缀如"_192"

        // 确保版本号有足够的部分
        var parts = version.Split('.');
        if (parts.Length < 2)
        {
            version += ".0";
        }

        return Version.TryParse(version, out var result) ? result : new Version(0, 0);
    }

    // 辅助方法：创建JavaInfo对象
    private static JavaInfo CreateJavaInfo(JavaDetils entry)
    {
        return new JavaInfo()
        {
            JavaPath = entry.JavaPath,
            Version = entry.Version
        };
    }

    public static ClientRunner Launch(LaunchClientInfo Info,Action<string> LogOutput = null)
    {
        BackCallManager.Call(BackCallType.LaunchedGame);

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
            LauncherInfo = "RMCL (By OverrideLauncher.Core)",
            GameInstances = par,
            WindowInfo = ClientWindowSizeEnum.Default
        });
        run.LogsOutput = s => LogOutput.Invoke(s);
        
        return run;
    }

    public static void LaunchTask(LaunchClientInfo Info)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var dow = new LaunchClientTaskItem();
            var cont = new TaskControl()
            {
                BoxContent = dow,
                TaskName = $"启动游戏 - {Info.GameName}"
            };
            cont.RunTask();
            var uuid1 = TaskManager.AddTask(cont);
            var logWindow = new ClientLogViewWindow();
            dow.ExitCompleted = (uuid) =>
            {
                TaskManager.DeleteTask(uuid1);
                Dispatcher.UIThread.Invoke(() => logWindow.GameExit());
            };
            dow.Launching = (entry) =>
            {
                dow.Runner = Launch(entry, (s) =>
                {
                    Console.WriteLine(s);
                    
                    Dispatcher.UIThread.Invoke(() => logWindow.AddLog(s));
                });
                dow.RunningGame();
                logWindow.GameProcess = dow.Runner.GameProcess;
                Dispatcher.UIThread.Invoke(() => logWindow.Show());
            };
            dow.Launch(Info);
        });
    }
}