using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Item.GameDrawer;

public partial class GameDrawerClientItem : UserControl
{
    public Action<string, string> OnLaunch { get; set; } = (s, s1) => Console.WriteLine("[游戏抽屉] 未设置回调对象"); 
    public string ParentGroupUUID { get; set; } = String.Empty;
    public string ItemUUID { get; set; } = String.Empty;
    public GameDrawerClientItem()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if(!string.IsNullOrEmpty(ParentGroupUUID) && !string.IsNullOrEmpty(ItemUUID))
        {
            OnLaunch(ParentGroupUUID, ItemUUID);
        }
    }
}