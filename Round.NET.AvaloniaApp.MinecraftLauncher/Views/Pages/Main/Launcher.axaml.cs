using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Launcher : UserControl
{
    public Launcher()
    {
        InitializeComponent();
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (MessageBox.Width != 300)
                    {
                        MessageBox.Width = 300;
                        MessageBox.Description = SystemMessageTaskMange.Message;
                    }
                    else
                    {
                        MessageBox.Width = 280;   
                        MessageBox.Description = "全新一代 RMCL，不太一样，大不一样。";
                    }
                });
                Thread.Sleep(5000);
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var Sel = Config.MainConfig.SelectedGameFolder;
                    // if (Sel != Config.MainConfig.SelectedGameFolder)
                    // {
                    //     SmTitle.Content = "请选择游戏版本";
                    //     Sel = Config.MainConfig.SelectedGameFolder;
                    // }
                    // else
                    // {
                    //     Sel = Config.MainConfig.SelectedGameFolder;
                    //     SmTitle.Content =
                    //         $"Minecraft {Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex])}";
                    // }
                    SmTitle.Content =
                        $"Minecraft {Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex])}";

                });
                Thread.Sleep(100);
            }
        });
    }

    private void MessageBox_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.SystemTask.Show();
    }

    private void LaunchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var Sel = Config.MainConfig.SelectedGameFolder;
        var dow = new LaunchJavaEdtion();
        dow.Version = Path.GetFileName(Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex]));
        SystemMessageTaskMange.AddTask(dow, SystemMessageTaskMange.TaskType.Launch);
    }
}