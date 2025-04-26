using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class GameSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{});
    }
    public bool IsEdit { get; set; } = false;
    public GameSetting()
    {
        InitializeComponent();
        SetLangZHCN.IsChecked = Config.MainConfig.SetTheLanguageOnStartup;
        SetGammaTop.IsChecked = Config.MainConfig.SetTheGammaOnStartup;
        LogWindowStart.SelectedIndex = Config.MainConfig.GameLogOpenModlue;
        IsEdit = true; 
    }
    private void SetValue_OnClick(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.SetTheLanguageOnStartup = (bool)SetLangZHCN.IsChecked;
            Config.MainConfig.SetTheGammaOnStartup = (bool)SetGammaTop.IsChecked;
            Config.SaveConfig();
        }
    }

    private void LogWindowStart_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.GameLogOpenModlue = LogWindowStart.SelectedIndex;
            Config.SaveConfig();
        }
    }
}