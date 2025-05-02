using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LevelManager.Views.Pages.Server;

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
        
       ServerManage.AddServer(name, ip);
    }
}