using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Tips;

public class LssuesTipBox : TemplatedControl
{
    public static readonly StyledProperty<LssuesTypeEnum> LssuesTypeProperty =
        AvaloniaProperty.Register<LssuesTipBox, LssuesTypeEnum>(nameof(LssuesType));
    
    public static readonly StyledProperty<IBrush> LssuesColorProperty =
        AvaloniaProperty.Register<LssuesTipBox, IBrush>(nameof(LssuesColor));
    
    public static readonly StyledProperty<IBrush> LssuesFontColorProperty =
        AvaloniaProperty.Register<LssuesTipBox, IBrush>(nameof(LssuesFontColor));
    
    public static readonly StyledProperty<string> LssuesTextProperty =
        AvaloniaProperty.Register<LssuesTipBox, string>(nameof(LssuesText),"");

    public LssuesTypeEnum LssuesType
    {
        get => GetValue(LssuesTypeProperty);
        set
        {
            SetValue(LssuesTypeProperty, value);
            SetValue(LssuesColorProperty, value switch
            {
                LssuesTypeEnum.Bug => new SolidColorBrush(Color.Parse("#2C2026")),
                LssuesTypeEnum.Feature => new SolidColorBrush(Color.Parse("#121D2F")),
                LssuesTypeEnum.Default => Brushes.DarkGray,
                LssuesTypeEnum.不错的第一个 => new SolidColorBrush(Color.Parse("#1F1E41")),
                LssuesTypeEnum.处理中 => new SolidColorBrush(Color.Parse("#0C1838")),
                LssuesTypeEnum.已完成 => new SolidColorBrush(Color.Parse("#333A27")),
                LssuesTypeEnum.已放弃 => new SolidColorBrush(Color.Parse("#1E241E")),
                LssuesTypeEnum.已查看 => new SolidColorBrush(Color.Parse("#0E3A21")),
                LssuesTypeEnum.投票中 => new SolidColorBrush(Color.Parse("#0B1A2C")),
                LssuesTypeEnum.文档 => new SolidColorBrush(Color.Parse("#0B2337")),
                LssuesTypeEnum.新功能 => new SolidColorBrush(Color.Parse("#28393E")),
                LssuesTypeEnum.无效 => new SolidColorBrush(Color.Parse("#343726")),
                LssuesTypeEnum.未查看 => new SolidColorBrush(Color.Parse("#29232F")),
                LssuesTypeEnum.漏洞 => new SolidColorBrush(Color.Parse("#321820")),
                LssuesTypeEnum.重复的 => new SolidColorBrush(Color.Parse("#30343A")),
                LssuesTypeEnum.问题 => new SolidColorBrush(Color.Parse("#32233C")),
                LssuesTypeEnum.需要帮助 => new SolidColorBrush(Color.Parse("#0B2628")),
            });
            SetValue(LssuesFontColorProperty, value switch
            {
                LssuesTypeEnum.Bug => new SolidColorBrush(Color.Parse("#F85149")),
                LssuesTypeEnum.Feature => new SolidColorBrush(Color.Parse("#4493F8")),
                LssuesTypeEnum.Default => Brushes.DarkGray,
                LssuesTypeEnum.不错的第一个 => new SolidColorBrush(Color.Parse("#C1B8FF")),
                LssuesTypeEnum.处理中 => new SolidColorBrush(Color.Parse("#9BB3FD")),
                LssuesTypeEnum.已完成 => new SolidColorBrush(Color.Parse("#DEF26E")),
                LssuesTypeEnum.已放弃 => new SolidColorBrush(Color.Parse("#95A958")),
                LssuesTypeEnum.已查看 => new SolidColorBrush(Color.Parse("#0DF246")),
                LssuesTypeEnum.投票中 => new SolidColorBrush(Color.Parse("#4F9DFC")),
                LssuesTypeEnum.文档 => new SolidColorBrush(Color.Parse("#35ABFF")),
                LssuesTypeEnum.新功能 => new SolidColorBrush(Color.Parse("#A0EEEE")),
                LssuesTypeEnum.无效 => new SolidColorBrush(Color.Parse("#E5E566")),
                LssuesTypeEnum.未查看 => new SolidColorBrush(Color.Parse("#B590AE")),
                LssuesTypeEnum.漏洞 => new SolidColorBrush(Color.Parse("#EB9CA6")),
                LssuesTypeEnum.重复的 => new SolidColorBrush(Color.Parse("#CDD1D5")),
                LssuesTypeEnum.问题 => new SolidColorBrush(Color.Parse("#D97EE5")),
                LssuesTypeEnum.需要帮助 => new SolidColorBrush(Color.Parse("#00E6C4")),
            });
        }
    }

    public IBrush LssuesColor
    {
        get => GetValue(LssuesColorProperty);
        set => SetValue(LssuesColorProperty, value);
    }

    public IBrush LssuesFontColor
    {
        get => GetValue(LssuesFontColorProperty);
        set => SetValue(LssuesFontColorProperty, value);
    }

    public string LssuesText
    {
        get => GetValue(LssuesTextProperty);
        set => SetValue(LssuesTextProperty, value);
    }
}