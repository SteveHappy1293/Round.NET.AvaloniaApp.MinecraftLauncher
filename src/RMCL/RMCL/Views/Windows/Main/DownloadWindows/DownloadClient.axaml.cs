using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Download;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;
using RMCL.Controls.Download;
using RMCL.Controls.TaskContentControl;
using RMCL.Models.Classes;

namespace RMCL.Views.Windows.Main.DownloadWindows;

public partial class DownloadClient : Window
{
    public DownloadClient(VersionManifestEntry.Version versioninfo)
    {
        InitializeComponent();

        VersionInstallName.Text = versioninfo.Id;
        VersionInstallName.Watermark = versioninfo.Id;
        VersionLabel.Content = versioninfo.Id;
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void InstallBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var name = VersionInstallName.Text;
        var id = VersionInstallName.Watermark.ToString();
        if (string.IsNullOrEmpty(name)) name = VersionInstallName.Watermark;

        LoadBox.IsVisible = true;
        BasicInstallationSettings.IsEnabled = false;
        ControlDockPanel.IsEnabled = false;
        Task.Run(() =>
        {
            var ins = new InstallClient(DownloadVersionHelper.TryingFindVersion(id).Result, name);
            ins.DownloadThreadsCount = Config.Config.MainConfig.DownloadThreads;
            
            Thread.Sleep(1200);
            Dispatcher.UIThread.Invoke(() =>
            {

                var dow = new DownloadClientTaskItem(ins);
                var cont = new TaskControl()
                {
                    BoxContent = dow,
                    TaskName = $"安装游戏 - {id}"
                };
                cont.RunTask();
                dow.Download(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);

                Core.MainWindow.Content = cont;

                Close();
            });
        });
    }
}