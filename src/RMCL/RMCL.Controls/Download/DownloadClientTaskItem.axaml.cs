using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Download;
using OverrideLauncher.Core.Modules.Enum.Download;
using RMCL.Controls.TaskContentControl;

namespace RMCL.Controls.Download;

public partial class DownloadClientTaskItem : UserControl
{
    private InstallClient InstallClient;
    public Action<string> DownloadCompleted;
    public DownloadClientTaskItem(InstallClient install)
    {
        InitializeComponent();
        this.InstallClient = install;
    }

    public string GetState(DownloadStateEnum enu)
    {
        return enu switch
        {
            DownloadStateEnum.DownloadAssets => "正在下载资源",
            DownloadStateEnum.DownloadAssetsSuccess => "资源下载完成",
            DownloadStateEnum.DownloadLibrary => "正在下载依赖库",
            DownloadStateEnum.DownloadLibrarySuccess => "依赖库下载完成",
            DownloadStateEnum.DownloadClient => "正在下载客户端",
            DownloadStateEnum.DownloadSuccess => "客户端下载完成",
            _ => "未知进度"
        };
    }
    public void Download(string gamedir)
    {
        InstallClient.ProgressCallback = (e,s, d) =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                ProgressTextBox.Text = $"{d:0.00}% - {GetState(e)} {s}";
                DownloadProgress.Value = d;

                if (e==DownloadStateEnum.DownloadSuccess)
                {
                    var cont = this.Tag as string;
                    DownloadCompleted(cont);
                }
            });
        };
        Task.Run(() => InstallClient.Install(Path.GetFullPath(gamedir)));
    }
}