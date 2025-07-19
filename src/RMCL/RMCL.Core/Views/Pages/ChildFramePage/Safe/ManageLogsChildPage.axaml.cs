using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using RMCL.Controls.Item.Safe;
using RMCL.Core.Models.Classes.Manager.UserManager;
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
                    it.OnSave += async (sender, s) =>
                    {
                        // 获取当前窗口（TopLevel）
                        var topLevel = TopLevel.GetTopLevel(this); // 假设在 Window/UserControl 内部
                        if (topLevel == null)
                            return;

                        // 创建 SaveFileDialog 选项
                        var options = new FilePickerSaveOptions
                        {
                            Title = "保存 RMCL 日志文件",
                            DefaultExtension = ".log",
                            FileTypeChoices = new[]
                            {
                                new FilePickerFileType("日志文件") { Patterns = new[] { "*.log" } },
                            }
                        };
                        options.SuggestedFileName = Path.GetFileName(s);
                        var file = await topLevel.StorageProvider.SaveFilePickerAsync(options);

                        if (file == null)
                            return; // 用户取消

                        // 获取文件路径
                        var filePath = file.Path.LocalPath;

                        File.Copy(s,filePath);
                    };
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