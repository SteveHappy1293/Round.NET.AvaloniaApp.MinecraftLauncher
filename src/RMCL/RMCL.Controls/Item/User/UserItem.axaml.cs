using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using LiteSkinViewer2D;
using LiteSkinViewer2D.Extensions;
using OverrideLauncher.Core.Modules.Entry.AccountEntry;
using RMCL.Base.Entry.User;

namespace RMCL.Controls.Item.User;

public partial class UserItem : UserControl
{
    public UserItem(UserEntry entry)
    {
        InitializeComponent();

        PlayerName.Text = entry.Account.UserName;
        PlayerType.Text = entry.Account.AccountType switch
        {
            "msa" => "微软正版",
            "off" => "离线账户"
        };

        var croppedImage = HeadCapturer.Default.Capture(SkinHelper.Base64ToSKBitmap(entry.Skin)).ToBitmap();
        SkinHeader.Background = new ImageBrush()
        {
            Source = croppedImage,
            Stretch = Stretch.UniformToFill
        };
    }
}