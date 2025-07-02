using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace RMCL.Controls.Item;

public partial class LabelBox : UserControl
{
    private string _text;
    private IBrush _boxbackground;
    private int _fontsize;
    private TextBlock _textBlock;

    public string Text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            _textBlock.Text = value;
        }
    }
    public IBrush BoxBackground
    {
        get
        {
            return _boxbackground;
        }
        set
        {
            _boxbackground = value;
            Border.Background = value;
        }
    }
    public int BoxFontSize
    {
        get
        {
            return _fontsize;
        }
        set
        {
            _fontsize = value;
            _textBlock.FontSize = value;
        }
    }

    public LabelBox()
    {
        InitializeComponent();
        _textBlock = ItTextBox;
    }
}