using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using RMCL.Controls.ControlHelper;

namespace RMCL.Controls.Container;

public partial class NavigationPage : UserControl
{
    public NavigationPage()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<NavigationPage, string>(
            nameof(Title),"");
    
    public static readonly StyledProperty<string> DefaultRouteProperty =
        AvaloniaProperty.Register<NavigationPage, string>(
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
    
    public List<NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var item = ((DockPanel)((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content);
        var text = ((TextBlock)(item.Children[1])).Text;
        var page = RouteConfigs.Find(config =>
        {
            return config.Route == ((NavigationViewItem)(View!).SelectedItem!).Tag;
        }).Page;
        MangeFrame.Navigate(page.GetType());
    }

    public void RegisterRoute(NavigationRouteConfig config)
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