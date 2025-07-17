using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using LiteSkinViewer2D;
using LiteSkinViewer2D.Extensions;
using OverrideLauncher.Core.Modules.Classes.Account;
using RMCL.Base.Entry.User;
using RMCL.Base.Enum.User;
using RMCL.Base.Interface;
using RMCL.Controls.Item.User;
using RMCL.Core.Models.Classes.Manager.UserManager;
using RMCL.Core.Views.Pages.DialogPage.User;
using RMCL.Core.Models.Classes;
using SkiaSharp;
using PointerType = LiteSkinViewer3D.Shared.Enums.PointerType;

namespace RMCL.Core.Views.Pages.Main.ManagePages;

public partial class ManageAccount : ISetting
{
    public ManageAccount()
    {
        InitializeComponent();
        this.Loaded += (sender, args) => RefreshUI();
    }

    public void RefreshUI()
    {
        IsEdit = false;
        UserListBox.Items.Clear();
        McSkinViewer3D.EnableAnimation =
            Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.EnableAnimations;
        SkinShowType.IsChecked= Config.Config.MainConfig.SkinRenderEntry.IsUse3D;
        if (PlayerManager.Player.Accounts.Count <= 0)
        {
            NullBox.IsVisible = true;
            SaveSkin.IsEnabled = false;
        }
        else
        {
            NullBox.IsVisible = false;
            SaveSkin.IsEnabled = true;
        }

        PlayerManager.Player.Accounts.ForEach(x =>
        {
            UserListBox.Items.Add(new ListBoxItem() { Content = new UserItem(x) });
        });
        UserListBox.SelectedIndex = PlayerManager.Player.SelectIndex;
        UpdateSkinUI();
        
        IsEdit = true;
    }

    private async void AddAccountBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var addUserDialog = new AddUserDialog();
        var con = new ContentDialog()
        {
            Content = addUserDialog,
            CloseButtonText = "选择",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close,
            Title = "新增账户"
        };
        con.CloseButtonClick += (s, e) =>
        {
            var UserType = addUserDialog.GetUserType();
            if (UserType == UserType.Microsoft)
            {
                MicrosoftLogin();
            }else if (UserType == UserType.Offline)
            {
                OfflineLogin();
            }
        };
        await con.ShowAsync(Models.Classes.Core.MainWindow);
    }

    public async Task MicrosoftLogin()
    {
        var logincon = new LoginMicrosoftAccountDialog();
        var login = new ContentDialog()
        {
            Content = logincon,
            Title = "微软正版登录",
            CloseButtonText = "取消"
        };
        login.ShowAsync(Models.Classes.Core.MainWindow);
        await logincon.Login();
        login.Hide();
        RefreshUI();
    }
    private static readonly ConcurrentDictionary<string, Bitmap> _imageCache = new ConcurrentDictionary<string, Bitmap>();

    public async Task OfflineLogin()
    {
        var logincon = new LoginOfflineAccountDialog();
        var login = new ContentDialog()
        {
            Content = logincon,
            Title = "新增离线账户",
            CloseButtonText = "添加",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        };
        login.CloseButtonClick += (s, e) =>
        {
            if(string.IsNullOrEmpty(logincon.GetUserName())) return;

            string base64str = "";
            if (string.IsNullOrEmpty(logincon.GetSkinPath()) || !File.Exists(logincon.GetSkinPath()))
            {
                string resourcePath = $"avares://RMCL.Core/Assets/Skin/Steve.png";
                var bitmap = _imageCache.GetOrAdd(resourcePath, key =>
                {
                    using var stream = AssetLoader.Open(new Uri(key));
                    return new Bitmap(stream); 
                });
                base64str = SkinHelper.BitmapToBase64(bitmap);
            }
            else
            {
                var bitmap = new Bitmap(logincon.GetSkinPath());
                base64str = SkinHelper.BitmapToBase64(bitmap);
            }

            PlayerManager.Player.Accounts.Add(new UserEntry()
            {
                Account = new OffineAuthenticator(logincon.GetUserName()).Authenticator(),
                SkinUrl = "",
                Skin = base64str
            });
            PlayerManager.SaveConfig();
            RefreshUI();
        };
        await login.ShowAsync(Models.Classes.Core.MainWindow);
    }
    private SKBitmap Base64ToSKBitmap(string base64)
    {
        var base64Data = base64.Split(',')[0];
        if (base64Data.Length == base64.Length)
        {
            base64Data = base64;
        }
        else
        {
            base64Data = base64.Substring(base64Data.Length + 1);
        }

        byte[] imageBytes = Convert.FromBase64String(base64Data);
        return SKBitmap.Decode(imageBytes);
    }
    private void UserListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            PlayerManager.Player.SelectIndex = UserListBox.SelectedIndex;
            PlayerManager.SaveConfig();

            // McSkinViewer2D.ShowSkin(PlayerManager.Player.Accounts[PlayerManager.Player.SelectIndex].Skin);
            UpdateSkinUI();
        }
    }

    private async void SaveSkin_OnClick(object? sender, RoutedEventArgs e)
    {
        // 获取当前窗口（TopLevel）
        var topLevel = TopLevel.GetTopLevel(this); // 假设在 Window/UserControl 内部
        if (topLevel == null)
            return;

        // 创建 SaveFileDialog 选项
        var options = new FilePickerSaveOptions
        {
            Title = "保存 Minecraft 皮肤文件",
            DefaultExtension = ".png",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("PNG 图片") { Patterns = new[] { "*.png" } },
            }
        };
        options.SuggestedFileName = $"{PlayerManager.Player.Accounts[UserListBox.SelectedIndex].Account.UUID}.png";
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(options);

        if (file == null)
            return; // 用户取消

        // 获取文件路径
        var filePath = file.Path.LocalPath;

        // 根据扩展名选择保存格式
        using (var stream = await file.OpenWriteAsync())
        {
            // 使用 Avalonia 的 Bitmap 保存方法
            SkinHelper.Base64ToBitmap(PlayerManager.Player.Accounts[UserListBox.SelectedIndex].Skin).Save(stream);

            Models.Classes.Core.MessageShowBox.AddInfoBar("保存文件", $"已保存至 {filePath}", InfoBarSeverity.Success);
        }
    }

    private void RefreshBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshUI();
    }

    private void SkinShowType_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.SkinRenderEntry.IsUse3D = (bool)SkinShowType.IsChecked;
            Config.Config.SaveConfig();
            
            UpdateSkinUI();
        }
    }

    public void UpdateSkinUI()
    {
        McSkinViewer3D.PointerMoved -= SkinViewer_PointerMoved;
        McSkinViewer3D.PointerPressed -= SkinViewer_PointerPressed;
        McSkinViewer3D.PointerReleased -= SkinViewer_PointerReleased;
        if (Config.Config.MainConfig.SkinRenderEntry.IsUse3D)
        {
            McSkinViewer2D.IsVisible = false;
            McSkinViewer3D.IsVisible = true;

            McSkinViewer3D.SkinBase64 = PlayerManager.Player.Accounts[UserListBox.SelectedIndex].Skin;
            McSkinViewer3D.ChangeSkin();
            
            McSkinViewer3D.PointerMoved += SkinViewer_PointerMoved;
            McSkinViewer3D.PointerPressed += SkinViewer_PointerPressed;
            McSkinViewer3D.PointerReleased += SkinViewer_PointerReleased;
        }
        else
        {
            McSkinViewer2D.IsVisible = true;
            McSkinViewer3D.IsVisible = false;
            
            McSkinViewer2D.Source =
                FullBodyCapturer.Default
                    .Capture(Base64ToSKBitmap(PlayerManager.Player.Accounts[UserListBox.SelectedIndex].Skin), 8)
                    .ToBitmap();
        }
    }
    
    private void SkinViewer_PointerReleased(object? sender, PointerReleasedEventArgs e) {
        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        PointerType type = PointerType.None;
        if (po.Properties.IsLeftButtonPressed) {
            type = PointerType.PointerLeft;
        } /*else if (po.Properties.IsRightButtonPressed) {
            type = PointerType.PointerRight;
        }*/

        McSkinViewer3D.UpdatePointerReleased(type,
            new((float)pos.X * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate,
                (float)pos.Y * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate));

    }

    private void SkinViewer_PointerPressed(object? sender, PointerPressedEventArgs e) {
        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        PointerType type = PointerType.None;
        if (po.Properties.IsLeftButtonPressed) {
            type = PointerType.PointerLeft;
        } /*else if (po.Properties.IsRightButtonPressed) {
            type = PointerType.PointerRight;
        }*/

        McSkinViewer3D.UpdatePointerPressed(type,
            new((float)pos.X * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate,
                (float)pos.Y * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate));
    }

    private void SkinViewer_PointerMoved(object? sender, PointerEventArgs e) {
        var po = e.GetCurrentPoint(this);
        var pos = e.GetPosition(this);

        PointerType type = PointerType.None;
        if (po.Properties.IsLeftButtonPressed) {
            type = PointerType.PointerLeft;
        } /*else if (po.Properties.IsRightButtonPressed) {
            type = PointerType.PointerRight;
        }*/

        McSkinViewer3D.UpdatePointerMoved(type,
            new((float)pos.X * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate,
                (float)pos.Y * Config.Config.MainConfig.SkinRenderEntry.Skin3DRenderConfigEntry.MovementRate));
    }
}