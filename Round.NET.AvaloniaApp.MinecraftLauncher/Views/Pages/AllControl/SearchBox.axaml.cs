using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class SearchBox : UserControl
{
    public Action CloseAction { get; set; }
    public SearchBox()
    {
        InitializeComponent();
        IsVisible = false;
        Opacity = 0;
    }

    public void Show()
    {
        if (this.IsVisible)
        {
            Margin = new Thickness(0, 0, 0, 0);
            Opacity = 0;
            ((MainView)Core.MainWindow.Content).SystemNavigationBar.NavTo("BackSearch");
            Task.Run(() =>
            {
                Dispatcher.UIThread.Invoke(() => CloseAction());
                Thread.Sleep(500);
                Dispatcher.UIThread.Invoke(() => this.IsVisible = false);
            });
        }
        else
        {
            this.IsVisible = true;
            this.Opacity = 1;
            ((MainView)Core.MainWindow.Content).SystemNavigationBar.NavTo("Clear",true);
            Margin = new Thickness(0, 0, 0, 0);
        }
    }
    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // 搜索类型选择回调
    }
    
    private List<Core.API.SearchItemConfig> SearchItems(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return Core.SearchItems;
        }

        // 使用 LINQ 表达式进行搜索，支持大小写不敏感匹配
        return Core.SearchItems
            .Where(item => item.Key.IndexOf(key) >= 0)
            .ToList();
    }
    
    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        // 搜索框修改输入回调
        
        SearchShowBox.Children.Clear();
        var key = ((TextBox)sender).Text;
        var result = SearchItems(key);
        
        // 将搜索结果显示到界面上
        foreach (var item in result)
        {
            var gobtn = new ListBoxItem()
            {
                Content = new Grid()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = 64,
                    Children =
                    {
                        new Label()
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(5),
                            Content = item.Title,
                            FontSize = 18,
                            FontWeight = FontWeight.Bold
                        },
                        new Label()
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(5),
                            Content = item.Type,
                        }
                    }
                },
            };
            gobtn.PointerPressed += (_, args) => item.Action(key);
            SearchShowBox.Children.Add(gobtn);
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Show();
    }
}