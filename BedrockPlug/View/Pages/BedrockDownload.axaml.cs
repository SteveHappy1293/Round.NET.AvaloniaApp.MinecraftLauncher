using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Highlighting;
using BedrockPlug.View.Controls;
using FluentAvalonia.FluentIcons;
using MCLauncher.Versions;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace BedrockPlug.View.Pages;

public partial class BedrockDownload : UserControl
{
    public BedrockDownload()
    {
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            Task.Run(() =>
            {
                UpdateVersionList();
            });
        };
    }
    private async Task UpdateVersionList()
    {
        var vers = await Versions.GetAllVersions();
        Dispatcher.UIThread.Invoke(() =>
        {
            this.Panel1.Children.Clear();
            for (var i=0;i>=vers.Count;i++)
            {
                var ver = vers[i];
                if(ver.Revision!="0") continue;
                var downl = new Button()
                {
                    Content = new FluentIcon()
                    {
                        Icon = FluentIconSymbol.ArrowDownload20Filled
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
                this.Panel1.Children.Add(new ListBoxItem()
                {
                    Opacity = 0,
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
            LoadingControl.IsVisible = false;
        });
    }
}