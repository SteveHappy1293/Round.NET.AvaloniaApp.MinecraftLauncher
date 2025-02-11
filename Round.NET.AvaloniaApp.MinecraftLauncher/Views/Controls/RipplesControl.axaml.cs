using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class RipplesControl : UserControl
{
    public RipplesControl()
    {
        InitializeComponent();
    }
    public void CircleShow(double CircleOpacity)
    {
        Dispatcher.UIThread.Invoke(() => IsVisible = true);
        if (Circle.Width == 0)
        {
            var wid = 1000.00;
            if (Core.MainWindow.Bounds.Width * 2 > Core.MainWindow.Bounds.Height * 2)
            {
                wid = Core.MainWindow.Bounds.Width*2;
            }
            else
            {
                wid = Core.MainWindow.Bounds.Height*2;
            }
            Circle.Width = wid;
            Circle.Height = wid;
            Circle.Opacity = CircleOpacity;
        }
        else
        {
            Circle.Width = 0;
            Circle.Height = 0;   
            Circle.Opacity = 0;
        }
    }
}