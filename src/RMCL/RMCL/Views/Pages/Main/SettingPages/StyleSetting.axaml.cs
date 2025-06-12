using System.Collections.Generic;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Controls.View;
using RMCL.Models.Classes;
using RMCL.Models.Classes.Manager.StyleManager;
using RMCL.Views.Pages.Main.SettingPages.SettingSubPages;

namespace RMCL.Views.Pages.Main.SettingPages;

public partial class StyleSetting : UserControl
{
    public bool IsEdit { get; set; } = false;

    public Dictionary<BackgroundModelEnum, UserControl> BackgroundSettings { get; set; } = new()
    {
        { BackgroundModelEnum.Glass, new ColorGlassSetting() },
        { BackgroundModelEnum.Pack, new PackSetting() },
        { BackgroundModelEnum.Image, new ImageSetting() },
        { BackgroundModelEnum.None, new NullBox() { SmallTitle = "这里没有设置",Margin = new Thickness(10)} },
        { BackgroundModelEnum.Mica, new NullBox() { SmallTitle = "这里没有设置",Margin = new Thickness(10) } },
        { BackgroundModelEnum.AcrylicBlur, new NullBox() { SmallTitle = "这里没有设置",Margin = new Thickness(10) } }
    };
    public StyleSetting()
    {
        InitializeComponent();
        ChooseBackgroundModel.SelectedIndex = Config.Config.MainConfig.Background.ChooseModel.GetHashCode();
        var enums = (BackgroundModelEnum)ChooseBackgroundModel.SelectedIndex;
        Config.Config.MainConfig.Background.ChooseModel = enums;
        BackgroundMaxSetting.Content = BackgroundSettings[enums];
        MainButtonStyle.SelectedIndex = Config.Config.MainConfig.HomeButtonStyle.GetHashCode();
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
            Config.Config.MainConfig.HomeButtonStyle = (HomeButtonStyle)MainButtonStyle.SelectedIndex;
            
            Config.Config.SaveConfig();
            Core.MainWindow.UpdateButtonStyle();
        }
    }
}