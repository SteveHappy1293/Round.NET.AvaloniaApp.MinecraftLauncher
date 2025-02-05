using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class UserShowControl : UserControl
{
    public UserShowControl(string username)
    {
        InitializeComponent();
        NameBox.Content = username;
    }
}