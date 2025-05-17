using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;

namespace RMCL.Controls.Item;

public partial class DownloadGameItem : UserControl
{
    public Action<string> OnDownload = s => { };
    public DownloadGameItem(VersionManifestEntry.Version version)
    {
        InitializeComponent();
        VersionName.Text = version.Id;
        VersionType.Text = version.Type;
        VersionTime.Text = DateTime.Parse(version.Time).ToString("yyyy/MM/dd  HH:mm:ss");
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OnDownload(VersionName.Text);
    }
}