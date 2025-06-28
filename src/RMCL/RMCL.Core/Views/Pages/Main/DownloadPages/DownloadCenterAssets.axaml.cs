using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RMCL.AssetsCenter;

namespace RMCL.Core.Views.Pages.Main.DownloadPages;

public partial class DownloadCenterAssets : UserControl
{
    public DownloadCenterAssets()
    {
        InitializeComponent();

        Task.Run(async () =>
        {
            var ls = await GetIndex.GetPluginIndex();
            ls.ForEach(x =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    AssetsList.Children.Add(new TextBlock() { Text = x.Name });
                });
            });
        });
    }

    private void SearchBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}