using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RMCL.Controls.Item.Safe;
using RMCL.PathsDictionary;

namespace RMCL.Core.Views.Pages.ChildFramePage.Safe;

public partial class ManageLogsChildPage : UserControl
{
    public ManageLogsChildPage()
    {
        InitializeComponent();
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        LogsView.Children.Clear();
        NullBox.IsVisible = false;
        LogsView.IsVisible = false;
        Loading.IsVisible = true;

        Task.Run(() =>
        {
            var file = Directory.GetFiles(PathDictionary.LogsPath).ToList();
            for (var i = file.Count - 1; i >= 0; i--)
            {
                var x = file[i];
                Dispatcher.UIThread.Invoke(() =>
                {
                    var it = new LogItem();
                    it.OnOpen += (sender, s) => SystemHelper.SystemHelper.OpenFile(s);
                    LogsView.Children.Add(it);
                    it.Load(x, Path.GetFileName(x) != Path.GetFileName(RMCL.Logger.ConsoleRedirector.FileName));
                });
            }

            Dispatcher.UIThread.Invoke(() =>
            {
                if (file.Count <= 0)
                {
                    NullBox.IsVisible = true;
                    LogsView.IsVisible = false;
                    Loading.IsVisible = false;
                    return;
                }

                NullBox.IsVisible = false;
                LogsView.IsVisible = true;
                Loading.IsVisible = false;
            });
        });
    }
}