using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class StyleSetting : UserControl
{
    public bool IsEdit = false;
    public StyleSetting()
    {
        InitializeComponent();
        
        if (!string.IsNullOrWhiteSpace(Config.MainConfig.BackImage))
        {
            ImagePathBox.Text = Config.MainConfig.BackImage;
            imagepath = Config.MainConfig.BackImage;
            BackTMDSlider.Value = Config.MainConfig.BackOpacity * 100;
        }
        
        if (!string.IsNullOrWhiteSpace(Config.MainConfig.StyleFile))
        {
            StylePathBox.Text = Config.MainConfig.StyleFile;
        }

        BackgroundTypeComboBox.SelectedIndex = Config.MainConfig.BackModlue;
        ChoseMainPageBody.SelectedIndex = Config.MainConfig.HomeBody;
        
        IsEdit = true;
    }
    private void BackgroundTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BackgroundTypeComboBox != null)
        {
            if (BackgroundTypeComboBox.SelectedIndex == 2)
            {
                ImageBox.IsVisible = true;
                StyleBox.IsVisible = false;
                SaveConfig.IsVisible = true;
            }else if (BackgroundTypeComboBox.SelectedIndex == 4)
            {
                SaveConfig.IsVisible = false;
                StyleBox.IsVisible = true;
                ImageBox.IsVisible = false;
            }
            else
            {
                ImageBox.IsVisible = false;
                SaveConfig.IsVisible = false;
                StyleBox.IsVisible = false;
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
                        Dispatcher.UIThread.Invoke(() => ImagePathBox.Text = fileName);
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
        if (Config.MainConfig.BackModlue == 2)
        {
            if (imagepath != String.Empty)
            {
                Config.MainConfig.BackImage = imagepath;
                Config.MainConfig.BackOpacity = ((int)BackTMDSlider.Value) * 0.01;
            }
        }else if (Config.MainConfig.BackModlue == 4)
        {
            Config.MainConfig.StyleFile = StylePathBox.Text;
        }
        Config.SaveConfig();
        StyleMange.Load();
    }
    private void BackTMDSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        BackTMDLabel.Content = $"背景图透明度({(int)BackTMDSlider.Value}%)";
    }

    private async void SaveConfig_OnClick(object? sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "另存为项目文件",
            InitialFileName = $"My Style {DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}", // 默认文件名
            DefaultExtension = "rskin",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "RMCL 通用主题格式", Extensions = new List<string> { "rskin" } }
            }
        };

        string? filePath = await saveFileDialog.ShowAsync(Core.MainWindow);

        if (!string.IsNullOrEmpty(filePath))
        {
            StyleMange.ExportStyleConfigFile(filePath);
        }
    }

    private async void ChooseStyle_OnClick_OnClick(object? sender, RoutedEventArgs e)
    {
        // 创建 OpenFileDialog 实例
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "打开主题文件", // 对话框标题
            AllowMultiple = false, // 是否允许选择多个文件
            Filters = new ()
            {
                new FileDialogFilter { Name = "RMCL 通用主题文件", Extensions = new List<string> { "rskin" } },
            }
        };

        // 打开文件对话框并获取用户选择的文件路径
        string[] filePaths = await openFileDialog.ShowAsync(Core.MainWindow);

        // 检查是否选择了文件
        if (filePaths.Length > 0)
        {
            string filePath = filePaths[0]; // 获取第一个文件的路径
            
            StylePathBox.Text = filePath;
        }
    }

    private void ChoseMainPageBody_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.HomeBody = ChoseMainPageBody.SelectedIndex;
            Config.SaveConfig();
            HomeBodyMange.Load();
        }
    }
}