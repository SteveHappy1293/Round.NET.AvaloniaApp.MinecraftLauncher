using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OnlinePlug.Views.Pages;

public partial class OnlineMain : UserControl
{
    public OnlineMain()
    {
        InitializeComponent();
        
        MinecraftClient.StartListeningMinecraftBroadcast();
    }
}