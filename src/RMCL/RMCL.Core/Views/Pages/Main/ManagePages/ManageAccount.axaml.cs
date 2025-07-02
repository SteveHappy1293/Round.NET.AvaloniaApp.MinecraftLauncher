using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using OverrideLauncher.Core.Modules.Classes.Account;
using RMCL.Base.Entry.User;
using RMCL.Base.Enum.User;
using RMCL.Base.Interface;
using RMCL.Controls.Item.User;
using RMCL.Core.Models.Classes.Manager.UserManager;
using RMCL.Core.Views.Pages.DialogPage.User;
using RMCL.Core.Models.Classes;

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
        ShowSkin();
        
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

    private void UserListBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            PlayerManager.Player.SelectIndex = UserListBox.SelectedIndex;
            PlayerManager.SaveConfig();

            ShowSkin();
        }
    }

    public void ShowSkin()
    {
        try
        {
            var skin = SkinHelper.Base64ToBitmap(PlayerManager.Player.Accounts[UserListBox.SelectedIndex].Skin);
            Bitmap leftarm = null;
            Bitmap rightarm = null;
            Bitmap leftfoot = null;
            Bitmap rightfoot = null;
            Bitmap body = null;
            Bitmap header = null;

            try { leftarm = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(44, 20, 4, 12), 8); } catch { }
            try { rightarm = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(36, 52, 4, 12), 8); } catch { }
            try { leftfoot = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(4, 20, 4, 12), 8); } catch { }
            try { rightfoot = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(20, 52, 4, 12), 8); } catch { }
            try { body = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(20, 20, 8, 12), 8); } catch { }
            try { header = SkinHelper.CropAndScaleBitmapOptimized(skin, new PixelRect(8, 8, 8, 8), 8); } catch { }

            try { LeftArm.Background = new ImageBrush() { Source = leftarm }; } catch { }
            try { LeftFoot.Background = new ImageBrush() { Source = leftfoot }; } catch { }
            try { RightArm.Background = new ImageBrush() { Source = rightarm }; } catch { }
            try { RightFoot.Background = new ImageBrush() { Source = rightfoot }; } catch { }
            try { Body.Background = new ImageBrush() { Source = body }; } catch { }
            try { Header.Background = new ImageBrush() { Source = header }; } catch { }
        }
        catch { }
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
}