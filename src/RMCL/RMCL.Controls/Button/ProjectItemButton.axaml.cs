using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace RMCL.Controls.Button;

public partial class ProjectItemButton : UserControl
{
    public ProjectItemButton()
    {
        InitializeComponent();
    }

    public void UpdateUI()
    {
        if (_title != String.Empty)
        {
            TitleBox.Text = _title;
        }

        if (_icon != null)
        {
            ImageBox.Source = _icon;
        }
    }
    private string _title { get; set; } = String.Empty;
    private string _url { get; set; } = String.Empty;
    private IImage _icon { get; set; } = null;
    
    public string Title
    {
        get { return _title; }
        set
        {
            _title = value;
            UpdateUI();
        }
    }
    
    public string URL
    {
        get { return _url; }
        set
        {
            _url = value;
            UpdateUI();
        }
    }

    public IImage Icon
    {
        get { return _icon; }
        set
        {
            _icon = value;
            UpdateUI();
        }
    }

    private void GoBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        SystemHelper.SystemHelper.OpenUrl(_url);
    }
}