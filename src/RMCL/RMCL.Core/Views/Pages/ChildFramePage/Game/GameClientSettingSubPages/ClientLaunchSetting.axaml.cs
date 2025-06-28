using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OverrideLauncher.Core.Modules.Classes.Version;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientLaunchSetting : ISetting ,IUISetting
{
    public void UpdateUI()
    {
        
    }
    public VersionParse Version { get; set; }
    public ClientLaunchSetting()
    {
        InitializeComponent();
    }
}