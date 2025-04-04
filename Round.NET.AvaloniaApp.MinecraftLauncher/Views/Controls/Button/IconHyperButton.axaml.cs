using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.CustomControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Button;

public class IconHyperButton : HyperlinkButton
{
    public static readonly StyledProperty<IImage> IconProperty =
        AvaloniaProperty.Register<IconHyperButton, IImage>(
            nameof(Icon));
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<IconHyperButton, string>(
            nameof(Text),"");
    
    public IImage Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
    }
}