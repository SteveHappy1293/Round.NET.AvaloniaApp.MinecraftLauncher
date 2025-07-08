using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FluentAvalonia.UI.Controls;
using OverrideLauncher.Core.Modules.Classes.Account;
using RMCL.Base.Entry.User;
using RMCL.Controls.Item;
using RMCL.Controls.Item.User;
using RMCL.Core.Models.Classes.Manager.UserManager;
using RMCL.Core.Views.Pages.DialogPage.User;

namespace RMCL.Core.Views.Pages.WizardPages;

public partial class WizardPlayer : UserControl
{
    public WizardPlayer()
    {
        InitializeComponent();
        UpdateUI();
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
        };
        await login.ShowAsync(Models.Classes.Core.MainWindow);
    }

    private async void MsaLogin_OnClick(object? sender, RoutedEventArgs e)
    {
        await MicrosoftLogin();
        UpdateUI();
    }

    private async void OffLogin_OnClick(object? sender, RoutedEventArgs e)
    {
        await OfflineLogin();
        UpdateUI();
    }

    public void UpdateUI()
    {
        UsersBox.Children.Clear();
        PlayerManager.Player.Accounts.ForEach(x =>
        {
            UsersBox.Children.Add(new ItemBox() { Content = new UserItem(x) });
        });
    }
}