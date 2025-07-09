using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class AssistSetting : ISetting
{
    public AssistSetting()
    {
        InitializeComponent();
        TopMostSwitch.IsChecked = Config.Config.MainConfig.LauncherWindowTopMost;
        SkinPreviewRateSlider.Value = Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate;
        SkinPreviewRate.Content = $"预览移动速率 (×{SkinPreviewRateSlider.Value})";
        EnablePreviewAnimations.IsChecked =
            Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.EnableAnimations;

        IsEdit = true;
    }

    private void TopMostSwitch_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.LauncherWindowTopMost = (bool)TopMostSwitch.IsChecked;
            Config.Config.SaveConfig();
            
            Core.Models.Classes.Core.MainWindow.Topmost = Config.Config.MainConfig.LauncherWindowTopMost;
        }
    }

    private void RangeBase_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEdit)
        {
            var Value = (int)SkinPreviewRateSlider.Value;
            IsEdit = false;
            SkinPreviewRateSlider.Value = Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate;
            IsEdit = true;
            
            Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate = Value;
            Config.Config.SaveConfig();

            SkinPreviewRate.Content = $"预览移动速率 (×{Value})";
        }
    }

    private void EnablePreviewAnimations_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.EnableAnimations =
                (bool)EnablePreviewAnimations.IsChecked;
            
            Config.Config.SaveConfig();
        }
    }
}