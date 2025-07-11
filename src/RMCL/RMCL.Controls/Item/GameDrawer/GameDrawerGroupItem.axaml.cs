using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Color = System.Drawing.Color;

namespace RMCL.Controls.Item.GameDrawer;

public partial class GameDrawerGroupItem : UserControl
{
    public Action<string> OnEdit = s => { };
    public Action<string> OnAdd = s => { };
    public Action<string,string> OnLaunch = (_,__) => { };
    public string GroupUUID
    {
        get
        {
            return _groupUUID;
        }
        set
        {
            _groupUUID = value;
            UpdateUI();
        }
    }

    private string _groupUUID { get; set; } = String.Empty;
    public GameDrawerGroupItem()
    {
        InitializeComponent();
    }

    public void UpdateUI()
    {
        var group = GameDrawerManager.GameDrawerManager.FindGroup(_groupUUID);
        GroupName.Text = group.Name;
        GroupName.BoxBackground = Brush.Parse(group.ColorHtmlCode);
        group.Children.ForEach(x =>
        {
            var it = new GameDrawerClientItem()
            {
                ParentGroupUUID = group.Uuid,
                ItemUUID = x.Uuid
            };
            it.OnLaunch = (s, s1) => OnLaunch.Invoke(s,s1);
            GroupPanel.Children.Add(it);
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OnEdit.Invoke(_groupUUID);
    }

    private void AddNewGame_OnClick(object? sender, RoutedEventArgs e)
    {
        OnAdd.Invoke(_groupUUID);
    }
}