using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class SeniorSetting : UserControl,IPage
{
    bool IsEditMode = false;
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{});
    }
    public SeniorSetting()
    {
        InitializeComponent();
        IsPlugUse.IsChecked = Config.MainConfig.IsUsePlug;        
        MsSlider.Value = (double)Config.MainConfig.MessageLiveTimeMs / 1000;
        MessageTimeLong.Description = $"通知停留时长 ({(double)Config.MainConfig.MessageLiveTimeMs/1000}s)：";
        IsAutoUpdata.IsChecked = Config.MainConfig.IsAutoUpdate;
        IsDebug.IsChecked = Config.MainConfig.IsDebug;
        UpdateLXBox.SelectedIndex = Config.MainConfig.UpdateSourse;
        
        this.Loaded += (_, __) => IsEditMode = true;
    }

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEditMode)
        {
            Config.MainConfig.IsUsePlug = (bool)((ToggleSwitch)sender).IsChecked;
            Config.SaveConfig();   
        }
    }

    private void RangeBase_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEditMode)
        {
            Config.MainConfig.MessageLiveTimeMs = (int)(e.NewValue*1000);
            Config.SaveConfig();
            MessageTimeLong.Description = $"通知停留时长 ({(double)Config.MainConfig.MessageLiveTimeMs/1000}s)：";
        }
    }

    private void IsAutoUpdata_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEditMode)
        {
            Config.MainConfig.IsAutoUpdate = (bool)((ToggleSwitch)sender).IsChecked;
            Config.SaveConfig();
        }
    }

    private void IsDebug_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEditMode)
        {
            Config.MainConfig.IsDebug = (bool)((ToggleSwitch)sender).IsChecked;
            Config.SaveConfig();
        }
    }

    private void UpdateLXBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEditMode)
        {
            var tag = (((ComboBox)sender).SelectedItem as ComboBoxItem).Tag as string;

            var ind = tag switch
            {
                "Github" => 0,
                "Auto" => 1
            };
            Config.MainConfig.UpdateSourse = ind;
            Config.SaveConfig();
        }
    }
}