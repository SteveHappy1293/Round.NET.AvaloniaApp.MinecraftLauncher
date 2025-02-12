using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using MinecraftLaunch.Classes.Models.Auth;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.UserLogin;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using User = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.User;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class UserMange : UserControl
{
    bool IsEdit = false;
    public UserMange()
    {
        InitializeComponent();

        int count = 0;
        Task.Run(() => // 刷新页面
        {
            while (true)
            {
                if (User.Users.Count != count)
                {
                    count = User.Users.Count;
                    Dispatcher.UIThread.Invoke(()=>RefreshUI());
                }
                Thread.Sleep(100);
            }
        });
    }
    public void RefreshUI()
    {
        IsEdit = false;
        UsersBox.Items.Clear();
        foreach (var use in User.Users)
        {
            UsersBox.Items.Add(new ListBoxItem()
            {
                Content = new Grid()
                {
                    Height = 65,
                    Children =
                    {
                        new Label()
                        {
                            Content = use.Config.Username,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            VerticalContentAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(5),
                            FontSize = 22
                        },
                        new Label()
                        {
                            Content = $"登录模式：{use.Type}",
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            VerticalContentAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(5),
                            FontSize = 15,
                            FontStyle = FontStyle.Italic,
                            Foreground = Brushes.DimGray,
                        },
                        new DockPanel()
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Center,
                            Children =
                            {
                                new Button()
                                {
                                    Content = new HeroIcon()
                                    {
                                        Foreground = Brushes.White,
                                        Type = IconType.Cog8Tooth,
                                        Min = true
                                    },
                                    Margin = new Thickness(5),
                                    Height = 32,
                                    Width = 32
                                }
                            }
                        }
                    }
                },
            });
        }
        UsersBox.SelectedIndex = Config.MainConfig.SelectedUser;
        IsEdit = true;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var com = new ComboBox()
        {
            Items =
            {
                new ComboBoxItem()
                {
                    Content = "离线登录"
                },
                new ComboBoxItem()
                {
                    Content = "正版登录"
                }
            },
            SelectedIndex = 1,
            Height = 32
        };
        var login = new ContentDialog()
        {
            Title = "添加游戏账户",
            Content = new DockPanel()
            {
                Children =
                {
                    new Label()
                    {
                        Content = "请选择你的登录方式",
                        Margin = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    com
                }
            },
            CloseButtonText = "确定",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        };
        login.CloseButtonClick += async (_, __) =>
        {
            if (com.SelectedIndex == 1)
            {
                var login = new UserLogin.MicrosoftLogin();
                var load = new LoadingContentDialog().Show();
                var logincon = new ContentDialog();
                login.GetedCode += (code) =>
                {
                    load.Hide();
                    logincon = new ContentDialog()
                    {
                        Content = new StackPanel()
                        {
                            Children =
                            {
                                new DockPanel()
                                {
                                    Children =
                                    {
                                        new ProgressRing
                                        {
                                            HorizontalAlignment = HorizontalAlignment.Left,
                                            VerticalAlignment = VerticalAlignment.Top,
                                            Margin = new Thickness(10),
                                        },
                                        new Label()
                                        {
                                            Content = "正在登录中...",  
                                            HorizontalContentAlignment = HorizontalAlignment.Right,
                                            VerticalAlignment = VerticalAlignment.Center,
                                        },
                                    },
                                    HorizontalAlignment = HorizontalAlignment.Stretch
                                },
                                new TextBox()
                                {
                                    Text = code,
                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                    FontWeight = FontWeight.Bold,
                                    IsReadOnly = true,
                                },
                                new Label()
                                {
                                    Margin = new Thickness(5),
                                    Content = "请将这个代码复制到已打开的网页中!"
                                },
                                new Label()
                                {
                                    Margin = new Thickness(5),
                                    Content = "登录ID使用的是YMCL的正版登录ID，已经过YMCL作者（呆鱼）的同意",
                                    Foreground = Brushes.Gray,
                                }
                            },
                        },
                        CloseButtonText = "取消",
                        Title = "正版登录"
                    };
                    OpenUrl($"https://www.microsoft.com/link");
                    logincon.ShowAsync(Core.MainWindow);
                    
                    
                };
                login.LoggedIn += (mi) =>
                {
                    logincon.Hide();
                    Modules.Message.Message.Show("账户管理",$"账户 {mi.Name} 已添加到用户管理中!\n登录模式：正版登录",InfoBarSeverity.Success);
                    Console.WriteLine(mi.Name);
                    User.AddAccount(mi);
                };
                try
                {
                    await login.Login();
                }
                catch (Exception e)
                {
                    Modules.Message.Message.Show("账户管理",$"无法通过正版登录！\n{e.Message}",InfoBarSeverity.Error);
                    logincon.Hide();
                }
            }
            else
            {
                var name =
                    new TextBox()
                    {
                        Width = 120,
                        Margin = new Thickness(5),
                    };
                var ofl = new ContentDialog()
                {
                    Title = "添加离线账户",
                    Content = new DockPanel()
                    {
                        Children =
                        {
                            new Label()
                            {
                                Content = "离线用户名",
                                Margin = new Thickness(5),
                                VerticalAlignment = VerticalAlignment.Center,
                            },
                            name
                        }
                    },
                    CloseButtonText = "添加",
                    PrimaryButtonText = "取消",
                    DefaultButton = ContentDialogButton.Close
                };
                ofl.CloseButtonClick += (_, __) =>
                {
                    if (!string.IsNullOrEmpty(name.Text))
                    {
                        User.AddAccount(new OfflineAccount()
                        {
                            Name = name.Text
                        });
                        Modules.Message.Message.Show("账户管理",$"账户 {name.Text} 已添加到用户管理中!\n登录模式：离线登录",InfoBarSeverity.Success);

                    }
                    else
                    {
                        Modules.Message.Message.Show("账户管理",$"无法添加离线账户！\n原因：玩家名无效",InfoBarSeverity.Error);
                    }
                };
                ofl.ShowAsync(Core.MainWindow);
            }
        };
        
        login.ShowAsync(Core.MainWindow);
    }
    public void OpenUrl(string url)
    {
        try
        {
            // 使用 Process.Start 打开默认浏览器
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            // 如果 Process.Start 失败，可能是由于 UseShellExecute = true 在非 Windows 系统上不支持
            Console.WriteLine("Failed to open URL using Process.Start. Trying alternative method...");
            Console.WriteLine(ex.Message);

            // 尝试使用平台特定的命令
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // 在 Linux 上使用 xdg-open
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // 在 macOS 上使用 open
                Process.Start("open", url);
            }
            else
            {
                Console.WriteLine("Unsupported platform. Unable to open URL.");
            }
        }
    }
    private void UsersBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.SelectedUser = UsersBox.SelectedIndex;
            Config.SaveConfig();
        }
    }
}