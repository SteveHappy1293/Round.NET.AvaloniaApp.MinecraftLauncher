using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using FluentAvalonia.FluentIcons;
using RMCL.Controls.Container;
using RMCL.Controls.ControlHelper.Wizard;
using RMCL.Core.Views.Pages.WizardPages;

namespace RMCL.Core.Views.Windows.Initialize;

public partial class InitializeWindow : Window
{
    public InitializeWindow()
    {
        InitializeComponent();
        
        
        RenderOptions.SetTextRenderingMode(this, TextRenderingMode.SubpixelAntialias); // 字体渲染模式
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.MediumQuality); // 图片渲染模式
        RenderOptions.SetEdgeMode(this, EdgeMode.Antialias); // 形状渲染模式
        
        Core.Models.Classes.Core.FluentAvaloniaTheme.PreferUserAccentColor = true;
        var GameFolder = new WizardGameFolder();
        GameFolder.ParentWindow = this;
        
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardWelcome(),
            Title = "欢迎"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardPlayer(),
            Title = "创建新账户"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = GameFolder,
            Title = "新增游戏目录"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardJava(),
            Title = "全局 Java 设置"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardFinish(),
            Title = "完成"
        });
        
        MainWizardFrame.SelectWizardItem(0);
    }
        
    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
        
    private void TitleBar_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
        
    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private bool IsFinish { get; set; } = false;
    private void NextBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!IsFinish)
        {
            var sta = MainWizardFrame.Next();
            LastBtn.IsEnabled = true;
            if (!sta)
            {
                NextBtn.Content = "完成";
                NextBtn.Icon = FluentIconSymbol.Checkmark20Regular;
                IsFinish = true;
            }
        }
        else
        {
            Config.Config.MainConfig.FirstLauncher = false;
            Config.Config.SaveConfig();
            string executablePath = Environment.ProcessPath!;

            // 启动新进程
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = true
            };

            Process.Start(startInfo);

            // 关闭当前进程
            Environment.Exit(0);
        }
    }

    private void LastBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var sta = MainWizardFrame.Last();
        IsFinish = false;
        NextBtn.IsEnabled = true;
        NextBtn.Content = "下一步";
        NextBtn.Icon = FluentIconSymbol.ArrowRight20Regular;
        if (!sta)
        {
            NextBtn.IsEnabled = true;
            LastBtn.IsEnabled = false;
        }
    }
}