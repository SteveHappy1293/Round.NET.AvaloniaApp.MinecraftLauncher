using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.ServerManges;

public partial class LaunchServer : UserControl
{
    public LaunchServer()
    {
        InitializeComponent();
        var list = Directory.GetDirectories(Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder].Path+"/versions");
        foreach (var version in list)
        {
            VersionsBox.Items.Add(new ComboBoxItem(){Content = Path.GetFileName(version), Tag = Path.GetFileName(version)});
        }

        VersionsBox.SelectedIndex =
            Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder].SelectedGameIndex;
    }
}