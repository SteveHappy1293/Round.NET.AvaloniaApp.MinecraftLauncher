using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.RSAccount;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountMainPage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account.AccountSubPage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account;

public partial class Account : UserControl
{
    public AccountHomePage MyAccount { get; set; } = new AccountHomePage();
    public Account()
    {
        InitializeComponent();
        this.HomeIcon1.Click = (sender, args) =>
        {
            ((MainView)Core.MainWindow.Content).CortentFrame.Opacity = 0;
            ((MainView)Core.MainWindow.Content).CortentFrame.Content = new Grid();
            ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };

        this.Loaded += (s, e) =>
        {
            Load();
        };
    }

    public void Load()
    {
        if (Config.MainConfig.RSAccount != String.Empty)
        {
            MainFrame.Content = MyAccount;
        }
        else
        {
            MainFrame.Content = new AccountLoginPage();
        }
    }
}