using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.VersionServerMange.Library.Entry;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.ServerManges;

public partial class ServerSetting : UserControl
{
    public ServerSetting(ServerEntry serverEntry)
    {
        InitializeComponent();
    }
}