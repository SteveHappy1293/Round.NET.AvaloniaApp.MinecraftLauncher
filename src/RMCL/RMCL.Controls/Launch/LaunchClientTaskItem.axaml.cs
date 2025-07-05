using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Launch.Client;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using RMCL.Base.Entry;

namespace RMCL.Controls.Launch;

public partial class LaunchClientTaskItem : UserControl
{
    public Action<string> ExitCompleted;
    public Action<LaunchClientInfo> Launching;
    public ClientRunner Runner;
    public LaunchClientTaskItem()
    {
        InitializeComponent();
    }

    public async Task Launch(LaunchClientInfo Info)
    {
        ProgressTextBox.Text = "补全文件中...";
        Task.Run(() =>
        {
            FileIntegrityChecker fileIntegrityChecker = new FileIntegrityChecker(new VersionParse(new ClientInstancesInfo()
            {
                GameCatalog = Info.GameFolder,
                GameName = Info.GameName
            })); // ver 参数是先前读取的游戏
        
            GameFileCompleter fileCompleter = new GameFileCompleter();
            fileCompleter.ProgressCallback = (@enum, s, arg3) => Dispatcher.UIThread.Invoke(() =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    ProgressTextBox.Text = $"{SystemHelper.GetDownloadStateText.GetText(@enum)} {s} {arg3:0.00} %";
                    CompleteTheResourceFile.Value = arg3;
                });
            });
            fileCompleter.DownloadMissingFilesAsync(fileIntegrityChecker.GetMissingFiles()).Wait();
            Dispatcher.UIThread.Invoke(() =>
            {
                ProgressTextBox.Text = $"资源补全完毕";
                CompleteTheResourceFile.Value = 100;
            });
            
            Launching.Invoke(Info);
        });
    }

    public void RunningGame()
    {
        Runner.GameExit = (() => ExitCompleted.Invoke(""));
        Runner.Start();
        
        Dispatcher.UIThread.Invoke(() =>
        {
            LaunchTheGame.Value = 100;
        });
    }
}