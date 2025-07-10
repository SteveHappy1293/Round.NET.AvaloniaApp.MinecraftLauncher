using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

public partial class AddGameGroup : UserControl
{
    public string GroupName { get;private set; }
    public string GroupColor { get;private set; }
    public AddGameGroup()
    {
        InitializeComponent();
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        GroupName = ((TextBox)sender).Text;
    }

    private void ColorPicker_OnColorChanged(object? sender, ColorChangedEventArgs e)
    {
        GroupColor = ((ColorPicker)sender).Color.ToString();
    }
}