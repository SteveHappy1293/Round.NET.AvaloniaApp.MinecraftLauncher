using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MCLauncher.Versions;

namespace BedrockPlug.View.Controls;

public partial class DownloadControl : UserControl
{
    public string Tuid { get; set; } = string.Empty;
    public DownloadControl(VersionInfo versionInfo)
    {
        InitializeComponent();
    }

    public void Download()
    {
        
    }
}