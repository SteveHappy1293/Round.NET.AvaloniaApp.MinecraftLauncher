using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.User.Player;

public partial class PlayerBox : UserControl
{
    public Modules.Game.User.User.UserConfig UserConfig { get; set; } = null;
    public int ThisIndex { get; set; } = 0;
    private string SteveBase64String = "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";
    public PlayerBox()
    {
        InitializeComponent();
        Task.Run(() =>
        {
            while (true)
            {
                if (Config.MainConfig.SelectedUser == ThisIndex)
                {
                    Dispatcher.UIThread.Invoke(() => MainButton.BorderBrush = Brushes.Green);
                    Dispatcher.UIThread.Invoke(() => MainButton.BorderThickness = new Thickness(2));
                }
                else
                {
                    Dispatcher.UIThread.Invoke(() => MainButton.BorderBrush = Brushes.Transparent);
                    Dispatcher.UIThread.Invoke(() => MainButton.BorderThickness = new Thickness(0));
                }
                Thread.Sleep(100);
            }
        });
    }

    public async Task Show(Modules.Game.User.User.UserConfig User)
    {
        UserConfig = User;
        NameLabel.Content = User.Config.Username;
        byte[] imageBytes;
        switch (User.Type)
        {
            case "Offline":
                LoginLabel.Foreground = Brushes.Orange;
                LoginLabel.Content = "离线账户";
                imageBytes = Convert.FromBase64String(SteveBase64String);

                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    SkinHandImage.Background = new ImageBrush(new Bitmap(memoryStream));
                }
                break;
            case "Microsoft":
                LoginLabel.Foreground = Brushes.Green;
                LoginLabel.Content = "微软正版账户";
                SkinHandImage.IsVisible = false;
                Task.Run(async () =>
                {
                    if (User.Head != null && User.Head != String.Empty)
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            SkinHandImage.IsVisible = true;
                            byte[] imageBytes = Convert.FromBase64String(User.Head);

                            using (var memoryStream = new MemoryStream(imageBytes))
                            {
                                SkinHandImage.Background = new ImageBrush(new Bitmap(memoryStream));
                            }
                        });
                    }
                    else
                    {
                        var skin = new global::MinecraftPlayerSkin();
                        skin.OnProgressUpdate += s =>
                        {
                            Console.WriteLine(s);
                        };
                        await skin.GetPlayerAttribute(User.Config.Username);

                        Dispatcher.UIThread.Invoke(() =>
                        {
                            SkinHandImage.IsVisible = true;
                            byte[] imageBytes = Convert.FromBase64String(skin.Head);
                            User.Head = skin.Head;
                            User.Skin = skin.Skin;
                            User.Body = skin.Body;
                            Modules.Game.User.User.SetUser(User.UUID,User);

                            using (var memoryStream = new MemoryStream(imageBytes))
                            {
                                SkinHandImage.Background = new ImageBrush(new Bitmap(memoryStream));
                            }
                        });
                    }
                });
                break;
            case "Mojang":
                LoginLabel.Foreground = Brushes.IndianRed;
                LoginLabel.Content = "Mojang正版账户";
                imageBytes = Convert.FromBase64String(SteveBase64String);

                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    SkinHandImage.Background = new ImageBrush(new Bitmap(memoryStream));
                }
                break;
            default:
                LoginLabel.Foreground = Brushes.Gainsboro;
                LoginLabel.Content = "未知账户";
                imageBytes = Convert.FromBase64String(SteveBase64String);

                using (var memoryStream = new MemoryStream(imageBytes))
                {
                    SkinHandImage.Background = new ImageBrush(new Bitmap(memoryStream));
                }
                break;
        }
    }

    private void ChoseThis_OnClick(object? sender, RoutedEventArgs e)
    {
        Config.MainConfig.SelectedUser = ThisIndex;
    }
}