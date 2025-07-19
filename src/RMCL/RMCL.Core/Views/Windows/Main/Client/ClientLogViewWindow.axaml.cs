using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using RMCL.LogAnalyzer.Minecraft;

namespace RMCL.Core.Views.Windows.Main.Client
{
    public partial class ClientLogViewWindow : Window
    {
        private bool _autoScroll = true;
        private bool _alwaysOnTop;

        
        public Process GameProcess;
        public ClientLogViewWindow()
        {
            InitializeComponent();
            
            // 初始化复选框状态
            var autoScrollCheckBox = this.FindControl<CheckBox>("AutoScrollCheckBox");
            var alwaysOnTopCheckBox = this.FindControl<CheckBox>("AlwaysOnTopCheckBox");
            
            autoScrollCheckBox.IsChecked = _autoScroll;
            alwaysOnTopCheckBox.IsChecked = _alwaysOnTop;
            
            // 绑定事件
            autoScrollCheckBox.Checked += (s, e) => _autoScroll = true;
            autoScrollCheckBox.Unchecked += (s, e) => _autoScroll = false;
            
            alwaysOnTopCheckBox.Checked += (s, e) => 
            {
                _alwaysOnTop = true;
                Topmost = true;
            };
            alwaysOnTopCheckBox.Unchecked += (s, e) => 
            {
                _alwaysOnTop = false;
                Topmost = false;
            };
            
            // 导出按钮点击事件
            var exportButton = this.FindControl<Button>("Export");
            exportButton.Click += ExportButton_Click;

            ExitGameBtn.IsEnabled = true;
        }
        
        // 添加日志方法
        private (IBrush Foreground, FontWeight FontWeight, FontFamily FontFamily) GetLogStyle(string message)
        {
            // 定义等宽字体（优先使用 Consolas，其次是 Courier New，最后是系统默认的等宽字体）
            var monospaceFont = FontFamily.Parse("Consolas");
            if(!string.IsNullOrEmpty(Config.Config.MainConfig.FontsConfig.ChoseFontName)) monospaceFont = FontFamily.Parse(Config.Config.MainConfig.FontsConfig.ChoseFontName);
        
            if (message.Contains("INFO", StringComparison.OrdinalIgnoreCase))
                return (Brushes.White, FontWeight.SemiBold, monospaceFont);
            
            if (message.Contains("WARN", StringComparison.OrdinalIgnoreCase))
                return (Brushes.Yellow, FontWeight.SemiBold, monospaceFont);
        
            if (message.Contains("DEBUG", StringComparison.OrdinalIgnoreCase))
                return (Brushes.Gray, FontWeight.Normal, monospaceFont);
        
            if (message.Contains("SUCCESS", StringComparison.OrdinalIgnoreCase))
                return (Brushes.LightGreen, FontWeight.SemiBold, monospaceFont);
            
            if (message.Contains("ERROR", StringComparison.OrdinalIgnoreCase) || message.Contains("at ", StringComparison.OrdinalIgnoreCase))
                return (Brushes.Red, FontWeight.Bold, monospaceFont);
            
            return (Brushes.White, FontWeight.Normal, monospaceFont);
        }

// 修改后的 AddLog 方法
        public void AddLog(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            // 获取日志样式（现在包含字体信息）
            var (foreground, fontWeight, fontFamily) = GetLogStyle(message);
    
            var logItem = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 0),
                Foreground = foreground,
                FontWeight = fontWeight,
                FontFamily = fontFamily,  // 设置等宽字体
                FontSize = Config.Config.MainConfig.FontsConfig.FontSize,
            };

            Dispatcher.UIThread.Post(() =>
            {
                try 
                {
                    LogPanel.Children.Add(logItem);
            
                    if (_autoScroll && LogPanel.Parent is ScrollViewer scrollViewer)
                    {
                        scrollViewer.ScrollToEnd();
                    }
                    else if (_autoScroll)
                    {
                        var scrollViewer1 = this.FindControl<ScrollViewer>("LogScrollViewer");
                        scrollViewer1?.ScrollToEnd();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine($"日志添加失败: {ex.Message}");
                }
            }, DispatcherPriority.Background);
        }
        
        // 导出日志
        private async void ExportButton_Click(object? sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "导出日志",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Minecraft 日志文件", Extensions = new List<string> { "log" } },
                },
                DefaultExtension = "log",
                InitialFileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.log"
            };
            
            var result = await saveFileDialog.ShowAsync(this);
            
            if (result != null)
            {
                try
                {
                    var sb = new StringBuilder();
                    foreach (var child in LogPanel.Children.OfType<TextBlock>())
                    {
                        sb.AppendLine(child.Text);
                    }
                    
                    await File.WriteAllTextAsync(result, sb.ToString());
                }
                catch (System.Exception ex)
                {
                    
                }
            }
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

        public void GameExit()
        {
            StatusLabel.Text = "游戏实例已退出";
            StatusLabel.BoxBackground = Brushes.DarkRed;
            ExitGameBtn.IsEnabled = false;
            
            
            var sb = new StringBuilder();
            foreach (var child in LogPanel.Children.OfType<TextBlock>())
            {
                sb.AppendLine(child.Text);
            }
        }

        private void ExitGameBtn_OnClick(object? sender, RoutedEventArgs e)
        {
            GameProcess.Kill(true);
        }

        public void UpdateInfo(ClientRunnerInfo info)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                GameFolder.Text = $"游戏目录：{info.GameInstances.ClientInstances.GameCatalog}";
                GameName.Text = $"游戏名称：{info.GameInstances.ClientInstances.GameName}";
                UserName.Text = $"账户名称：{info.Account.UserName}";
                UserType.Text = $"账户类型：{info.Account.AccountType}";
                JvmName.Text = $"Java 路径：{info.JavaInfo.JavaPath}";
            });
        }
    }
}