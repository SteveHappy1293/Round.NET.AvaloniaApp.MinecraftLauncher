using System.Security.AccessControl;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;

namespace MusicPlug.Views.Controls;

public partial class MusicItem : UserControl
{
    public Song MusicInfo { get; set; }
    public MusicItem()
    {
        InitializeComponent();
    }

    public void Change()
    {
        MusicNameBox.Text = MusicInfo.Name;
        MusicWriterBox.Text = string.Join('、',MusicInfo.Artists.Select(x => x.Name));
    }

    private void InputElement_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        ControlPanel.Margin = new Thickness(0, 0, 0, 0);
    }

    private void InputElement_OnPointerExited(object? sender, PointerEventArgs e)
    {
        ControlPanel.Margin = new Thickness(55, 0, -55, 0);
    }

    private void PlayButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var download = new DownloadMusic();
        download.MusicInfo = MusicInfo;
        var taskbox = new ContentDialog()
        {
            Content = download,
            Title = $"正在下载：{MusicInfo.Name}"
        };
        download.Download();
        taskbox.ShowAsync();
    }
}