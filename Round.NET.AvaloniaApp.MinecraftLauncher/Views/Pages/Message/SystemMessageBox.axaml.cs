using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Message;

public partial class SystemMessageBox : UserControl
{
    public SystemMessageBox()
    {
        InitializeComponent();
        Core.SystemMessage = this;
    }
}