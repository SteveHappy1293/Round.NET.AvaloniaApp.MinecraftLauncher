using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using RMCL.Config;
using RMCL.Controls.Item.Safe;
using RMCL.Core.Views.Windows.Main.Exception;
using RMCL.Modules.Entry;
using RMCL.PathsDictionary;

namespace RMCL.Core.Views.Pages.ChildFramePage.Safe;

public partial class ManageExceptionChildPage : UserControl
{
    public ManageExceptionChildPage()
    {
        InitializeComponent();
        UpdateUI();
    }
    public void UpdateUI()
    {
        ExceptionsView.Children.Clear();
        NullBox.IsVisible = false;
        ExceptionsView.IsVisible = false;
        Loading.IsVisible = true;

        Task.Run(() =>
        {
            var file = Directory.GetDirectories(PathDictionary.ExceptionFolder).ToList();
            for (var i = file.Count - 1; i >= 0; i--)
            {
                var x = file[i];
                Dispatcher.UIThread.Invoke(() =>
                {
                    var entry = JsonSerializer.Deserialize<ExceptionEntry>(
                        File.ReadAllText(Path.Combine(x, "Exception.json")));

                    if (!string.IsNullOrEmpty(entry.ExceptionName))
                    {
                        var it = new ExceptionItem();
                        it.OnOpen += (sender, s) =>
                        {
                            var win = new ExceptionReportWindow();
                            win.PackFile = Path.Combine(x, "Pack.rexp");
                            win.ShowException(entry);
                            win.Show();
                        };
                        ExceptionsView.Children.Add(it);
                        it.ShowLoad(entry);
                    }
                });
            }

            Dispatcher.UIThread.Invoke(() =>
            {
                if (file.Count <= 0)
                {
                    NullBox.IsVisible = true;
                    ExceptionsView.IsVisible = false;
                    Loading.IsVisible = false;
                    return;
                }

                NullBox.IsVisible = false;
                ExceptionsView.IsVisible = true;
                Loading.IsVisible = false;
            });
        });
    }
}