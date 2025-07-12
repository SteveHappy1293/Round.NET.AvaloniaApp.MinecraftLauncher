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
    public EventHandler OnChanged { get; set; } = ((sender, args) => { });
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
    
    public bool IsEdit { get; set; } = true;

    public void NavigateTo(string route)
    {
        IsEdit = false;
        var i = 0;
        RouteConfigs.ForEach(config =>
        {
            if (config.Route == route)
            {
                Navigate(config.Page);
                View.SelectedItem = View.MenuItems[i];
            }

            i++;
        });
        IsEdit = true;
    }

    private void Navigate(UserControl page)
    {
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(4,4,4,-10));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                MangeFrame.Content = page;
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(4,4,4,-4));
        });
    }
    
    public List<NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            var page = RouteConfigs.Find(config =>
            {
                return config.Route == ((NavigationViewItem)(View!).SelectedItem!).Tag;
            }).Page;
            OnChanged.Invoke(page, null);
            Navigate(page);
        }
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