using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Item.Safe;

public partial class LogItem : UserControl
{
    public LogItem()
    {
        InitializeComponent();
    }

    public EventHandler<string> OnOpen { get; set; } = (sender, s) => { };
    public EventHandler<string> OnSave { get; set; } = (sender, s) => { };
    public string FileName { get; private set; }

    public void Load(string file, bool isthis = false)
    {
        if (isthis) this.LabelBox.IsVisible = false;
        FileName = file;
        var filename = Path.GetFileName(file);

        string datePart = filename.Substring(14, 10);
        string timePart = filename.Substring(25, 10);

        // 将日期和时间部分转换为DateTime对象
        DateTime dateTime = DateTime.ParseExact(
            $"{datePart} {timePart}",
            "yyyy.MM.dd HHmmss.fff",
            CultureInfo.InvariantCulture
        );

        DataBox.Text = dateTime.ToString();
    }

    private void SaveLogBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        OnSave.Invoke(this, FileName);
    }

    private void OpenLogBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        OnOpen.Invoke(this, FileName);
    }
}