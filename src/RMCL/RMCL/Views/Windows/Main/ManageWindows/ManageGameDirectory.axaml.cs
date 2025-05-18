using System.IO;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using RMCL.Controls.ControlHelper;
using RMCL.Controls.Item;
using RMCL.Models.Classes;

namespace RMCL.Views.Windows.Main.ManageWindows;

public partial class ManageGameDirectory : Window
{
    public ManageGameDirectory()
    {
        InitializeComponent();

        Refuse();
    }

    public void Refuse()
    {
        DirsBox.Items.Clear();
        Config.Config.MainConfig.GameFolders.ForEach(x =>
        {
            DirsBox.Items.Add(new GameDirectoryItem(new GameDirectoryItemInfo()
            {
                Name = x.Name,
                Path = x.Path,
            }));
        });
        DirsBox.SelectedIndex = Config.Config.MainConfig.SelectedGameFolder;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void DeleteFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var con = new ContentDialog()
        {
            Content = $"你确定要移除 {Config.Config.MainConfig.GameFolders[DirsBox.SelectedIndex].Name} 这个游戏目录吗？\n移除后你需要添加可以再次添加，此次操作不会删除磁盘文件。",
            CloseButtonText = "移除",
            PrimaryButtonText = "保留",
            Title = "移除游戏目录",
            DefaultButton = ContentDialogButton.Primary
        };
        con.CloseButtonClick += (s, e1) =>
        {
            if (Config.Config.MainConfig.SelectedGameFolder == DirsBox.SelectedIndex &&
                Config.Config.MainConfig.SelectedGameFolder > 0)
            {
                Config.Config.MainConfig.SelectedGameFolder--;
            }
            else
            {
                Config.Config.MainConfig.SelectedGameFolder = 0;
            }

            Config.Config.MainConfig.GameFolders.RemoveAt(DirsBox.SelectedIndex);
            Config.Config.SaveConfig();
            Refuse();
        };
        con.ShowAsync(this);
    }

    private void OpenFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var path = Config.Config.MainConfig.GameFolders[DirsBox.SelectedIndex].Path;
        SystemHelper.FileExplorer.OpenFolder(Path.GetFullPath(path));
    }
}