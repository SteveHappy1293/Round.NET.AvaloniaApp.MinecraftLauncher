using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

    public void UpdateUI()
    {
        ClientName.Text = GameDrawerManager.GameDrawerManager.FindGroup(ParentGroupUUID).Children
            .Find(x => x.Uuid == ItemUUID).ClientInfo.GameName;
    }
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if(!string.IsNullOrEmpty(ParentGroupUUID) && !string.IsNullOrEmpty(ItemUUID))
        {
            OnLaunch(ParentGroupUUID, ItemUUID);
        }
    }

    private void InputElement_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        MDockPanel.Classes.Remove("ReBlur");
        MDockPanel.Classes.Add("Blur");
        ControlBorder.IsVisible = true;
    }

    private void InputElement_OnPointerExited(object? sender, PointerEventArgs e)
    {
        MDockPanel.Classes.Remove("Blur");
        MDockPanel.Classes.Add("ReBlur");
        ControlBorder.IsVisible = false;
    }
}