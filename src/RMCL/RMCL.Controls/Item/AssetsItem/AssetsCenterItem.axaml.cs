using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Base.Entry.Assets.Center;

namespace RMCL.Controls.Item.AssetsItem;

public partial class AssetsCenterItem : UserControl
{
    public AssetsCenterItem()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void LoadShow(AssetsIndexItemEntry item)
    {
        AssetsName.Text = item.Name;
        AssetsProfile.Text = item.Description;
    }
}