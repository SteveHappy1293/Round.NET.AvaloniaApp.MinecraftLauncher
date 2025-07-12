using System.Collections.Concurrent;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;
using RMCL.AssetsPool;

namespace RMCL.Controls.Item
{
    public partial class DownloadGameItem : UserControl
    {
        private static readonly ConcurrentDictionary<string, Bitmap> _imageCache = new ConcurrentDictionary<string, Bitmap>();

        public Action<string> OnDownload = s => { };

        public DownloadGameItem(VersionManifestEntry.Version version)
        {
            InitializeComponent();
            VersionName.Text = version.Id;
            VersionType.Text = version.Type;
            VersionTime.Text = DateTime.Parse(version.Time).ToString("yyyy/MM/dd HH:mm:ss");

            string resourcePath = $"avares://RMCL.Controls/Assets/MinecraftIcons/{version.Type}.png";
            
            var bitmap = Pools.AvaloniaResourcesPool.GetImage(resourcePath,24);

            IconImage.Source = bitmap;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            OnDownload(VersionName.Text);
        }
    }
}