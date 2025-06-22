using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using RMCL.Base.Interface;
using RMCL.Controls.Item.StyleItem;
using RMCL.Models.Classes.Manager.StyleManager;

namespace RMCL.Views.Pages.Main.SettingPages.SettingSubPages;

public partial class ImageSetting : ISetting
{
    public ImageSetting()
    {
        InitializeComponent();

        UpdateUI();
    }

    public void UpdateUI()
    {
        IsEdit = false;
        ChooseImageListBox.Items.Clear();
        
        Config.Config.MainConfig.Background.ImageEntry.ImagePaths.ForEach(x =>
        {
            var it = new ImageItem(x);
            it.DeleteCallBack = s =>
            {
                if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex == 0)
                {
                    Config.Config.MainConfig.Background.ImageEntry.ChooseIndex = -1;
                }
                var num = Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Count(x1 => x1 == s);
                var delnum = Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Count - num;

                if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex >= delnum)
                {
                    Config.Config.MainConfig.Background.ImageEntry.ChooseIndex = 0;
                }
                Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Remove(s);
                Config.Config.SaveConfig();
                
                StyleManager.UpdateBackground();
                UpdateUI();
            };
            ChooseImageListBox.Items.Add(it);
        });
        if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex != -1)
        {
            ChooseImageListBox.SelectedIndex = Config.Config.MainConfig.Background.ImageEntry.ChooseIndex;
        }
        
        OpacitySetting.Value = Config.Config.MainConfig.Background.ImageEntry.Opacity;
        ShowOpacity.Text = $"背景透明度 ({Config.Config.MainConfig.Background.ImageEntry.Opacity} %)";

        IsEdit = true;
    }

    private async void AddImageBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this) ?? throw new InvalidOperationException("无法获取顶级窗口");

        var fileOptions = new FilePickerOpenOptions
        {
            Title = "选择图片文件",
            FileTypeFilter = new[]
            {
                new FilePickerFileType("图片文件")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" }
                }
            },
            AllowMultiple = false // 仅允许选择单个文件
        };

        var selectedFiles = await topLevel.StorageProvider.OpenFilePickerAsync(fileOptions);
        if (selectedFiles.Count > 0 && selectedFiles[0] is { } selectedFile)
        {
            var path = Path.GetFullPath(selectedFile.Path.LocalPath);
            
            Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Add(path);
            Config.Config.SaveConfig();
        }

        UpdateUI();
    }

    private void UpdateBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        UpdateUI();
    }

    private void ChooseImageListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.Background.ImageEntry.ChooseIndex = ChooseImageListBox.SelectedIndex;
            Config.Config.SaveConfig();
            
            StyleManager.UpdateBackground();
        }
    }

    private void RangeBase_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.Background.ImageEntry.Opacity = (int)OpacitySetting.Value;
            Config.Config.SaveConfig();
            ShowOpacity.Text = $"背景透明度 ({Config.Config.MainConfig.Background.ImageEntry.Opacity} %)";
            
            StyleManager.UpdateBackground();
        }
    }
}