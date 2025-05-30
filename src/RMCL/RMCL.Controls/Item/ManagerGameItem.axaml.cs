using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OverrideLauncher.Core.Modules.Classes.Version;

namespace RMCL.Controls.Item;

public partial class ManagerGameItem : UserControl
{
    public Action<VersionParse> OnLaunch = s => { };
    private VersionParse _versionParse;
    private static readonly ConcurrentDictionary<string, Bitmap> _imageCache = new ConcurrentDictionary<string, Bitmap>();
    public ManagerGameItem(VersionParse versionInfo)
    {
        _versionParse = versionInfo;
        InitializeComponent();

        VersionName.Text = versionInfo.ClientInstances.GameName;
        VersionType.Text = versionInfo.GameJson.Type;
        VersionTime.Text = DateTime.Parse(versionInfo.GameJson.Time).ToString("yyyy/MM/dd HH:mm:ss");
        
        string resourcePath = $"avares://RMCL.Controls/Assets/MinecraftIcons/{versionInfo.GameJson.Type}.png";
        var bitmap = _imageCache.GetOrAdd(resourcePath, key =>
        {
            using var stream = AssetLoader.Open(new Uri(key));
            return Bitmap.DecodeToWidth(stream, 24); // 按需调整目标宽度
        });

        IconImage.Source = bitmap;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OnLaunch(_versionParse);
    }
}