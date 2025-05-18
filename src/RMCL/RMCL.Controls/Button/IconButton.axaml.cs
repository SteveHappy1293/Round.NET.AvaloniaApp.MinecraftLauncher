using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;

namespace RMCL.Controls.Button;

public partial class IconButton : Avalonia.Controls.Button
{
    public IconButton()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<FluentIconSymbol> IconProperty =
        AvaloniaProperty.Register<IconButton, FluentIconSymbol>(nameof(Icon));
        
    public static readonly StyledProperty<object> ContentProperty =
        ContentControl.ContentProperty.AddOwner<IconButton>();

    public FluentIconSymbol Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
        
    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}