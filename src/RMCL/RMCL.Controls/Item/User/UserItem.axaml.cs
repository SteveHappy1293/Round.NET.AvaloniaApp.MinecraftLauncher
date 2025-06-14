using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
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
        
        var originalImage = SkinHelper.Base64ToBitmap(entry.Skin);
        var cropRect = new PixelRect(8, 8, 8, 8);

        var croppedImage = SkinHelper.CropAndScaleBitmapOptimized(originalImage, cropRect,4);
        SkinHeader.Background = new ImageBrush()
        {
            Source = croppedImage,
            Stretch = Stretch.UniformToFill
        };
    }
}