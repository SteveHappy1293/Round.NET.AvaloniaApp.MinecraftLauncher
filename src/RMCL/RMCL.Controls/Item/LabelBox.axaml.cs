using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Item;

public partial class LabelBox : UserControl
{
    private string _text;
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

    public LabelBox()
    {
        InitializeComponent();
        _textBlock = ItTextBox;
    }
}