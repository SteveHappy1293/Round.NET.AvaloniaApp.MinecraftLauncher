using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class MySelfSetting : UserControl
{
    public MySelfSetting()
    {
        InitializeComponent();

        if (!string.IsNullOrWhiteSpace(Config.MainConfig.BackImage))
        {
            ImagePathBox.Text = Config.MainConfig.BackImage;
            imagepath = Config.MainConfig.BackImage;
        }

        BackgroundTypeComboBox.SelectedIndex = Config.MainConfig.BackModlue;
    }
    private void BackgroundTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selected = BackgroundTypeComboBox.SelectedItem as ComboBoxItem;
        if (selected != null)
        {
            if (selected.Tag.ToString() == "图片")
            {
                ImageBox.IsVisible = true;
            }
            else
            {
                ImageBox.IsVisible = false;
            }
        }
    }

    private string imagepath = String.Empty;
    private void ChooseImage_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter()
                {
                    Name = "图片资源",
                    Extensions = new List<string>(){"png", "jpg", "jpeg", "bmp", "gif"}
                }
            }
        };

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Task.Run(() =>
            {
                Message.Show("本体设置[个性化]", $"正在打开图片", InfoBarSeverity.Informational);
                string[] fileNames = openFileDialog.ShowAsync(desktop.MainWindow).Result;
                if (fileNames != null && fileNames.Length > 0)
                {
                    string fileName = fileNames[0];
                    try
                    {
                        imagepath = fileName;
                        ImagePathBox.Text = fileName;
                    }
                    catch (Exception ex)
                    {
                        Message.Show("本体设置[个性化]", $"打开图片失败！\n{ex.Message}", InfoBarSeverity.Error);
                    }
                }
            });
        }
    }
    private void SaveImage_OnClick(object? sender, RoutedEventArgs e)
    {
        Config.MainConfig.BackModlue = BackgroundTypeComboBox.SelectedIndex;
        if (Config.MainConfig.BackModlue == 1)
        {
            if (imagepath != String.Empty)
            {
                Config.MainConfig.BackImage = imagepath;
            }
        }
        Config.SaveConfig();
        LoadingBackground.Load();
    }
}