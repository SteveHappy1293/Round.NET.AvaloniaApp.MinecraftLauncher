using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using FluentAvalonia.FluentIcons;
using RMCL.Core.Models.Classes.ExceptionMessage;
using RMCL.Modules.Entry;
using RMCL.Modules.Enum;

namespace RMCL.Core.Views.Windows.Main.Exception;

public partial class ExceptionReportWindow : Window
{
    private ExceptionEntry _currentException;
    private string _fullExceptionText;

    public ExceptionReportWindow()
    {
        InitializeComponent();
        InitializeWindow();
    }

    private void InitializeWindow()
    {
        // 设置窗口属性
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.CanResize = true;

        // 添加键盘快捷键
        this.KeyDown += OnKeyDown;

        // 监听主题变化
        this.ActualThemeVariantChanged += OnThemeChanged;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        // 主题变化时更新UI
        if (_currentException != null)
        {
            UpdateExceptionLevelBadge(_currentException.ExceptionType);
            GenerateSuggestions(_currentException);
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
        else if (e.Key == Key.F5)
        {
            RefreshExceptionData();
        }
        else if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.C)
        {
            CopyAllInformation();
        }
    }

    public void ShowException(ExceptionEntry ex)
    {
        _currentException = ex;
        UpdateWindowTitle(ex);
        PopulateExceptionData(ex);
        GenerateSuggestions(ex);
    }

    private void UpdateWindowTitle(ExceptionEntry ex)
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown";
        var debugMode = "";

#if DEBUG
        debugMode = " [Debug]";
#endif

        Title = $"RMCL {version}{debugMode} - 异常报告 ({ex.RecordTime:yyyy-MM-dd HH:mm:ss})";
    }

    private void PopulateExceptionData(ExceptionEntry ex)
    {
        // 更新异常标题和时间
        ExceptionTitleText.Text = $"{ex.ExceptionName} 异常";
        ExceptionTimeText.Text = $"发生时间：{ex.RecordTime:yyyy年MM月dd日 HH:mm:ss}";

        // 更新异常级别徽章
        UpdateExceptionLevelBadge(ex.ExceptionType);
        ExceptionTypeText.Text = ex.ExceptionName;

        // 更新异常消息
        ExceptionMessageText.Text = string.IsNullOrEmpty(ex.Exception) ? "无异常消息" : ex.Exception;

        // 更新堆栈跟踪
        StackTraceText.Text = string.IsNullOrEmpty(ex.StackTrace) ? "无堆栈跟踪信息" : ex.StackTrace;

        // 更新系统信息
        SystemVersionText.Text = ex.SystemVersion ?? "未知";
        SystemLanguageText.Text = ex.SystemLanguage ?? "未知";
        SystemTimeZoneText.Text = ex.SystemTimeZone ?? "未知";
        RMCLVersionText.Text = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "未知";
        RuntimeVersionText.Text = Environment.Version.ToString();
        ExceptionSourceText.Text = ex.ExceptionSource ?? "未知";

        // 更新配置信息
        UpdateConfigurationInfo();

        // 生成完整异常文本
        GenerateFullExceptionText(ex);
    }

    private void UpdateExceptionLevelBadge(ExceptionEnum level)
    {
        var levelText = ExceptionMessage.GetExceptionLevelText(level);

        // 更新Border内的TextBlock文本
        if (ExceptionLevelBadge.Child is TextBlock textBlock)
        {
            textBlock.Text = levelText;
        }

        // 清除所有级别类
        ExceptionLevelBadge.Classes.Clear();
        ExceptionLevelBadge.Classes.Add("exception-level");

        // 添加对应的级别类
        switch (level)
        {
            case ExceptionEnum.Critical:
                ExceptionLevelBadge.Classes.Add("critical");
                break;
            case ExceptionEnum.Error:
                ExceptionLevelBadge.Classes.Add("error");
                break;
            case ExceptionEnum.Warning:
                ExceptionLevelBadge.Classes.Add("warning");
                break;
            case ExceptionEnum.Information:
                ExceptionLevelBadge.Classes.Add("information");
                break;
        }
    }

    private void UpdateConfigurationInfo()
    {
        try
        {
            var configInfo = new StringBuilder();
            configInfo.AppendLine("=== RMCL 配置信息 ===");

            // 尝试获取配置信息（如果Config类可用）
            try
            {
                // 这里需要根据实际的Config类结构来获取配置信息
                configInfo.AppendLine($"首次启动: {Config.Config.MainConfig.FirstLauncher}");
                configInfo.AppendLine($"主题模式: {Config.Config.MainConfig.ThemeColors.Theme}");
                configInfo.AppendLine($"颜色类型: {Config.Config.MainConfig.ThemeColors.ColorType}");
                configInfo.AppendLine($"背景模式: {Config.Config.MainConfig.Background.ChooseModel}");
                configInfo.AppendLine($"下载线程数: {Config.Config.MainConfig.DownloadThreads}");
            }
            catch (System.Exception ex)
            {
                configInfo.AppendLine($"无法获取配置信息: {ex.Message}");
            }

            // 添加环境信息
            configInfo.AppendLine();
            configInfo.AppendLine("=== 环境信息 ===");
            configInfo.AppendLine($"工作目录: {Environment.CurrentDirectory}");
            configInfo.AppendLine($"64位进程: {Environment.Is64BitProcess}");
            configInfo.AppendLine($"64位操作系统: {Environment.Is64BitOperatingSystem}");
            configInfo.AppendLine($"处理器数量: {Environment.ProcessorCount}");
            configInfo.AppendLine($"系统页面大小: {Environment.SystemPageSize}");

            ConfigurationText.Text = configInfo.ToString();
        }
        catch (System.Exception ex)
        {
            ConfigurationText.Text = $"获取配置信息时发生错误: {ex.Message}";
        }
    }

    private void GenerateFullExceptionText(ExceptionEntry ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== RMCL 异常报告 ===");
        sb.AppendLine($"时间: {ex.RecordTime:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"异常类型: {ex.ExceptionName}");
        sb.AppendLine($"异常级别: {ExceptionMessage.GetExceptionLevelText(ex.ExceptionType)}");
        sb.AppendLine($"RMCL版本: {Assembly.GetEntryAssembly()?.GetName().Version}");
        sb.AppendLine();

        sb.AppendLine("=== 异常消息 ===");
        sb.AppendLine(ex.Exception ?? "无");
        sb.AppendLine();

        sb.AppendLine("=== 堆栈跟踪 ===");
        sb.AppendLine(ex.StackTrace ?? "无");
        sb.AppendLine();

        sb.AppendLine("=== 系统信息 ===");
        sb.AppendLine($"操作系统: {ex.SystemVersion}");
        sb.AppendLine($"系统语言: {ex.SystemLanguage}");
        sb.AppendLine($"时区: {ex.SystemTimeZone}");
        sb.AppendLine($"运行时: {Environment.Version}");
        sb.AppendLine($"异常源: {ex.ExceptionSource}");

        _fullExceptionText = sb.ToString();
    }

    private void GenerateSuggestions(ExceptionEntry ex)
    {
        SuggestionsPanel.Children.Clear();

        var suggestions = GetSuggestionsForException(ex);

        if (suggestions.Count == 0)
        {
            SuggestionsCard.IsVisible = false;
            return;
        }

        SuggestionsCard.IsVisible = true;

        foreach (var suggestion in suggestions)
        {
            var suggestionItem = CreateSuggestionItem(suggestion);
            SuggestionsPanel.Children.Add(suggestionItem);
        }
    }

    private List<string> GetSuggestionsForException(ExceptionEntry ex)
    {
        var suggestions = new List<string>();

        switch (ex.ExceptionName)
        {
            case "NullReferenceException":
                suggestions.Add("检查代码中是否有未初始化的对象引用");
                suggestions.Add("确保在使用对象前进行空值检查");
                suggestions.Add("重启应用程序可能会解决临时问题");
                break;

            case "FileNotFoundException":
                suggestions.Add("检查相关文件是否存在");
                suggestions.Add("验证文件路径是否正确");
                suggestions.Add("重新安装或修复RMCL可能会恢复缺失的文件");
                break;

            case "UnauthorizedAccessException":
                suggestions.Add("以管理员身份运行RMCL");
                suggestions.Add("检查文件和文件夹的访问权限");
                suggestions.Add("确保防病毒软件没有阻止RMCL访问文件");
                break;

            case "OutOfMemoryException":
                suggestions.Add("关闭其他占用内存的应用程序");
                suggestions.Add("重启计算机以释放内存");
                suggestions.Add("检查是否有内存泄漏问题");
                break;

            case "TimeoutException":
                suggestions.Add("检查网络连接是否正常");
                suggestions.Add("稍后重试操作");
                suggestions.Add("检查防火墙设置是否阻止了连接");
                break;

            default:
                if (ex.ExceptionType == ExceptionEnum.Critical)
                {
                    suggestions.Add("这是一个严重错误，建议重启应用程序");
                    suggestions.Add("如果问题持续存在，请联系开发者");
                }
                else
                {
                    suggestions.Add("尝试重新执行导致错误的操作");
                    suggestions.Add("重启应用程序可能会解决问题");
                }
                break;
        }

        // 通用建议
        suggestions.Add("如果问题持续存在，请在GitHub上报告此问题");

        return suggestions;
    }

    private Control CreateSuggestionItem(string suggestion)
    {
        var border = new Border
        {
            Classes = { "suggestion-item" }
        };

        var panel = new DockPanel();

        var icon = new FluentAvalonia.FluentIcons.FluentIcon
        {
            Icon = FluentIconSymbol.Lightbulb24Regular,
            Width = 16,
            Height = 16,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top,
            Margin = new Thickness(0, 2, 8, 0)
        };

        var textBlock = new TextBlock
        {
            Text = suggestion,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 13
        };

        DockPanel.SetDock(icon, Dock.Left);
        panel.Children.Add(icon);
        panel.Children.Add(textBlock);

        border.Child = panel;
        return border;
    }

    private void RefreshExceptionData()
    {
        if (_currentException != null)
        {
            PopulateExceptionData(_currentException);
            GenerateSuggestions(_currentException);
        }
    }

    // 事件处理方法
    private async void CopyMessageButton_Click(object sender, RoutedEventArgs e)
    {
        await CopyToClipboard(ExceptionMessageText.Text ?? "");
        ShowCopyNotification("异常消息已复制到剪贴板");
    }

    private async void CopyStackTraceButton_Click(object sender, RoutedEventArgs e)
    {
        await CopyToClipboard(StackTraceText.Text ?? "");
        ShowCopyNotification("堆栈跟踪已复制到剪贴板");
    }

    private async void CopySystemInfoButton_Click(object sender, RoutedEventArgs e)
    {
        var systemInfo = GenerateSystemInfoText();
        await CopyToClipboard(systemInfo);
        ShowCopyNotification("系统信息已复制到剪贴板");
    }

    private async void CopyConfigButton_Click(object sender, RoutedEventArgs e)
    {
        await CopyToClipboard(ConfigurationText.Text ?? "");
        ShowCopyNotification("配置信息已复制到剪贴板");
    }

    private async void CopyAllButton_Click(object sender, RoutedEventArgs e)
    {
        await CopyAllInformation();
    }

    private async void ExportReportButton_Click(object sender, RoutedEventArgs e)
    {
        await ExportFullReport();
    }

    private void OpenGitHubButton_Click(object sender, RoutedEventArgs e)
    {
        OpenGitHubIssue();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // 辅助方法
    private async Task CopyToClipboard(string text)
    {
        try
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            if (clipboard != null)
            {
                await clipboard.SetTextAsync(text);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"复制到剪贴板失败: {ex.Message}");
        }
    }

    private async Task CopyAllInformation()
    {
        await CopyToClipboard(_fullExceptionText);
        ShowCopyNotification("所有异常信息已复制到剪贴板");
    }

    private string GenerateSystemInfoText()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== 系统信息 ===");
        sb.AppendLine($"操作系统: {SystemVersionText.Text}");
        sb.AppendLine($"系统语言: {SystemLanguageText.Text}");
        sb.AppendLine($"时区: {SystemTimeZoneText.Text}");
        sb.AppendLine($"RMCL版本: {RMCLVersionText.Text}");
        sb.AppendLine($"运行时: {RuntimeVersionText.Text}");
        sb.AppendLine($"异常源: {ExceptionSourceText.Text}");
        return sb.ToString();
    }

    private async Task ExportFullReport()
    {
        try
        {
            var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
            if (storageProvider == null) return;

            var fileTypeChoices = new[]
            {
                new FilePickerFileType("文本文件")
                {
                    Patterns = new[] { "*.txt" }
                },
                new FilePickerFileType("所有文件")
                {
                    Patterns = new[] { "*.*" }
                }
            };

            var result = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "导出异常报告",
                DefaultExtension = "txt",
                FileTypeChoices = fileTypeChoices,
                SuggestedFileName = $"RMCL_Exception_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            });

            if (result != null)
            {
                await using var stream = await result.OpenWriteAsync();
                await using var writer = new StreamWriter(stream, Encoding.UTF8);
                await writer.WriteAsync(_fullExceptionText);
                ShowCopyNotification($"报告已导出到: {result.Name}");
            }
        }
        catch (System.Exception ex)
        {
            ShowCopyNotification($"导出失败: {ex.Message}");
        }
    }

    private void OpenGitHubIssue()
    {
        try
        {
            var issueTitle = Uri.EscapeDataString($"[异常报告] {_currentException?.ExceptionName} - {DateTime.Now:yyyy-MM-dd}");
            var issueBody = Uri.EscapeDataString($"## 异常信息\n\n```\n{_fullExceptionText}\n```\n\n## 重现步骤\n请描述导致此异常的操作步骤...\n\n## 预期行为\n请描述您期望的正常行为...");

            var url = $"https://github.com/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/issues/new?title={issueTitle}&body={issueBody}";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (System.Exception ex)
        {
            ShowCopyNotification($"无法打开GitHub: {ex.Message}");
        }
    }


    private void ShowCopyNotification(string message)
    {
        // 这里可以显示一个临时的通知消息
        // 由于没有现成的通知系统，我们暂时使用控制台输出
        Console.WriteLine($"通知: {message}");

        // 可以考虑添加一个临时的文本块来显示通知
        // 或者集成到现有的通知系统中
    }
}