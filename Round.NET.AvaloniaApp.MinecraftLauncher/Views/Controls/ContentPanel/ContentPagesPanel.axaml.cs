using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.ContentPanel;

public partial class ContentPagesPanel : UserControl
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ContentPagesPanel, string>(
            nameof(Title),"");
    
    public static readonly StyledProperty<string> DefaultRouteProperty =
        AvaloniaProperty.Register<ContentPagesPanel, string>(
            nameof(DefaultRoute),"");

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string DefaultRoute
    {
        get => GetValue(DefaultRouteProperty);
        set => SetValue(DefaultRouteProperty, value);
    }
    
    public ContentPagesPanel()
    {
        InitializeComponent();
    }
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var item = ((DockPanel)((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content);
        var text = ((TextBlock)(item.Children[1])).Text;
        ControlChange.ChangeLabelText(PageTitleLabel,text);
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(4,4,4,-14));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                var page = RouteConfigs.Find(config =>
                {
                    return config.Route == ((NavigationViewItem)(View!).SelectedItem!).Tag;
                }).Page;
                MangeFrame.Content = page;
                page.Open();
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(4));
        });
    }

    public void ChangeSelectItemMenu()
    {
        if (View.SelectedItem != null)
        {
            var page = RouteConfigs.Find(config =>
            {
                return config.Route == ((NavigationViewItem)(View!).SelectedItem!).Tag;
            }).Page;
            page?.Open();
        }
    }

    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == DefaultRoute)
        {
            isthis = true;
        }

        var doc = new DockPanel()
        {
            Children =
            {
                new FluentIcon() { Margin = new Thickness(-5, 0, 0, 0), Icon = config.Icon, Width = 15, Height = 15 },
                new TextBlock() { Margin = new Thickness(10, 0, 0, 0), Text = config.Title, Name = "TitleLabel" }
            }
        };
        var item = new NavigationViewItem()
        {
            Tag = config.Route,
            Content = doc,
            IsSelected = isthis
        };
        if (config.IsFoot)
        {
            View.FooterMenuItems.Add(item);
        }
        else
        {
            View.MenuItems.Add(item);
        }
    }
}