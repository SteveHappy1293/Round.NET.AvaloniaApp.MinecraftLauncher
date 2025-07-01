using System.IO;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using RMCL.Config;
using RMCL.Controls.ControlHelper;
using RMCL.Controls.Item;
using RMCL.Core.Models.Classes;

namespace RMCL.Core.Views.Windows.Main.ManageWindows;

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
        SystemHelper.SystemHelper.FileExplorer.OpenFolder(Path.GetFullPath(path));
    }

    private async void AddFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var storageProvider = this.StorageProvider;

        // 配置文件夹选择选项
        var options = new FolderPickerOpenOptions
        {
            Title = "选择文件夹",
            SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync("C:\\"), // 可选默认路径
            AllowMultiple = false // 是否允许多选
        };

        // 显示对话框并获取结果
        var folders = await storageProvider.OpenFolderPickerAsync(options);
        var folder = folders.Count > 0 ? folders[0].Path.LocalPath : null;
        
        if (folder != null)
        {
            var namebox =
                new TextBox()
                {
                    Width = 120,
                    Text = SystemHelper.SystemHelper.GetFolderName(folder)
                };
            var con = new ContentDialog()
            {
                Content = new DockPanel()
                {
                    Children =
                    {
                        new TextBlock()
                        {
                            VerticalAlignment = VerticalAlignment.Center,
                            Text = "文件夹名称：",
                            Margin = new Thickness(0,0,20,0)
                        },
                        namebox
                    }
                },
                Title = "设置文件夹名称",
                PrimaryButtonText = "取消",
                CloseButtonText = "添加",
                DefaultButton = ContentDialogButton.Close
            };
            con.CloseButtonClick += (s, e) =>
            {
                Config.Config.MainConfig.GameFolders.Add(new GameFolderConfig()
                {
                    Path = folder,
                    Name = namebox.Text == "" ? SystemHelper.SystemHelper.GetFolderName(folder) : namebox.Text
                });
                
                Refuse();
            };
            con.ShowAsync(this);
        }
    }
}