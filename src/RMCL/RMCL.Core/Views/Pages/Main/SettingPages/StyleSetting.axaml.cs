using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.Interop;
using FluentAvalonia.UI.Controls;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Base.Enum.ButtonStyle;
using RMCL.Base.Interface;
using RMCL.Config;
using RMCL.Controls.View;
using RMCL.Core.Models.Classes.Manager.StyleManager;
using RMCL.Core.Views.Pages.Main.SettingPages.SettingSubPages;
using RMCL.Core.Models.Classes;
using RMCL.Core.Views.Windows.Main;
using SkiaSharp;
using Color = Avalonia.Media.Color;
using ColorChangedEventArgs = Avalonia.Controls.ColorChangedEventArgs;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class StyleSetting : ISetting
{
    public Dictionary<BackgroundModelEnum, UserControl> BackgroundSettings { get; set; } = new()
    {
        { BackgroundModelEnum.Glass, new ColorGlassSetting() },
        { BackgroundModelEnum.Pack, new PackSetting() },
        { BackgroundModelEnum.Image, new ImageSetting() },
        { BackgroundModelEnum.None, new NullBox() { SmallTitle = "这里没有设置", Margin = new Thickness(10) } },
        { BackgroundModelEnum.Mica, new NullBox() { SmallTitle = "这里没有设置", Margin = new Thickness(10) } },
        { BackgroundModelEnum.AcrylicBlur, new NullBox() { SmallTitle = "这里没有设置", Margin = new Thickness(10) } }
    };

    public StyleSetting()
    {
        InitializeComponent();
        ChooseBackgroundModel.SelectedIndex = Config.Config.MainConfig.Background.ChooseModel.GetHashCode();
        var enums = (BackgroundModelEnum)ChooseBackgroundModel.SelectedIndex;
        Config.Config.MainConfig.Background.ChooseModel = enums;
        BackgroundMaxSetting.Content = BackgroundSettings[enums];
        MainButtonStyle.SelectedIndex = Config.Config.MainConfig.ButtonStyle.HomeButton.GetHashCode() - 1;
        QuickChoosePlayerButtonStyle.SelectedIndex = Config.Config.MainConfig.ButtonStyle.QuickChoosePlayerButton.GetHashCode();
        ColorPicker.Color = Color.Parse(Config.Config.MainConfig.ThemeColors.ThemeColors);
        TurnOnMusic.IsChecked = Config.Config.MainConfig.BackMusicEntry.Enabled;
        MusicPathBox.Text = Config.Config.MainConfig.BackMusicEntry.Path;
        
        ColorThemeModel.SelectedIndex = Config.Config.MainConfig.ThemeColors.ColorType.GetHashCode();
        LightTheme.SelectedIndex = Config.Config.MainConfig.ThemeColors.Theme.GetHashCode();
        SystemHelper.GetSystemFonts.GetSystemFontFamilies().ForEach(x => ChoseLogViewFontBox.Items.Add(x));
        ChoseLogViewFontBox.SelectedIndex = SystemHelper.GetSystemFonts.GetSystemFontFamilies()
            .FindIndex(x => x == Config.Config.MainConfig.FontsConfig.ChoseFontName);
        FontSizeSetting.Value = Config.Config.MainConfig.FontsConfig.FontSize;
        if (Config.Config.MainConfig.ThemeColors.ColorType == ColorType.System)
        {
            UserColorBox.IsVisible = false;
        }
        else
        {
            UserColorBox.IsVisible = true;
        }

        UpdateFontPreview();
        IsEdit = true;
        
        if (Config.Config.MainConfig.Background.ChooseModel == BackgroundModelEnum.Pack)
        {
            ExportSkinPackBox.IsVisible = false;
            SetButton.IsVisible = false;
            SetColor.IsVisible = false;
        }else{
            ExportSkinPackBox.IsVisible = true;
            SetButton.IsVisible = true;
            SetColor.IsVisible = true;
        }
        
        if (!OSVersionHelper.IsWindows11()) ItemMica.IsEnabled = false;
        if (!OSVersionHelper.IsWindows()) ItemAcrylicBlur.IsEnabled = false;
    }

    private void ChooseBackgroundModel_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            var enums = (BackgroundModelEnum)ChooseBackgroundModel.SelectedIndex;
            Config.Config.MainConfig.Background.ChooseModel = enums;
            BackgroundMaxSetting.Content = BackgroundSettings[enums];
            StyleManager.UpdateBackground();

            Config.Config.SaveConfig();

            if (Config.Config.MainConfig.Background.ChooseModel == BackgroundModelEnum.Pack)
            {
                ExportSkinPackBox.IsVisible = false;
                SetButton.IsVisible = false;
                SetColor.IsVisible = false;
            }else{
                ExportSkinPackBox.IsVisible = true;
                SetButton.IsVisible = true;
                SetColor.IsVisible = true;
            }
        }
    }

    private void MainButtonStyle_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.ButtonStyle.HomeButton = (OrdinaryButtonStyle)MainButtonStyle.SelectedIndex+1;

            Config.Config.SaveConfig();
            Models.Classes.Core.MainWindow.UpdateButtonStyle();
        }
    }

    private void QuickChoosePlayerButtonStyle_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.ButtonStyle.QuickChoosePlayerButton = (OrdinaryButtonStyle)QuickChoosePlayerButtonStyle.SelectedIndex;

            Config.Config.SaveConfig();
            Models.Classes.Core.MainWindow.UpdateButtonStyle();
        }
    }

    private void ColorView_OnColorChanged(object? sender, ColorChangedEventArgs e)
    {
        if (IsEdit)
        {
            StyleManager.UpdateSystemColor(ColorPicker.Color.ToString());
        }
    }

    private void ColorThemeModel_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.ThemeColors.ColorType = (ColorType)ColorThemeModel.SelectedIndex;
            Config.Config.SaveConfig();
            
            StyleManager.UpdateSystemColor();

            if (Config.Config.MainConfig.ThemeColors.ColorType == ColorType.System)
            {
                UserColorBox.IsVisible = false;
            }
            else
            {
                UserColorBox.IsVisible = true;
            }
        }
    }

    private void AccentIconButton_OnClick(object? sender, RoutedEventArgs e)
    {
        new ExportUserThemeWindow().ShowDialog(Models.Classes.Core.MainWindow);
    }

    private void LightTheme_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.ThemeColors.Theme = (ThemeType)LightTheme.SelectedIndex;
            Config.Config.SaveConfig();
            
            StyleManager.UpdateBackground();
        }
    }

    private void GetImageColor_OnClick(object? sender, RoutedEventArgs e)
    {
        GetImageColor.IsEnabled = false;
        GetImageColor.Content = new ProgressRing()
        {
            Width = 20,
            Height = 20
        };
        Task.Run(() =>
        {
            if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex != -1
                && Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Count != 0
                && Config.Config.MainConfig.Background.ImageEntry.ChooseIndex <
                Config.Config.MainConfig.Background.ImageEntry.ImagePaths.Count
                && File.Exists(
                    Config.Config.MainConfig.Background.ImageEntry.ImagePaths[
                        Config.Config.MainConfig.Background.ImageEntry.ChooseIndex]))
            {
                using var bitmap = SKBitmap.Decode(Config.Config.MainConfig.Background.ImageEntry.ImagePaths[
                    Config.Config.MainConfig.Background.ImageEntry.ChooseIndex]);
                var dominantColor = DominantColorExtractor.GetDominantColor(bitmap);
                var avaloniaColor = Color.FromRgb(dominantColor.Red, dominantColor.Green, dominantColor.Blue);

                Dispatcher.UIThread.Invoke(() =>
                {
                    ColorPicker.Color = avaloniaColor;
                });
            }
            
            Dispatcher.UIThread.Invoke(() =>
            {
                GetImageColor.IsEnabled = true;
                GetImageColor.Content = new FluentIcon()
                {
                    Width = 20,
                    Icon = FluentIconSymbol.Target20Regular
                };
            });
        });
    }

    private void ChoseLogViewFontBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.FontsConfig.ChoseFontName =
                SystemHelper.GetSystemFonts.GetSystemFontFamilies()[ChoseLogViewFontBox.SelectedIndex];
            
            Config.Config.SaveConfig();
            UpdateFontPreview();
        }
    }

    public void UpdateFontPreview()
    {
        try
        {
            var fontname = Config.Config.MainConfig.FontsConfig.ChoseFontName;
            var fontsize = Config.Config.MainConfig.FontsConfig.FontSize;
        
            FontPreview.FontSize = fontsize;
            FontPreview.FontFamily = Avalonia.Media.FontFamily.Parse(fontname);
            FontSizeSettingBox.Content = $"日志字体大小 ({fontsize}pt)";
        }catch{ }
    }

    private void FontSizeSetting_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.FontsConfig.FontSize = (int)FontSizeSetting.Value;
            
            Config.Config.SaveConfig();
            UpdateFontPreview();
        }
    }
    public async Task<string?> SelectSingleMp3File()
    {
        // 获取顶级窗口
        var topLevel = TopLevel.GetTopLevel(Models.Classes.Core.MainWindow);
    
        // 创建文件选择器选项
        var fileOptions = new FilePickerOpenOptions
        {
            Title = "选择 MP3 文件",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("MP3 音频文件")
                {
                    Patterns = new[] { "*.mp3" },
                    MimeTypes = new[] { "audio/mpeg" }
                }
            }
        };
    
        // 打开文件选择对话框
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(fileOptions);
    
        // 返回选择的文件路径
        return files.Count == 1 ? files[0].Path.LocalPath : null;
    }
    private void TurnOnMusic_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.BackMusicEntry.Enabled = (bool)TurnOnMusic.IsChecked;
            Config.Config.SaveConfig();
            UpdateMusic();
        }
    }

    private async void ChooseMusicBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var path = await SelectSingleMp3File();
        MusicPathBox.Text = Path.GetFullPath(path);
        
        Config.Config.MainConfig.BackMusicEntry.Path = Path.GetFullPath(path);
        Config.Config.SaveConfig();
        UpdateMusic();
    }

    private void MusicPathBox_OnTextInput(object? sender, TextInputEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.BackMusicEntry.Path = Path.GetFullPath(MusicPathBox.Text);
            Config.Config.SaveConfig();

            UpdateMusic();
        }
    }

    public void UpdateMusic()
    {
        if (Config.Config.MainConfig.BackMusicEntry.Enabled)
        {
            if (!String.IsNullOrWhiteSpace(Config.Config.MainConfig.BackMusicEntry.Path))
            {
                Models.Classes.Core.Music.Enabled = true;
                Models.Classes.Core.Music.Play(Config.Config.MainConfig.BackMusicEntry.Path);
            }
        }else Models.Classes.Core.Music.Stop();
    }
}