using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace RMCL.Views.Pages.DialogPage.User;

public partial class LoginOfflineAccountDialog : UserControl
{
    public LoginOfflineAccountDialog()
    {
        InitializeComponent();
    }
    private string UserName { get; set; } = String.Empty;
    public string GetUserName()
    {
        return UserName;
    }
    public string GetSkinPath()
    {
        return SkinFilePathString;
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            UserName = UserNameBox.Text;
        }catch{ }
    }

    private async void SelectTheFile_OnClick(object? sender, RoutedEventArgs e)
    {
        // 获取当前窗口（TopLevel）
        var topLevel = TopLevel.GetTopLevel(this); // 假设在 Window/UserControl 内部
        if (topLevel == null)
            return;

        // 配置文件选择选项
        var options = new FilePickerOpenOptions
        {
            Title = "选择图片",
            FileTypeFilter = new[]
            {
                new FilePickerFileType("图片文件")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" },
                    MimeTypes = new[] { "image/png", "image/jpeg", "image/bmp", "image/gif" }
                }
            },
            AllowMultiple = false // 是否允许多选
        };

        // 打开文件选择对话框
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
        if (files.Count == 0)
            return; // 用户取消选择

        // 获取第一个文件（因为 AllowMultiple = false）
        var file = files[0];
    
        try
        {
            var path = file.Path.LocalPath;
            SkinPathBox.Text = path;
            SkinFilePathString = path;
        }
        catch
        { }
    }
    public string SkinFilePathString { get; set; } = String.Empty;
    private void SkinPathBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            SkinFilePathString = SkinPathBox.Text;
        }catch
        {}
    }
}