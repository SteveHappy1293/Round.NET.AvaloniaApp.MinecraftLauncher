using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class ConfigSetting : UserControl
{
    public bool IsEdit { get; set; } = false;
    public ConfigSetting()
    {
        InitializeComponent();
        OrgToggleSwitch.IsChecked = Config.MainConfig.IsUseOrganizationConfig;
        UrlBox.IsEnabled = Config.MainConfig.IsUseOrganizationConfig;
        UrlTextBox.Text = Config.MainConfig.OrganizationUrl;
        IsEdit = true;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.IsUseOrganizationConfig = (bool)OrgToggleSwitch.IsChecked;
            Config.SaveConfig();

            UrlBox.IsEnabled = Config.MainConfig.IsUseOrganizationConfig;
        }
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (IsEdit)
        {
            var textBox = (TextBox)sender;
            Config.MainConfig.OrganizationUrl = textBox.Text;
            Config.SaveConfig();
        }
    }
}