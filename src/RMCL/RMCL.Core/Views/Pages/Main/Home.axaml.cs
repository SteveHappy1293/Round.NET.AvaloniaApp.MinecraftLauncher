using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Base.Enum.ButtonStyle;
using RMCL.Core.Models.Classes;

namespace RMCL.Core.Views.Pages.Main;

public partial class Home : UserControl
{
    public Home()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            if (Config.Config.MainConfig.ButtonStyle.QuickChoosePlayerButton == OrdinaryButtonStyle.None)
            {
                ChoosePlayer.IsVisible = false;
            }
            else
            {
                ChoosePlayer.IsVisible = true;
            }
        };
    }
}