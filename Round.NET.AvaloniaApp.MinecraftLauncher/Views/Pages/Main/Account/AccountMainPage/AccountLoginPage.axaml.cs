using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.RSAccount;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountMainPage;

public partial class AccountLoginPage : UserControl
{
    public AccountLoginPage()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var acc = new LoginAccount();
        acc.Login();
        
        var con = new ContentDialog()
        {
            Title = "登录 Round Studio 账户",
            Content = new StackPanel()
            {
                Children =
                {
                    new DockPanel()
                    {
                        Children =
                        {
                            new ProgressRing(){ HorizontalAlignment = HorizontalAlignment.Left },
                            new TextBlock(){ Text = "登录中...",HorizontalAlignment = HorizontalAlignment.Right,VerticalAlignment = VerticalAlignment.Center}
                        },Margin = new Thickness(5)
                    },
                    new TextBlock(){Text = "已自动打开浏览器进行登录！",Margin = new Thickness(5)},
                    new HyperlinkButton() { Content = "没打开浏览器？点此手动打开",NavigateUri = new Uri(acc.LocalIPAddress),Margin = new Thickness(5)},
                    new TextBlock(){Text = "等待用户登录获取令牌...",Margin = new Thickness(5)}
                }
            },
            CloseButtonText = "取消"
        };
        con.CloseButtonClick += (_, _) => acc.Server.Stop();
        acc.Server.LogiedAction = new Action<string>((uuid) =>
        {
            Config.MainConfig.RSAccount = uuid;
            Config.SaveConfig();
            con.Hide();
            
            ((Main.Account.Account)((Grid)((Frame)this.Parent).Parent).Parent).Load();
        });
        con.ShowAsync();
    }
}