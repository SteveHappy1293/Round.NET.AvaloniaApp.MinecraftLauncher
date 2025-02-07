using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

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
                    SmTitle.Content =
                        $"{Path.GetFileName(Directory.GetDirectories($"{Config.MainConfig.GameFolders[Sel].Path}/versions")[Config.MainConfig.GameFolders[Sel].SelectedGameIndex])}";

                });
                Thread.Sleep(100);
            }
        });
        Task.Run(() =>
        {
            Thread.Sleep(800);
            Dispatcher.UIThread.Invoke(() =>
                LaunchBored.Margin = new Thickness(0));
            Dispatcher.UIThread.Invoke(() =>
                LaunchBored.Opacity = 1);
        });
        // foreach (var user in Config.MainConfig.Users)
        // {
        //     UserButton.Items.Add(new ComboBoxItem()
        //     {
        //         Content = new UserShowControl(user.Name),
        //         VerticalContentAlignment = VerticalAlignment.Center
        //     });
        // }
        //UserButton.SelectedIndex = Config.MainConfig.SelectedUser;
        IsEdit = true;
    }

    private bool IsEdit = false;
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

    private void UserButton_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            // Config.MainConfig.SelectedUser = UserButton.SelectedIndex;
            Config.SaveConfig();
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        ((MainView)Core.MainWindow.Content).SystemNavigationBar.Show();
    }
}