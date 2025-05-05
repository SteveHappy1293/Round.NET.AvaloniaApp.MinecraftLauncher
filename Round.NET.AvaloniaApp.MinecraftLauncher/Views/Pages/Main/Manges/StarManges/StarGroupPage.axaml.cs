using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using FluentAvalonia.FluentIcons;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Mange.StarMange;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Dialog;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.StarManges;

public partial class StarGroupPage : UserControl
{
    public string GUID = string.Empty;
    public StarGroupPage()
    {
        InitializeComponent();
        this.HomeIcon1.Click = (sender, args) =>
        {
            Core.MainWindow.MainView.ContentFrame.Opacity = 0;
            Core.MainWindow.MainView.ContentFrame.Content = new Grid();
            Core.MainWindow.MainView.MainContent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };
    }

    public void Load()
    {
        var group = StarGroup.GetStarGroup(GUID);
        GroupTitle.Content = $"收藏夹 - {group.GroupName}";
        if (group.Stars.Count == 0)
        {
            NullControl.IsVisible = true;
        }

        foreach (var it in group.Stars)
        {
            StarsBox.Children.Add(GetStarGroupItem(it));
        }
    }

    public ListBoxItem GetStarGroupItem(StarItemEnrty enrty)
    {
        var res = new ListBoxItem()
        {
            Margin = new Thickness(0,0,0,10),
        };
        switch(enrty.Type)
        {
            case StarItemTypeEnum.GameVersion:
                 var launc = new Button()
                {
                    Content = new FluentIcon()
                    {
                        Icon = FluentIconSymbol.Airplane20Regular,
                        Margin = new Thickness(-10),
                    },
                    Margin = new Thickness(5),
                    Height = 32,
                    Width = 32
                };
                launc.Click += (_, __) =>
                {
                   // var dow = new LaunchGameTask(enrty.SourceData.Split('|')[1],enrty.SourceData.Split('|')[0]);
                    //SystemMessageTaskMange.AddTask(dow);
                };

                res.Content = new Grid()
                {
                    Height = 65,
                    Children =
                    {
                        new Label()
                        {
                            Content = enrty.SourceData.Split('|')[1],
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
                                launc
                            }
                        }
                    }
                };
                break;
        }

        return res;
    }
}