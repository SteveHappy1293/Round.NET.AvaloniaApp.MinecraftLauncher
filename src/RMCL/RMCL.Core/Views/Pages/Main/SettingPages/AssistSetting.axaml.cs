﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
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

        GCTimeChose.SelectedIndex = GetIndex(Config.Config.MainConfig.GCTime);

        TextRenderingMode.SelectedIndex = Config.Config.MainConfig.RenderModel.TextRenderingMode.GetHashCode();
        BitmapInterpolationMode.SelectedIndex = Config.Config.MainConfig.RenderModel.BitmapInterpolationMode.GetHashCode();
        EdgeMode.SelectedIndex = Config.Config.MainConfig.RenderModel.EdgeMode.GetHashCode();
        
        IsEdit = true;
    }

    public int GetIndex(int GCTime) => GCTime switch
    {
        3000 => 0,
        2500 => 1,
        2000 => 2,
        1500 => 3,
        1000 => 4
    };

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

    private void GCTimeChose_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.GCTime = int.Parse(((ComboBoxItem)GCTimeChose.SelectedItem).Tag.ToString());
            Config.Config.SaveConfig();
        }
    }

    private void Mode_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.RenderModel.TextRenderingMode =
                (Avalonia.Media.TextRenderingMode)TextRenderingMode.SelectedIndex;
            Config.Config.MainConfig.RenderModel.BitmapInterpolationMode =
                (BitmapInterpolationMode)BitmapInterpolationMode.SelectedIndex;
            Config.Config.MainConfig.RenderModel.EdgeMode =
                (Avalonia.Media.EdgeMode)EdgeMode.SelectedIndex;
            
            Config.Config.SaveConfig();
            
            Models.Classes.Core.MainWindow.UpdateRending();
        }
    }
}