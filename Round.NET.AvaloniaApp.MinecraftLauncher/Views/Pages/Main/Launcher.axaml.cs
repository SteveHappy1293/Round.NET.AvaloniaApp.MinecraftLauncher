using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.TaskMange.SystemMessage;

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
    }

    private void MessageBox_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.SystemTask.Show();
    }
}