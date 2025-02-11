using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        User.LoadUser();
        FindJava.LoadJava();
        Config.LoadConfig();
        
        InitializeComponent();
        PlugsLoader.LoadingPlug();
        TitleBar.Height = 38;
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        RenderOptions.SetTextRenderingMode(this, TextRenderingMode.SubpixelAntialias); // 字体渲染模式
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.MediumQuality); // 图片渲染模式
        RenderOptions.SetEdgeMode(this, EdgeMode.Antialias); // 形状渲染模式
        
        Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.Minecraft"));
        Core.MainWindow = this;
        StyleMange.Load();

        if (!Config.MainConfig.WindowSize.IsEmpty)
        {
            this.Width = Config.MainConfig.WindowSize.Width;
            this.Height = Config.MainConfig.WindowSize.Height;
            Console.WriteLine($"Window size: {Config.MainConfig.WindowSize}");
        }
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        Config.MainConfig.WindowSize = new Size((int)this.Bounds.Width, (int)this.Bounds.Height);
        Config.SaveConfig();
    }
}
