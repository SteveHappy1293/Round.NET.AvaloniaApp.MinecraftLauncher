using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Modules.Entry;

namespace RMCL.Controls.Item.Safe;

public partial class ExceptionItem : UserControl
{
    public EventHandler OnOpen { get; set; } = (e,s) => { };
    public ExceptionItem()
    {
        InitializeComponent();
    }

    public void ShowLoad(ExceptionEntry info)
    {
        ExceptionName.Text = info.RecordTime.ToString();
        ExceptionType.Text = info.ExceptionName;
        ExceptionLevel.Text = $"异常等级：{info.ExceptionType.GetHashCode()}";
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OnOpen.Invoke(this,null);
    }
}