using System.Collections.Generic;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Interop;
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
using Color = Avalonia.Media.Color;

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
        LightTheme.SelectedIndex = Config.Config.MainConfig.Theme.GetHashCode();
        if (Config.Config.MainConfig.ThemeColors.ColorType == ColorType.System)
        {
            UserColorBox.IsVisible = false;
        }
        else
        {
            UserColorBox.IsVisible = true;
        }
        IsEdit = true;

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
            Config.Config.MainConfig.Theme = (ThemeType)LightTheme.SelectedIndex;
            Config.Config.SaveConfig();
            
            StyleManager.UpdateBackground();
        }
    }
}