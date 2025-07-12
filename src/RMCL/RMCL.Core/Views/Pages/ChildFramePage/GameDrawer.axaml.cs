using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using RMCL.Base.Entry;
using RMCL.Base.Entry.Game.GameDrawer;
using RMCL.Controls.Item.GameDrawer;
using RMCL.Core.Models.Classes.Launch;
using RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

namespace RMCL.Core.Views.Pages.ChildFramePage;

public partial class GameDrawer : UserControl
{
    public GameDrawer()
    {
        InitializeComponent();
        UpdateUI();
    }

    public void UpdateUI()
    {
        GameDrawerPanel.Children.Clear();
        if (GameDrawerManager.GameDrawerManager.GameDrawer.Groups.Count == 0) NullBox.IsVisible = true;
        else NullBox.IsVisible = false;
        
        GameDrawerManager.GameDrawerManager.GameDrawer.Groups.ForEach(x =>
        {
            var it = new GameDrawerGroupItem();
            it.GroupUUID = x.Uuid;
            it.OnEdit = (s) =>
            {
                var edit = new EditGameGroup(x.Uuid);
                var con = new ContentDialog()
                {
                    Content = edit,
                    Title = "编辑游戏分类",
                    CloseButtonText = "保存",
                    PrimaryButtonText = "取消",
                    SecondaryButtonText = "删除",
                    DefaultButton = ContentDialogButton.Close
                };
                con.CloseButtonClick += async (_, __) =>
                {
                    GameDrawerManager.GameDrawerManager.FindGroup(s).Name = edit.TextBox.Text;
                    GameDrawerManager.GameDrawerManager.FindGroup(s).ColorHtmlCode = edit.ColorPicker.Color.ToString();
                    
                    GameDrawerManager.GameDrawerManager.SaveConfig();
                    UpdateUI();
                };
                con.SecondaryButtonClick += (sender, args) =>
                {
                    GameDrawerManager.GameDrawerManager.GameDrawer.Groups.RemoveAll(x => x.Uuid == s);
                    GameDrawerManager.GameDrawerManager.SaveConfig();
                    UpdateUI();
                };
                con.ShowAsync();
            };
            it.OnAdd = (s) =>
            {
                var add = new AddGameItem();
                var con = new ContentDialog()
                {
                    Content = add,
                    Title = "添加游戏",
                    CloseButtonText = "添加",
                    PrimaryButtonText = "取消",
                    DefaultButton = ContentDialogButton.Close
                };
                con.CloseButtonClick += async (_, __) =>
                {
                    GameDrawerManager.GameDrawerManager.RegisterItem(s,new GameDrawerItem()
                    {
                        ClientInfo = new()
                        {
                            GameFolder = add.GameFolder,
                            GameName = add.GameName
                        }
                    });
                    GameDrawerManager.GameDrawerManager.SaveConfig();
                    
                    UpdateUI();
                };
                con.ShowAsync();
            };
            it.OnLaunch = (s, s1) =>
            {
                var entry = GameDrawerManager.GameDrawerManager.FindGroup(s).Children
                    .Find(x => x.Uuid == s1).ClientInfo;
                LaunchService.LaunchTask(entry);
            };
            GameDrawerPanel.Children.Add(it);
        });
    }
    private void AddGameGroupBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var add = new AddGameGroup();
        var con = new ContentDialog()
        {
            Content = add,
            Title = "新增游戏分类",
            CloseButtonText = "新增",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        };
        con.CloseButtonClick += async (_, __) =>
        {
            GameDrawerManager.GameDrawerManager.RegisterGroup(new GameDrawerGroupEntry()
            {
                ColorHtmlCode = add.GroupColor,
                Name = add.GroupName,
            });
            GameDrawerManager.GameDrawerManager.SaveConfig();
            UpdateUI();
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