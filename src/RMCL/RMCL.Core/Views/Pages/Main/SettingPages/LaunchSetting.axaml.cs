using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Entry.Game.Client;
using RMCL.Base.Enum.Client;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class LaunchSetting : ISetting
{
    public ClientConfig Config { get; set; }
    public Action<ClientConfig> OnSave { get; set; }
    public LaunchSetting()
    {
        InitializeComponent();
    }

    public void OnLoaded()
    {
        LogViewChoseBox.SelectedIndex = Config.LogViewShow.GetHashCode();
        WindowVisibilityChoseBox.SelectedIndex = Config.LauncherVisibility.GetHashCode();
        
        IsEdit = true;
    }
    private void LogViewChoseBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.LogViewShow = (LogViewShowEnum)LogViewChoseBox.SelectedIndex;
            OnSave.Invoke(Config);
        }
    }

    private void WindowVisibilityChoseBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.LauncherVisibility = (LauncherVisibilityEnum)WindowVisibilityChoseBox.SelectedIndex;
            OnSave.Invoke(Config);
        }
    }
}