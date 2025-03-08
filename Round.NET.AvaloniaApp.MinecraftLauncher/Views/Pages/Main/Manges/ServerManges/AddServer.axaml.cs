using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.ServerManges;

public partial class AddServer : UserControl
{
    public AddServer()
    {
        InitializeComponent();
    }

    public void Add()
    {
        var name = NameBox.Text;
        var ip = IPBox.Text;
        
        Modules.Server.ServerMange.AddServer(name, ip);
    }
}