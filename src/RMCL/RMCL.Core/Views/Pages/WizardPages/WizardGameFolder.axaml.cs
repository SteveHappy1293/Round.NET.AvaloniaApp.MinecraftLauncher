using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using RMCL.Config;
using RMCL.Controls.ControlHelper;
using RMCL.Controls.Item;

namespace RMCL.Core.Views.Pages.WizardPages;

public partial class WizardGameFolder : UserControl
{
    public Window ParentWindow { get; set; }
    public WizardGameFolder()
    {
        InitializeComponent();
        UpdateUI();
    }

    public void UpdateUI()
    {
        GameFolderBox.Children.Clear();
        Config.Config.MainConfig.GameFolders.ForEach(x =>
        {
            GameFolderBox.Children.Add(new ItemBox()
            {
                Content = new GameDirectoryItem(new GameDirectoryItemInfo()
                {
                    Name = x.Name,
                    Path = x.Path,
                })
            });
        });
    }

    private async void AddFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var storageProvider = ParentWindow.StorageProvider;

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
                
                UpdateUI();
            };
            con.ShowAsync();
        }
    }
}