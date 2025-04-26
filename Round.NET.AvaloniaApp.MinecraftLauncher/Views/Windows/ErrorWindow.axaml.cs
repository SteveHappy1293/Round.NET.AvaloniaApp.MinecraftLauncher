using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.ExceptionMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Windows;

public partial class ErrorWindow : Window
{
    public ExceptionEntry ExceptionEntry { get; set; } = new();
    public ErrorWindow()
    {
        InitializeComponent();
        TimeShow.Content = DateTime.Now.ToString();
    }

    public void ShowEx(Exception ex,DateTime time)
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        var sysstr = new SequenceString();
        sysstr.Add(new() { Title = "操作系统版本", Text = Environment.OSVersion.ToString() });
        sysstr.Add(new() { Title = "64位操作系统", Text = Environment.Is64BitOperatingSystem.ToString() });
        sysstr.Add(new() { Title = "系统语言", Text = CultureInfo.InstalledUICulture.Name });
        sysstr.Add(new() { Title = "时区", Text = localTimeZone.DisplayName });
        
        var constr = new SequenceString();
        constr.Add(new() { Title = "背景显示模式", Text = Config.MainConfig.BackModlue.ToString() });
        constr.Add(new() { Title = "虚拟机内存", Text = Config.MainConfig.JavaUseMemory.ToString() });
        constr.Add(new() { Title = "最大下载线程", Text = Config.MainConfig.DownloadThreads.ToString() });

        ExceptionEntry.Exception = ex.Message;
        ExceptionEntry.ExceptionSource = ex.Source;
        ExceptionEntry.StackTrace = ex.StackTrace;
        ExceptionEntry.RecordTime = time;
        ExceptionEntry.SystemLanguage = CultureInfo.InstalledUICulture.Name;
        ExceptionEntry.SystemVersion = Environment.OSVersion.ToString();
        ExceptionEntry.SystemTimeZone = localTimeZone.DisplayName;
        ExceptionEntry.ExceptionType = ExceptionMessage.GetExceptionSeverity(ex);

        ExceptionMessage.LogException(ExceptionEntry);
        ErrorTypeLabel.Content = ex.GetType().Name;
        StackMessage.Text = ex.ToString();
        ErrorMessage.Text = ex.Message;
        ConfigMessage.Text = constr.GetResult();
        SystemMessage.Text = sysstr.GetResult();
    }

    private void ResetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var con = new ContentDialog()
        {
            Title = "重置",
            Content = "你确定要重置 RMCL 吗？\n如果你重置，则全部配置将消失！",
            CloseButtonText = "取消",
            PrimaryButtonText = "确定",
            DefaultButton = ContentDialogButton.Close
        };
        con.PrimaryButtonClick += (_, _) =>
        {
            try
            {
                var dir = Path.GetFullPath("../RMCL/");
                Directory.Delete(dir, true);
            }catch{ }

            Thread.Sleep(100);
            Environment.Exit(0);
        };
        con.ShowAsync();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}