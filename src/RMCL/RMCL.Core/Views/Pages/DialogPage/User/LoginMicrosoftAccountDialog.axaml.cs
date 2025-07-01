using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RMCL.Base.Entry.User;
using RMCL.Core.Models.Classes;
using RMCL.Core.Models.Classes.Manager.UserManager;

namespace RMCL.Core.Views.Pages.DialogPage.User;

public partial class LoginMicrosoftAccountDialog : UserControl
{
    public LoginMicrosoftAccountDialog()
    {
        InitializeComponent();
    }

    public async Task Login()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            LoadingBox.IsVisible = true;
            ContentBox.IsVisible = false;
        });
        var msaLogin = new MicrosoftAuthenticator("c06d4d68-7751-4a8a-a2ff-d1b46688f428");
        msaLogin.Login = entry =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                LoadingBox.IsVisible = false;
                ContentBox.IsVisible = true;

                LoginLink.Text = entry.URL;
                LoginCode.Text = entry.Code;
            });

            SystemHelper.SystemHelper.OpenUrl(entry.URL);
        };

        var login = await msaLogin.Authenticator();
        var skinurl = await UserInfoSerch.GetSkinUrlByUuid(login.UUID);
        PlayerManager.Player.Accounts.Add(new UserEntry()
        {
            Account = login,
            Skin = await UserInfoSerch.GetImageAsBase64Async(skinurl),
            SkinUrl = skinurl
        });
        PlayerManager.SaveConfig();
    }
}