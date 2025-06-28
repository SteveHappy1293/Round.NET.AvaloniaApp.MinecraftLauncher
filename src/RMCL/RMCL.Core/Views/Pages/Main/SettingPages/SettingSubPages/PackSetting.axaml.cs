using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using RMCL.Base.Interface;
using RMCL.Core.Models.Classes.Manager.StyleManager;

namespace RMCL.Core.Views.Pages.Main.SettingPages.SettingSubPages;

public partial class PackSetting : ISetting
{
    public PackSetting()
    {
        InitializeComponent();
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        IsEdit = false;
        Directory.CreateDirectory(PathsDictionary.PathDictionary.SkinFolder);
        
        ChoosePackListBox.Items.Clear();
        var its = Directory.GetFiles(PathsDictionary.PathDictionary.SkinFolder);
        
        foreach (var it in its)
        {
            ChoosePackListBox.Items.Add(new ListBoxItem() { Content = Path.GetFileName(it) });
        }

        ChoosePackListBox.SelectedIndex = Config.Config.MainConfig.Background.PackEntry.SelectedIndex;
        IsEdit = true;
    }

    private async void AddPackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException("无法获取顶级窗口");

        var fileOptions = new FilePickerOpenOptions
        {
            Title = "选择主题文件",
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Round Studio 通用主题文件")
                {
                    Patterns = new[] { "*.rskin" }
                }
            },
            AllowMultiple = false // 仅允许选择单个文件
        };

        var selectedFiles = await topLevel.StorageProvider.OpenFilePickerAsync(fileOptions);
        if (selectedFiles.Count > 0 && selectedFiles[0] is { } selectedFile)
        {
            var path = Path.GetFullPath(selectedFile.Path.LocalPath);
            Directory.CreateDirectory(PathsDictionary.PathDictionary.SkinFolder);

            File.Copy(path, Path.Combine(PathsDictionary.PathDictionary.SkinFolder, Path.GetFileName(path)));
        }

        UpdateUI();
    }

    private void UpdateBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        UpdateUI();
    }

    private void ChoosePackListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.Background.PackEntry.SelectedIndex = ChoosePackListBox.SelectedIndex;
            Config.Config.SaveConfig();
            
            StyleManager.UpdateBackground();
        }
    }
}