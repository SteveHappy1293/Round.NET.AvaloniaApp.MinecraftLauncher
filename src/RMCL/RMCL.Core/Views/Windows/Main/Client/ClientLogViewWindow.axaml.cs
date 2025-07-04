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
using Avalonia.Threading;

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
        public void AddLog(string message)
        {
            if(string.IsNullOrEmpty(message)) return;
            
            var logItem = new TextBlock
            {
                Text = $"{message}",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 5)
            };
            
            logItem.Foreground = Avalonia.Media.Brushes.White;
            if(message.Contains("ERROR")) logItem.Foreground = Avalonia.Media.Brushes.Red;
            if(message.Contains("WARN")) logItem.Foreground = Avalonia.Media.Brushes.Yellow;
            if(message.Contains("DEBUG")) logItem.Foreground = Avalonia.Media.Brushes.Gray;
            
            Dispatcher.UIThread.Post(() =>
            {
                LogPanel.Children.Add(logItem);
                
                // 如果启用了自动滚动，滚动到底部
                if (_autoScroll)
                {
                    var scrollViewer = this.FindControl<ScrollViewer>("LogScrollViewer");
                    scrollViewer.ScrollToEnd();
                }
            });
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
                catch (Exception ex)
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
        }

        private void ExitGameBtn_OnClick(object? sender, RoutedEventArgs e)
        {
            GameProcess.Kill(true);
        }
    }
}