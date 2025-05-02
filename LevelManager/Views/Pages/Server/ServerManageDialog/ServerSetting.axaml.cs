using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.VersionServerMange.Library.Entry;

namespace LevelManager.Views.Pages.Server;

public partial class ServerSetting : UserControl
{
    public ServerSetting(ServerEntry serverEntry)
    {
        InitializeComponent();
    }
}