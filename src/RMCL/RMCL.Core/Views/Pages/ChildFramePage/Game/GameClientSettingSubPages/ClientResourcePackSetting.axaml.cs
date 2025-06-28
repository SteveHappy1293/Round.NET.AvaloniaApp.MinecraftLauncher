using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientResourcePackSetting : ISetting ,IUISetting
{
    public void UpdateUI()
    {
        
    }
    public string Path { get; set; }
    public ClientResourcePackSetting()
    {
        InitializeComponent();
    }
}