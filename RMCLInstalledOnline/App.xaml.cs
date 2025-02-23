using System.Configuration;
using System.Data;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace RMCLInstalledOnline;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 获取启动参数
        string[] args = e.Args;

        // 遍历参数并处理
        foreach (string arg in args)
        {
            Console.WriteLine($"参数: {arg}");
        }

        var win = new MainWindow();
        // 根据参数决定是否启动主窗口
        if (args.Length > 0)
        {
            win.Visibility = Visibility.Hidden;
            win.TaskbarIcon.ShowBalloonTip("升级 RMCL", "RMCL 升级已在后台开启",BalloonIcon.Info);
            win.ShowDialog();
        }
        else
        {
            win.ShowDialog();
        }   
    }
}