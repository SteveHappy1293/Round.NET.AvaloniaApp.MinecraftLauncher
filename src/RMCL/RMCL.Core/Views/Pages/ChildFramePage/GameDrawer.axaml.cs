using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

namespace RMCL.Core.Views.Pages.ChildFramePage;

public partial class GameDrawer : UserControl
{
    public GameDrawer()
    {
        InitializeComponent();
    }

    private void AddGameGroupBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var con = new ContentDialog()
        {
            Content = new AddGameGroup(),
            Title = "新增游戏分类",
            CloseButtonText = "新增",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        };
        con.ShowAsync(Core.Models.Classes.Core.MainWindow);
    }

    private void TextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        if (!string.IsNullOrEmpty(SearchBox.Text))
        {
            SearchBox.HorizontalAlignment = HorizontalAlignment.Right;
            SearchBox.Margin = new Thickness(20,60,20,20);
            SearchBox.Width = Bounds.Width / 3;
            SearchTextBlock.Width = Bounds.Width - 80 - (Bounds.Width / 3);
            SearchTextBlock.Opacity = 1;
            SearchTextBlock.Margin = new Thickness(20, 60, 0, 10);

            SearchTextBlock.Text = $"\"{SearchBox.Text}\"";
        }
        else
        {
            SearchBox.HorizontalAlignment = HorizontalAlignment.Right;
            SearchBox.Margin = new Thickness(65,20);
            SearchBox.Width = 200;
            SearchTextBlock.Opacity = 0;
            SearchTextBlock.Margin = new Thickness(20, 20, 0, 10);
        }
    }
}