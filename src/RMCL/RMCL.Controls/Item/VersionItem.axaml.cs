using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Item;

public partial class VersionItem : UserControl
{
    public VersionItem(string version)
    {
        InitializeComponent();
        
        TitleBox.Text = version;
    }
    
    public Action DownloadClick;

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        DownloadClick.Invoke();
    }
}