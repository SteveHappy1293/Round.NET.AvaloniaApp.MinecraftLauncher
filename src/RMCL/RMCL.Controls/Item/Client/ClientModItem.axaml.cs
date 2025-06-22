using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Item.Client;

public partial class ClientModItem : UserControl
{
    public ClientModItem()
    {
        InitializeComponent();
    }

    public async Task LoadInfoAsync(string filename)
    {
        await Task.Run(() =>
        {
            LoadModInfo.LoadJarMod.LoadJarModInfo(filename);
        });
    }
}