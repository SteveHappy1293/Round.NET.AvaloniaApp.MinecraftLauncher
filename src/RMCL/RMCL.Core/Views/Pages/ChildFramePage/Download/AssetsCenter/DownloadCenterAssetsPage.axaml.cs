using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RMCL.AssetsCenter;
using RMCL.Base.Entry.Assets.Center;
using RMCL.Controls.Download;
using RMCL.Controls.Item;
using RMCL.Controls.TaskContentControl;
using RMCL.Core.Models.Classes.Manager.TaskManager;

namespace RMCL.Core.Views.Pages.ChildFramePage.Download.AssetsCenter;

public partial class DownloadCenterAssetsPage : UserControl
{
    public DownloadCenterAssetsPage()
    {
        InitializeComponent();
    }

    public void LoadAssets(AssetsIndexItemEntry info)
    {
        NameBox.Text = info.Name;
        ProfileBox.Text = info.Description;
        
        Task.Run(() =>
        {
            GetAssetInfo.GetAssetItemInfo(info).Result.Versions.ForEach(x =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var it = new VersionItem(x.Version);
                    it.DownloadClick = () =>
                    {
                        Download(x);
                    };
                    ResultBox.Children.Add(it);
                });
            });
        });
    }

    public void Download(AssetInfoEntry.VersionInfo info)
    {
        var dow = new DownloadCenterAssetItem();
        var cont = new TaskControl()
        {
            BoxContent = dow,
            TaskName = $"下载 RMCL 资源"
        };
        cont.RunTask();
        dow.Download(info,PathsDictionary.PathDictionary.SkinFolder);
        var uuid1 = TaskManager.AddTask(cont);
        dow.DownloadCompleted = () => TaskManager.DeleteTask(uuid1);
    }
}