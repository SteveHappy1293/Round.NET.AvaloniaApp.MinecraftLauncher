using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
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
            foreach (var pl in PlugsLoader.Plugs)
            {
                var sets = new Button()
                {
                    Content = new HeroIcon()
                    {
                        Foreground = Brushes.White,
                        Type = IconType.Cog6Tooth,
                        Min = true
                    },
                    Margin = new Thickness(5),
                    Height = 32,
                    Width = 32
                };
                sets.Click += (_, __) =>
                {
                    var con = new ContentDialog()
                    {
                        PrimaryButtonText = "取消",
                        CloseButtonText = "确定",
                        DefaultButton = ContentDialogButton.Close,
                        Title = $"插件 {Path.GetFileName(pl.FileName)} 设置",
                        Content = new SettingPlug(pl)
                    };
                    con.ShowAsync(Core.MainWindow);
                };
                PlugsBox.Children.Add(new ListBoxItem()
                {
                    Content = new Grid()
                    {
                        Height = 65,
                        Children =
                        {
                            new Label()
                            {
                                Content = Path.GetFileName(pl.FileName),
                                HorizontalContentAlignment = HorizontalAlignment.Left,
                                VerticalContentAlignment = VerticalAlignment.Top,
                                Margin = new Thickness(5),
                                FontSize = 22
                            },
                            new Label()
                            {
                                Content = pl.Title,
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