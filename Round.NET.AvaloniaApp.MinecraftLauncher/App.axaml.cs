using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HotAvalonia;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Windows;

namespace Round.NET.AvaloniaApp.MinecraftLauncher;

public partial class App : Application
{
    public override void Initialize()
    {
        this.EnableHotReload();
        AvaloniaXamlLoader.Load(this);
        
        // 订阅所有全局异常处理器
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        Dispatcher.UIThread.UnhandledException += UIThread_UnhandledException;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void UIThread_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogException(e.Exception, "UI Thread");
        ShowErrorDialog(e.Exception);
        e.Handled = true; // 阻止应用崩溃
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;
        LogException(exception, "AppDomain");
        ShowErrorDialog(exception);
        
        // 非致命错误可以继续运行
        if (!e.IsTerminating)
        {
            return;
        }
        
        // 致命错误需要特殊处理
        HandleFatalError(exception);
    }

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        LogException(e.Exception, "Task");
        ShowErrorDialog(e.Exception);
        e.SetObserved(); // 标记为已处理
    }

    private void ShowErrorDialog(Exception ex)
    {
        try
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                var error = new ErrorWindow();
                error.ShowEx(ex);
                Message.Show("发生错误","RMCL 发生了致命的错误，已将其信息保存至 异常追踪器。可前往 设置 > 安全 > 异常追踪 页面查看。",InfoBarSeverity.Error);
                
                if (Config.MainConfig.ShowErrorWindow)
                {
                    error.ShowDialog(Core.MainWindow);
                }
            });
        }
        catch (Exception dialogEx)
        {
            // 如果对话框本身出错，至少记录到控制台
            Console.WriteLine($"显示错误对话框时出错: {dialogEx}");
        }
    }

    private void LogException(Exception ex, string source)
    {
        Console.WriteLine($"[{DateTime.Now}] [{source}] 异常: {ex?.ToString() ?? "未知异常"}");
        // 这里可以添加文件日志记录
    }

    private void HandleFatalError(Exception ex)
    {
        LogException(ex, "FATAL");
        // 尝试保存重要数据
        // ...
        
        // 优雅退出
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(1);
        }
        else
        {
            Environment.Exit(1);
        }
    }
}