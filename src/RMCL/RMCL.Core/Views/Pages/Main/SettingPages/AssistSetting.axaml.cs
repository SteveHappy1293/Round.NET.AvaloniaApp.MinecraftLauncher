using Avalonia;
using Avalonia.Controls;
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
}