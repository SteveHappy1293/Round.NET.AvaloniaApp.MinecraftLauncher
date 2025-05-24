using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Enum;
using RMCL.Models.Classes.Manager.StyleManager;

namespace RMCL.Views.Pages.Main.SettingPages;

public partial class StyleSetting : UserControl
{
    public bool IsEdit { get; set; } = false;
    public StyleSetting()
    {
        InitializeComponent();
        IsEdit = true;
    }

    private void ChooseBackgroundModel_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.Background.ChooseModel = 
                ((ComboBoxItem)ChooseBackgroundModel.SelectedItem).Tag.ToString() switch
                {
                    "Mica"=>BackgroundModelEnum.Mica,
                    "AcrylicBlur"=>BackgroundModelEnum.AcrylicBlur,
                    "None"=>BackgroundModelEnum.None
                };
            StyleManager.UpdateBackground();
        }
    }
}