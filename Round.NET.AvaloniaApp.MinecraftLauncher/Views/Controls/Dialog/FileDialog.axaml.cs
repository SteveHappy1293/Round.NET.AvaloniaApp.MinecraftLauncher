using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Dialog;

public partial class FileDialog : UserControl
{
    public FileDialog(string Title = "选择一个文件",string type = "*")
    {
        InitializeComponent();
        TitleLabel.Content = Title;
        InitTreeView();
    }

    public void InitTreeView()
    {
        TreeView.Items.Clear();
        foreach (var VARIABLE in DriveInfo.GetDrives())
        {
            if (VARIABLE.IsReady)
            {

                var item = new TreeViewItem { Header = $"{VARIABLE.VolumeLabel}（{VARIABLE.Name}）", Tag = VARIABLE.Name };
                item.Items.Add(new TreeViewItem{Tag = "TakingPlace"});
                item.Expanded += (sender, args) => { InitTreeViewItem(item); };
                TreeView.Items.Add(item);
            }
        }
    }

    public void InitTreeViewItem(TreeViewItem parent)
    {
        var path = parent.Tag as string;
        if (((TreeViewItem)parent.Items[0]).Tag == "TakingPlace")
        {
            parent.Items.Clear();
            foreach (var VARIABLE in Directory.GetDirectories(path))
            {
                var item = new TreeViewItem { Header = new FileInfo(VARIABLE).Name, Tag = VARIABLE };
                item.Items.Add(new TreeViewItem{Tag = "TakingPlace"});
                item.Expanded += (sender, args) => { InitTreeViewItem(item); };
                parent.Items.Add(item);
            }

            foreach (var VARIABLE in Directory.GetFiles(path))
            {
                var item = new TreeViewItem { Header = new FileInfo(VARIABLE).Name, Tag = VARIABLE };
                parent.Items.Add(item);
            }
        }
    }
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => this.Dialog.Margin = new Thickness(0,500,0,-500));
            Dispatcher.UIThread.Invoke(() => this.Opacity = 0);
            Thread.Sleep(500);
            Dispatcher.UIThread.Invoke(() => ((Grid)((MainView)Core.MainWindow.Content).Content).Children.Remove(this));
        });
    }
    private void SelectButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => this.Dialog.Margin = new Thickness(0,500,0,-500));
            Dispatcher.UIThread.Invoke(() => this.Opacity = 0);
            Thread.Sleep(500);
            Dispatcher.UIThread.Invoke(() => ((Grid)((MainView)Core.MainWindow.Content).Content).Children.Remove(this));
        });
    }

    public string result = null;
    public string Show()
    {
        this.Dialog.Margin = new Thickness(0, 100, 0, -100);
        this.Opacity = 0;
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((Grid)((MainView)Core.MainWindow.Content).Content).Children.Add(this));
            Dispatcher.UIThread.Invoke(() => this.Dialog.Margin = new Thickness(0,0,0,0));
            Thread.Sleep(200);
            Dispatcher.UIThread.Invoke(() => this.Opacity = 1);
        });
        while (result != null)
        {
            return result;
        }
        return null;    
    }
}