using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Interface;

namespace RMCL.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientArchiveSetting : ISetting ,IUISetting
{
    public void UpdateUI()
    {
        
    }
    public string Path { get; set; }
    public ClientArchiveSetting()
    {
        InitializeComponent();
    }
}