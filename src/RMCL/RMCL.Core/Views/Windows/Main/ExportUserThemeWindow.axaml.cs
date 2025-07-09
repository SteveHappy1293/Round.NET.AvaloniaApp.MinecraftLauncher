using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using RMCL.Core.Models.Classes.Manager.StyleManager;

namespace RMCL.Core.Views.Windows.Main;

public partial class ExportUserThemeWindow : Window
{
    public ExportUserThemeWindow()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void TitleBar_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private async void Export_OnClick(object? sender, RoutedEventArgs e)
    {
        var build = new BuildNeoSkinPack();
        build.CollectConfig((bool)ModelBackground.IsChecked, (bool)ModelColor.IsChecked, (bool)ModelButton.IsChecked,
            (bool)ModelMusic.IsChecked);
        
        var saveFileDialog = new SaveFileDialog
        {
            Title = "另存为 RS 通用主题文件",
            InitialFileName = $"My Style {DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}", // 默认文件名
            DefaultExtension = "rskin",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "RMCL 通用主题格式", Extensions = new List<string> { "rskin" } }
            }
        };

        string? filePath = await saveFileDialog.ShowAsync(this);

        if (!string.IsNullOrEmpty(filePath))
        {
            File.Copy(build.ResultPath, filePath);
            
            this.Close();
        }
    }
}