using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
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

        ColorThemeModel.SelectedIndex = Config.Config.MainConfig.ThemeColors.ColorType.GetHashCode();
        LightTheme.SelectedIndex = Config.Config.MainConfig.ThemeColors.Theme.GetHashCode();
        SystemHelper.GetSystemFonts.GetSystemFontFamilies().ForEach(x => ChoseLogViewFontBox.Items.Add(x));
        if (Config.Config.MainConfig.ThemeColors.ColorType == ColorType.System)
        {
            UserColorBox.IsVisible = false;
        }
        else
        {
            UserColorBox.IsVisible = true;
        }
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
        }
    }
}