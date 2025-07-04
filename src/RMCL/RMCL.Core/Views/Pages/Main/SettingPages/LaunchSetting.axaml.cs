using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Enum.Client;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class LaunchSetting : ISetting
{
    public LaunchSetting()
    {
        InitializeComponent();

        LogViewChoseBox.SelectedIndex = Config.Config.MainConfig.PublicClietConfig.LogViewShow.GetHashCode();
        WindowVisibilityChoseBox.SelectedIndex = Config.Config.MainConfig.PublicClietConfig.LauncherVisibility.GetHashCode();
        
        IsEdit = true;
    }

    private void LogViewChoseBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.PublicClietConfig.LogViewShow = (LogViewShowEnum)LogViewChoseBox.SelectedIndex;
            Config.Config.SaveConfig();
        }
    }

    private void WindowVisibilityChoseBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.PublicClietConfig.LauncherVisibility = (LauncherVisibilityEnum)WindowVisibilityChoseBox.SelectedIndex;
            Config.Config.SaveConfig();
        }
    }
}