using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class GameSetting : UserControl
{
    public bool IsEdit { get; set; } = false;
    public GameSetting()
    {
        InitializeComponent();
        Task.Run(() => {
            // 更新 UI
            Dispatcher.UIThread.Invoke(() =>
            {
                foreach (var java in FindJava.JavasList)
                {
                    JavaComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = $"[Java {java.JavaVersion}] {java.JavaPath}"
                    });
                }
                JavaComboBox.SelectedIndex = Config.MainConfig.SelectedJava;
            });  
        }); // 更新Java设置下拉框
        SetLangZHCN.IsChecked = Config.MainConfig.SetTheLanguageOnStartup;
        SetGammaTop.IsChecked = Config.MainConfig.SetTheGammaOnStartup;
        IsEdit = true;
    }

    private void JavaComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) //Java选择下拉框选择回调
    {
        Config.MainConfig.SelectedJava = JavaComboBox.SelectedIndex;
        Config.SaveConfig();
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
}