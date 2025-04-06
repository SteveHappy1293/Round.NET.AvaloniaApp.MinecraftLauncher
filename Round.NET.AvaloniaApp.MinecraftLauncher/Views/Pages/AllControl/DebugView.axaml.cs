using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Debug;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class DebugView : UserControl
{
    public DebugView()
    {
        InitializeComponent();
        this.IsVisible = false;
        var fps = new Fps();
        fps.OnUpdate = l =>
        {
            FPSBox.Text = $"FPS：{l:0}";
            if (!Config.MainConfig.IsDebug)
            {
                this.IsVisible = false;
            }
            else
            {
                this.IsVisible = true;
            }
        };
    }
}