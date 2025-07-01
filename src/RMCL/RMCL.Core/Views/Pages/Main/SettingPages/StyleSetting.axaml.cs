using System.Collections.Generic;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Base.Enum.ButtonStyle;
using RMCL.Base.Interface;
using RMCL.Controls.View;
using RMCL.Core.Models.Classes.Manager.StyleManager;
using RMCL.Core.Views.Pages.Main.SettingPages.SettingSubPages;
using RMCL.Core.Models.Classes;
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
        MainButtonStyle.SelectedIndex = Config.Config.MainConfig.ButtonStyle.HomeButton.GetHashCode()-1;
        QuickChoosePlayerButtonStyle.SelectedIndex = Config.Config.MainConfig.ButtonStyle.QuickChoosePlayerButton.GetHashCode();
        ColorPicker.Color = Color.Parse(Config.Config.MainConfig.ThemeColors);
        IsEdit = true;
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
}