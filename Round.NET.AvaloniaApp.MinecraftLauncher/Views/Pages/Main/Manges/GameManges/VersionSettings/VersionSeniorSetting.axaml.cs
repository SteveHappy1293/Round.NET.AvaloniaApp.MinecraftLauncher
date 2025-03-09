using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges.VersionSettings;

public partial class VersionSeniorSetting : UserControl
{
    private string _version;

    public string version
    {
        get
        {
            return _version;
        }
        set
        {
            _version = value;
        }
    }

    public VersionSeniorSetting()
    {
        InitializeComponent();
    }
}