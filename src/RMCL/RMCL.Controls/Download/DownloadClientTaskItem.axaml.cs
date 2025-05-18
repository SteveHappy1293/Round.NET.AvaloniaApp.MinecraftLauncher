using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Download;

namespace RMCL.Controls.Download;

public partial class DownloadClientTaskItem : UserControl
{
    private InstallClient InstallClient;
    public DownloadClientTaskItem(InstallClient install)
    {
        InitializeComponent();
        this.InstallClient = install;
    }

    public void Download(string gamedir)
    {
        InstallClient.ProgressCallback = (s, d) =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                ProgressTextBox.Text = $"{d:0.00}% - {s}";
                DownloadProgress.Value = d;
            });
        };
        Task.Run(() => InstallClient.Install(Path.GetFullPath(gamedir)));
    }
}