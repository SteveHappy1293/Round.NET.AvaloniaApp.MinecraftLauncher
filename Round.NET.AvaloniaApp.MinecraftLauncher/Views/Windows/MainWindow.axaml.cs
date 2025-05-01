using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Rendering;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using MinecraftLaunch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Mange.StarMange;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.NetWork.Organization;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.RSAccount;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Initialize;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using FileDialog = Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Dialog.FileDialog;
using ServerMange = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Server.ServerMange;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Core.MainWindow = this;
        User.LoadUser();
        MinecraftLauncher.Modules.Java.FindJava.JavasList.Clear();
        Config.LoadConfig();
        StarGroup.LoadStars();
        ServerMange.Load();

        // if (Config.MainConfig.IsUseOrganizationConfig) OrganizationCore.LoadOrganizationConfig();
        DownloadMirrorManager.MaxThread = Config.MainConfig.DownloadThreads;
        DownloadMirrorManager.IsEnableMirror = false;

        InitializeComponent();
        // RendererDiagnostics.DebugOverlays ^= RendererDebugOverlays.Fps;
        // PlugsLoader.LoadingPlug();

        RenderOptions.SetTextRenderingMode(this, TextRenderingMode.SubpixelAntialias); // 字体渲染模式
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.MediumQuality); // 图片渲染模式
        RenderOptions.SetEdgeMode(this, EdgeMode.Antialias); // 形状渲染模式

        Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.Minecraft"));

        this.Width = Config.MainConfig.WindowWidth;
        this.Height = Config.MainConfig.WindowHeight;
        if (Config.MainConfig.WindowX != 0 && Config.MainConfig.WindowY != 0)
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Position = new PixelPoint(Config.MainConfig.WindowX, Config.MainConfig.WindowY);
        }
        try
        {
            StyleMange.Load(this);
        }
        catch (Exception ex)
        {
            Message.Show("主题加载", $"主题加载错误！\n{ex}", InfoBarSeverity.Error);
        }
        PlugLoaderNeo.LoadPlugs();
        if (PlugLoaderNeo.Plugs.Count > 0)
            Message.Show("插件加载", $"当前已加载 {PlugLoaderNeo.Plugs.Count} 个插件！", InfoBarSeverity.Success);
        if (Config.MainConfig.IsAutoUpdate)
        {
            Task.Run(() =>
            {
                var downloader = new Updater((v, s) =>
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    // 获取程序集的版本信息
                    Version version = assembly.GetName().Version;
                    if (v.Replace("v", "").Replace("0", "").Replace(".", "") !=
                        version.ToString().Replace(".", "").Replace("0", ""))
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            var con = new ContentDialog()
                            {
                                PrimaryButtonText = "取消",
                                CloseButtonText = "现在更新",
                                Title = $"更新 RMCL3 - {v.Replace("0", "")}",
                                DefaultButton = ContentDialogButton.Close,
                                Content = new StackPanel()
                                {
                                    Children =
                                    {
                                        new Label()
                                        {
                                            Content = "你好！打扰一下~\nRMCL当前有个更新，需要花费您一些时间，请问您是否更新？"
                                        },
                                        new Label()
                                        {
                                            Content = $"当前版本：v{version.ToString().Replace(".", "").Replace("0", "")}"
                                        },
                                        new Label()
                                        {
                                            Content = $"更新版本：{v.Replace(".", "").Replace("0", "")}"
                                        }
                                    }
                                }
                            };
                            con.CloseButtonClick += (_, __) =>
                            {
                                var dow = new DownloadUpdate();
                                dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                                dow.URL = s;
                                dow.Version = v.Replace(".", "").Replace("0", "");
                                dow.Download();
                            };
                            con.ShowAsync(this);
                        });
                    }
                });
                downloader.GetDownloadUrlAsync(
                    "https://api.github.com/repos/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/releases");
            });
        }
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        Config.MainConfig.WindowWidth = (int)this.Bounds.Width;
        Config.MainConfig.WindowHeight = (int)this.Bounds.Height;
        Config.MainConfig.WindowX = (int)this.Position.X;
        Config.MainConfig.WindowY = (int)this.Position.Y;
        Config.SaveConfig();
    }

    public Grid GetDialogGrid()
    {
        return dialog;
    }
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        var btn = (Button)sender;
        btn.Content = WindowState == WindowState.Maximized
            ? new PathIcon()
            {
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 3, 0, 0),
                Data = StreamGeometry.Parse(
                    "M832 704H704v128c0 70.692-57.308 128-128 128H192c-70.692 0-128-57.308-128-128V448c0-70.692 57.308-128 128-128h128V192c0-70.692 57.308-128 128-128h384c70.692 0 128 57.308 128 128v384c0 70.692-57.308 128-128 128zM192 384c-35.346 0-64 28.654-64 64v384c0 35.346 28.654 64 64 64h384c35.346 0 64-28.654 64-64V448c0-35.346-28.654-64-64-64H192z m704-192c0-35.346-28.654-64-64-64H448c-35.346 0-64 28.654-64 64v128h192c70.692 0 128 57.308 128 128v192h128c35.346 0 64-28.654 64-64V192z")
            }
            : new PathIcon()
            {
                Margin = new Thickness(0, 1, 0, 0),
                Width = 16,
                Height = 16,
                Data = StreamGeometry.Parse(
                    "M233.301333 128A105.301333 105.301333 0 0 0 128 233.301333v557.397334A105.301333 105.301333 0 0 0 233.301333 896h557.397334A105.301333 105.301333 0 0 0 896 790.698667V233.301333A105.301333 105.301333 0 0 0 790.698667 128H233.301333z m-18.602666 105.301333c0-10.24 8.32-18.602667 18.602666-18.602666h557.397334c10.24 0 18.602667 8.32 18.602666 18.602666v557.397334c0 10.24-8.32 18.602667-18.602666 18.602666H233.301333a18.56 18.56 0 0 1-18.602666-18.602666V233.301333z")
            };
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    public void ChangeMenuItems(List<MenuItem> menuItems)
    {
        Menu.Margin = new Thickness(20, 0, -20, 0);
        Menu.Opacity = 0;
        Task.Run(() =>
        {
            Thread.Sleep(120);
            Dispatcher.UIThread.Invoke((() =>Menu.Margin = new Thickness(-20, 0, 20, 0)));
            Thread.Sleep(150);
            Dispatcher.UIThread.Invoke(() =>
            {
                Menu.ItemsSource = menuItems;
                Menu.Margin = new Thickness(0, 0, 0, 0);
                Menu.Opacity = 1;
            });
        });
    }

    // 标题栏拖拽逻辑
    private void TitleBar_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        var btn = Maxbtn;
        btn.Content = WindowState == WindowState.Maximized
            ? new PathIcon()
            {
                Width = 12,
                Height = 12,
                Margin = new Thickness(0, 3, 0, 0),
                Data = StreamGeometry.Parse(
                    "M832 704H704v128c0 70.692-57.308 128-128 128H192c-70.692 0-128-57.308-128-128V448c0-70.692 57.308-128 128-128h128V192c0-70.692 57.308-128 128-128h384c70.692 0 128 57.308 128 128v384c0 70.692-57.308 128-128 128zM192 384c-35.346 0-64 28.654-64 64v384c0 35.346 28.654 64 64 64h384c35.346 0 64-28.654 64-64V448c0-35.346-28.654-64-64-64H192z m704-192c0-35.346-28.654-64-64-64H448c-35.346 0-64 28.654-64 64v128h192c70.692 0 128 57.308 128 128v192h128c35.346 0 64-28.654 64-64V192z")
            }
            : new PathIcon()
            {
                Margin = new Thickness(0, 1, 0, 0),
                Width = 12,
                Height = 12,
                Data = StreamGeometry.Parse(
                    "M233.301333 128A105.301333 105.301333 0 0 0 128 233.301333v557.397334A105.301333 105.301333 0 0 0 233.301333 896h557.397334A105.301333 105.301333 0 0 0 896 790.698667V233.301333A105.301333 105.301333 0 0 0 790.698667 128H233.301333z m-18.602666 105.301333c0-10.24 8.32-18.602667 18.602666-18.602666h557.397334c10.24 0 18.602667 8.32 18.602666 18.602666v557.397334c0 10.24-8.32 18.602667-18.602666 18.602666H233.301333a18.56 18.56 0 0 1-18.602666-18.602666V233.301333z")
            };
    }
    

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {

    }

    private void TaskViewButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.SystemTask.Show();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.BottomBar.NavigationTo("Launch");
    }
}

