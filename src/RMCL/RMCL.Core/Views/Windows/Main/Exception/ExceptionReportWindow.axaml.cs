using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Modules.Entry;

namespace RMCL.Core.Views.Windows.Main.Exception;

public partial class ExceptionReportWindow : Window
{
    public ExceptionReportWindow()
    {
        InitializeComponent();
    }

    public void ShowException(ExceptionEntry ex)
    {
        var wintitle = "[RMCL 3] {DEBUG} Version {VERSION} 异常报告详细信息 {EXCEPTION_TIME}";
        wintitle = wintitle.Replace("{VERSION}", Assembly.GetEntryAssembly()?.GetName().Version.ToString());
        
#if DEBUG
        wintitle = wintitle.Replace("{DEBUG}", "[Debug 模式]");
#else
        wintitle = wintitle.Replace("{DEBUG}", "");
#endif
        
        wintitle = wintitle.Replace("{EXCEPTION_TIME}", ex.RecordTime.ToString());
        Title = wintitle;
    }
}