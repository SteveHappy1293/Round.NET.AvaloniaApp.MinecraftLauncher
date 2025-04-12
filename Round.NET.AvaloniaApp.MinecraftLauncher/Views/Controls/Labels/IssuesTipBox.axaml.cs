using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Tips;

public class IssuesTipBox : TemplatedControl
{
    public static readonly StyledProperty<IssuesTypeEnum> IssuesTypeProperty =
        AvaloniaProperty.Register<IssuesTipBox, IssuesTypeEnum>(nameof(IssuesType));
    
    public static readonly StyledProperty<IBrush> IssuesColorProperty =
        AvaloniaProperty.Register<IssuesTipBox, IBrush>(nameof(IssuesColor));
    
    public static readonly StyledProperty<IBrush> IssuesFontColorProperty =
        AvaloniaProperty.Register<IssuesTipBox, IBrush>(nameof(IssuesFontColor));
    
    public static readonly StyledProperty<string> IssuesTextProperty =
        AvaloniaProperty.Register<IssuesTipBox, string>(nameof(IssuesText),"");

    public IssuesTypeEnum IssuesType
    {
        get => GetValue(IssuesTypeProperty);
        set
        {
            SetValue(IssuesTypeProperty, value);
            SetValue(IssuesColorProperty, value switch
            {
                IssuesTypeEnum.Bug => new SolidColorBrush(Color.Parse("#2C2026")),
                IssuesTypeEnum.Feature => new SolidColorBrush(Color.Parse("#121D2F")),
                IssuesTypeEnum.Default => Brushes.DarkGray,
                IssuesTypeEnum.不错的第一个 => new SolidColorBrush(Color.Parse("#1F1E41")),
                IssuesTypeEnum.处理中 => new SolidColorBrush(Color.Parse("#0C1838")),
                IssuesTypeEnum.已完成 => new SolidColorBrush(Color.Parse("#333A27")),
                IssuesTypeEnum.已放弃 => new SolidColorBrush(Color.Parse("#1E241E")),
                IssuesTypeEnum.已查看 => new SolidColorBrush(Color.Parse("#0E3A21")),
                IssuesTypeEnum.投票中 => new SolidColorBrush(Color.Parse("#0B1A2C")),
                IssuesTypeEnum.文档 => new SolidColorBrush(Color.Parse("#0B2337")),
                IssuesTypeEnum.新功能 => new SolidColorBrush(Color.Parse("#28393E")),
                IssuesTypeEnum.无效 => new SolidColorBrush(Color.Parse("#343726")),
                IssuesTypeEnum.未查看 => new SolidColorBrush(Color.Parse("#29232F")),
                IssuesTypeEnum.漏洞 => new SolidColorBrush(Color.Parse("#321820")),
                IssuesTypeEnum.重复的 => new SolidColorBrush(Color.Parse("#30343A")),
                IssuesTypeEnum.问题 => new SolidColorBrush(Color.Parse("#32233C")),
                IssuesTypeEnum.需要帮助 => new SolidColorBrush(Color.Parse("#0B2628")),
            });
            SetValue(IssuesFontColorProperty, value switch
            {
                IssuesTypeEnum.Bug => new SolidColorBrush(Color.Parse("#F85149")),
                IssuesTypeEnum.Feature => new SolidColorBrush(Color.Parse("#4493F8")),
                IssuesTypeEnum.Default => Brushes.DarkGray,
                IssuesTypeEnum.不错的第一个 => new SolidColorBrush(Color.Parse("#C1B8FF")),
                IssuesTypeEnum.处理中 => new SolidColorBrush(Color.Parse("#9BB3FD")),
                IssuesTypeEnum.已完成 => new SolidColorBrush(Color.Parse("#DEF26E")),
                IssuesTypeEnum.已放弃 => new SolidColorBrush(Color.Parse("#95A958")),
                IssuesTypeEnum.已查看 => new SolidColorBrush(Color.Parse("#0DF246")),
                IssuesTypeEnum.投票中 => new SolidColorBrush(Color.Parse("#4F9DFC")),
                IssuesTypeEnum.文档 => new SolidColorBrush(Color.Parse("#35ABFF")),
                IssuesTypeEnum.新功能 => new SolidColorBrush(Color.Parse("#A0EEEE")),
                IssuesTypeEnum.无效 => new SolidColorBrush(Color.Parse("#E5E566")),
                IssuesTypeEnum.未查看 => new SolidColorBrush(Color.Parse("#B590AE")),
                IssuesTypeEnum.漏洞 => new SolidColorBrush(Color.Parse("#EB9CA6")),
                IssuesTypeEnum.重复的 => new SolidColorBrush(Color.Parse("#CDD1D5")),
                IssuesTypeEnum.问题 => new SolidColorBrush(Color.Parse("#D97EE5")),
                IssuesTypeEnum.需要帮助 => new SolidColorBrush(Color.Parse("#00E6C4")),
            });
        }
    }

    public IBrush IssuesColor
    {
        get => GetValue(IssuesColorProperty);
        set => SetValue(IssuesColorProperty, value);
    }

    public IBrush IssuesFontColor
    {
        get => GetValue(IssuesFontColorProperty);
        set => SetValue(IssuesFontColorProperty, value);
    }

    public string IssuesText
    {
        get => GetValue(IssuesTextProperty);
        set => SetValue(IssuesTextProperty, value);
    }
}