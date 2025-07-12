using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

public partial class EditGameGroup : UserControl
{
    public string GroupName { get;private set; }
    public string GroupColor { get;private set; }
    public EditGameGroup(string uuid)
    {
        InitializeComponent();

        var item = GameDrawerManager.GameDrawerManager.FindGroup(uuid);
        TextBox.Text = item.Name;
        ColorPicker.Color = Color.Parse(item.ColorHtmlCode);
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