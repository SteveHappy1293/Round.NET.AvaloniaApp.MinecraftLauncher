using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls; 
using MinecraftLaunch.Base.Models.Authentication;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.User;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.UserLogin;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.User.Player;
using User = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.User;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class UserMange : UserControl,IPage
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
    /*public void RefreshUI()
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
                                    Content = new FluentIcon()
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
    }*/
    
    public void RefreshUI()
    {
        IsEdit = false;
        Users.Children.Clear();
        foreach (var use in User.Users)
        {
            var box = new PlayerBox();
            box.ThisIndex = User.Users.IndexOf(use);
            box.Show(use);
            Users.Children.Add(box);
        }
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
                    RLogs.WriteLog(mi.Name);
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
                        User.AddAccount(new OfflineAccount(name.Text,Guid.NewGuid(),Guid.NewGuid().ToString()));
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
            RLogs.WriteLog("Failed to open URL using Process.Start. Trying alternative method...");
            RLogs.WriteLog(ex.Message);

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
                RLogs.WriteLog("Unsupported platform. Unable to open URL.");
            }
        }
    }
    /*private void UsersBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.MainConfig.SelectedUser = UsersBox.SelectedIndex;
            Config.SaveConfig();
        }
    }*/
    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        User.LoadUser();
        RefreshUI();
    }

    private void GetUserSkin_OnClick(object? sender, RoutedEventArgs e)
    {
        var text = new TextBox()
        {
            Watermark = "输入正版账户名称...",
            Width = 300
        };
        var con = new ContentDialog()
        {
            Content = text,
            Title = "输入名称",
            PrimaryButtonText = "取消",
            CloseButtonText = "确定",
            DefaultButton = ContentDialogButton.Close
        };
        con.CloseButtonClick += (_, __) =>
        {
            var taskbox = new ContentDialog()
            {
                Content = "获取中...",
                Title = "稍等以下",
            };
            var user = text.Text;
            if(string.IsNullOrEmpty(user)) return;
            Task.Run(() =>
            {
                UserInfoSerch use = new UserInfoSerch();
                var uuid = use.GetUuidByUsername(user).Result;
                var url = use.GetSkinUrlByUuid(uuid).Result;
                Dispatcher.UIThread.Invoke(taskbox.Hide);
                ImageDownloader imageDownloader = new ImageDownloader(new HttpClient());
                imageDownloader.DownloadImageAsync(url,Path.GetFullPath($"../RMCL/RMCL.UserSkin/{uuid}"));
                
                Dispatcher.UIThread.Invoke(async () =>
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Title = "另存为皮肤文件",
                        InitialFileName = Path.GetFileName(imageDownloader.FilePath), // 默认文件名
                        DefaultExtension = Path.GetExtension(imageDownloader.FilePath),
                        Filters = new List<FileDialogFilter>
                        {
                            new FileDialogFilter { Name = "Minecraft 皮肤文件", Extensions = new List<string> { "png" } }
                        }
                    };

                    string? filePath = await saveFileDialog.ShowAsync(Core.MainWindow);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        File.Copy(imageDownloader.FilePath, filePath);
                    }
                });
            });
            taskbox.ShowAsync();
        };
        con.ShowAsync();
    }

    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>
        {
            ControlHelper.CreateMenuItem("添加用户",()=>Button_OnClick(null,null)),
            ControlHelper.CreateMenuItem("刷新",()=>Button_OnClick1(null,null)),
            ControlHelper.CreateMenuItem("获取正版账户皮肤",()=>GetUserSkin_OnClick(null,null))
        });
    }
}