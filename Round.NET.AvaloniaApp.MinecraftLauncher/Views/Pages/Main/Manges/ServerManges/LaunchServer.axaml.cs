using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.ServerManges;

public partial class LaunchServer : UserControl
{
    public LaunchServer()
    {
        InitializeComponent();
        LoadVersionsAsync();
    }

    private async void LoadVersionsAsync()
    {
        var versionsPath = Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder].Path+"/versions";
        var list = await Task.Run(() => Directory.GetDirectories(versionsPath));
        
        await Dispatcher.UIThread.InvokeAsync(() => {
            foreach (var version in list)
            {
                VersionsBox.Items.Add(new ComboBoxItem(){Content = Path.GetFileName(version), Tag = Path.GetFileName(version)});
            }
            VersionsBox.SelectedIndex = Config.MainConfig.GameFolders[Config.MainConfig.SelectedGameFolder].SelectedGameIndex;
        });
    }
}