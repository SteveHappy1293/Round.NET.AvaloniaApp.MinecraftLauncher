using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using BedrockPlug.View.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using MCLauncher.Versions;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace BedrockPlug.View.Pages;

public partial class BedrockDownload : UserControl
{
    public BedrockDownload()
    {
        InitializeComponent();
        Task.Run(() =>
        { 
            UpdateVersionList();
        });
    }
    private async Task UpdateVersionList()
    {
        var vers = await Versions.GetAllVersions();
        Dispatcher.UIThread.Invoke(() =>
        {
            Panel.Children.Clear();
            foreach (var ver in vers)
            {
                if(ver.Revision!="0") continue;
                var downl = new Button()
                {
                    Content = new HeroIcon()
                    {
                        Foreground = Brushes.White,
                        Type = IconType.InboxArrowDown,
                        Min = true
                    },
                    Margin = new Thickness(5),
                    Height = 32,
                    Width = 32
                };
                downl.Click += (_, __) =>
                {
                    var dow = new DownloadControl(ver);
                    dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                    dow.Download();
                };
                Panel.Children.Insert(0,new ListBoxItem()
                {
                    Content = new Grid()
                    {
                        Height = 65,
                        Children =
                        {
                            new Label()
                            {
                                Content = ver.Version,
                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                VerticalContentAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(5),
                                FontSize = 22
                            },
                            new Label()
                            {
                                Content = "无描述文件...",
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
                                    downl
                                }
                            }
                        }
                    },
                });
            }
        });
    }
}