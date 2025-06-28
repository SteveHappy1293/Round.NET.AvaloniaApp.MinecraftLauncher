using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using RMCL.Base.Interface;
using RMCL.Core.Models.Classes.Manager.StyleManager;

namespace RMCL.Core.Views.Pages.Main.SettingPages.SettingSubPages;

public partial class ColorGlassSetting : ISetting
{
    public ColorGlassSetting()
    {
        InitializeComponent();
        this.ColorPicker.Color = Color.Parse(Config.Config.MainConfig.Background.ColorGlassEntry.HtmlColor);
        IsEdit = true;
    }

    private void ColorView_OnColorChanged(object? sender, ColorChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.Background.ColorGlassEntry.HtmlColor = this.ColorPicker.Color.ToString();
            Config.Config.SaveConfig();
            
            StyleManager.UpdateBackground();
        }
    }
}