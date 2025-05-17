using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;

namespace RMCL.Views.Windows.Main.DownloadWindows;

public partial class DownloadClient : Window
{
    public DownloadClient(VersionManifestEntry.Version versioninfo)
    {
        InitializeComponent();

        VersionInstallName.Text = versioninfo.Id;
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
}