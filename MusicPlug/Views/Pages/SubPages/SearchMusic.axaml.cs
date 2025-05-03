using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MusicPlug.Modules;
using MusicPlug.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.ContentPanel;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace MusicPlug.Views.Pages.SubPages;

public partial class SearchMusic : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>());
    }

    public SearchMusic()
    {
        InitializeComponent();
    }

    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // Store the search text on the UI thread before starting the background task
        string searchText = SearchKey.Text;
    
        SearchButton.Content = new ProgressRing()
        {
            Width = 15,
            Height = 15
        };
        SearchButton.IsEnabled = false;
        MusicBox.Children.Clear();
    
        Task.Run(() =>
        {
            var songs = (SearchCore.Search(searchText)).Result.Songs;
            Dispatcher.UIThread.Invoke(() =>
            {
                foreach (var song in songs)
                {
                    if (song.Fee > 4)
                    {
                        var item = new Controls.MusicItem()
                        {
                            MusicInfo = song
                        };
                        item.Change();
                        MusicBox.Children.Add(item);
                    }
                }
            
                SearchButton.Content = "搜索";
                SearchButton.IsEnabled = true;
            });
        });
    }
}