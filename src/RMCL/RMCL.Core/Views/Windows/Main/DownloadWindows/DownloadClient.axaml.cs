using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using OverrideLauncher.Core.Modules.Classes.Download;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;
using RMCL.Controls.Download;
using RMCL.Controls.TaskContentControl;
using RMCL.Core.Models.Classes.Manager.TaskManager;
using RMCL.Logger;
using RMCL.Core.Models.Classes;

namespace RMCL.Core.Views.Windows.Main.DownloadWindows;

public partial class DownloadClient : Window
{
    public static string FormatFileSize(ulong bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; // 如有需要可以继续扩展

        if (bytes == 0)
            return "0" + suffixes[0];

        long absBytes = Math.Abs((long)bytes);
        int place = Convert.ToInt32(Math.Floor(Math.Log(absBytes, 1024)));
        double num = Math.Round(absBytes / Math.Pow(1024, place), 1);

        return (Math.Sign((long)bytes) * num).ToString() + suffixes[place];
    }

    public InstallClient Install;

    public DownloadClient(VersionManifestEntry.Version versioninfo)
    {
        InitializeComponent();

        VersionInstallName.Text = versioninfo.Id;
        VersionInstallName.Watermark = versioninfo.Id;
        VersionLabel.Content = versioninfo.Id;
        BasicInstallationSettings.IsEnabled = false;
        LoadBox.IsVisible = true;

        Task.Run(() =>
        {
            Install = new InstallClient(DownloadVersionHelper.TryingFindVersion(versioninfo.Id).Result);
            Dispatcher.UIThread.Invoke(() =>
                BasicInstallationSettings.IsEnabled = true);
            // var size = Install.GetThePreInstalledSize().Result;
            ulong size = 0;

            var sizetxt = FormatFileSize(size);
            Dispatcher.UIThread.Invoke(() =>
            {
                LoadBox.IsVisible = false;

                ClientSizeLabel.Content = sizetxt;
            });
        });
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
        var ID = VersionInstallName.Watermark;
        if (string.IsNullOrEmpty(name)) name = VersionInstallName.Watermark;

        LoadBox.IsVisible = true;
        BasicInstallationSettings.IsEnabled = false;
        ControlDockPanel.IsEnabled = false;
        Task.Run(() =>
        {
            while (Install == null)
            {
            }

            Install.DownloadThreadsCount = Config.Config.MainConfig.DownloadThreads;

            Dispatcher.UIThread.Invoke(() =>
            {

                var dow = new DownloadClientTaskItem(Install);
                var cont = new TaskControl()
                {
                    BoxContent = dow,
                    TaskName = $"安装游戏 - {ID}"
                };
                cont.RunTask();
                dow.Download(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
                    name);
                var uuid1 = TaskManager.AddTask(cont);
                dow.DownloadCompleted = (uuid) => TaskManager.DeleteTask(uuid1);
                Models.Classes.Core.MessageShowBox.AddInfoBar("安装游戏", $"已将 {ID} 的安装添加至后台", InfoBarSeverity.Success);

                Close();
            });
        });
    }
}