using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Manges;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class PlugMange : UserControl
{
    public PlugMange()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            PlugsBox.Children.Clear();
            foreach (var pl in PlugLoaderNeo.Plugs)
            {
                var sets = new Button()
                {
                    Content = new FluentIcon()
                    {
                        Icon = FluentIconSymbol.Settings20Regular
                    },
                    Margin = new Thickness(5),
                    Height = 32,
                    Width = 32
                };
                sets.Click += (_, __) =>
                {
                    /*var con = new ContentDialog()
                    {
                        PrimaryButtonText = "取消",
                        CloseButtonText = "确定",
                        DefaultButton = ContentDialogButton.Close,
                        Title = $"插件 {pl.Name} 设置",
                        Content = new SettingPlug(pl)
                    };
                    con.ShowAsync(Core.MainWindow);*/
                };
                PlugsBox.Children.Add(new ListBoxItem()
                {
                    Padding = new Thickness(5),
                    Content = new DockPanel()
                    {
                        Margin = new Thickness(5,0,0,0),
                        Children =
                        {
                            new DockPanel()
                            {
                                Children =
                                {
                                    new Border()
                                    {
                                        Background = new ImageBrush()
                                        {
                                            Source = pl.Icon
                                        },
                                        Width = 40,
                                        Height = 40,
                                        CornerRadius = new CornerRadius(6),
                                    },
                                    new StackPanel()
                                    {
                                        Margin = new Thickness(10,0),
                                        Children =
                                        {
                                            new Label()
                                            {
                                                Content = pl.Name,
                                                FontSize = 22,
                                                Margin = new Thickness(0,-3,0,0),
                                            },
                                            new Label()
                                            {
                                                Content = pl.Notes,
                                                FontSize = 15,
                                                FontStyle = FontStyle.Italic,
                                                Foreground = Brushes.DimGray,
                                                Margin = new Thickness(0,-10,0,0),
                                            },
                                        }
                                    }
                                }
                            },
                            new DockPanel()
                            {
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Center,
                                Children =
                                {
                                    sets
                                }
                            }
                        }
                    },
                });
            }
        };
    }
}