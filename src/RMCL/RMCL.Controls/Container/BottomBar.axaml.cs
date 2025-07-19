using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using RMCL.Controls.ControlHelper;

namespace RMCL.Controls.Container;

public partial class BottomBar : UserControl
{
    public List<BottomBarNavigationEntry> NavigationItems { get; private set; } = new();
    private List<BottomBarNavigationItemEntry> NavigationButtons { get; set; } = new();
    public string DefaultTag { get; set; } = "Launch";
    public Frame ContentFrame { get; set; }
    public string NowTag { get; set; }

    // 懒加载页面缓存
    private readonly Dictionary<string, UserControl> _pageCache = new();

    public BottomBar()
    {
        InitializeComponent();
    }

    public void NavigationTo(string Tag)
    {
        foreach (var it in BtnBox.Children)
        {
            var btn = (Avalonia.Controls.Button)it;
            NavigationButtons.Find(x => x.NavItem.Tag == btn.Tag).IsThis = false;
            btn.Classes.Clear();
        }

        var entry = NavigationItems.Find(x => x.Tag == Tag);

        Nav(entry);
    }

    private void Nav(BottomBarNavigationEntry entry)
    {
        UserControl pageInstance;

        // 懒加载：检查缓存中是否已有页面实例
        if (_pageCache.TryGetValue(entry.Tag, out pageInstance))
        {
            // 使用缓存的页面实例
            if (entry.Tag != DefaultTag)
                ContentFrame.Navigate(pageInstance.GetType(), this, new EntranceNavigationTransitionInfo());
            else
                ContentFrame.Content = pageInstance;
        }
        else
        {
            // 首次访问，创建页面实例并缓存
            if (entry.PageFactory != null)
            {
                pageInstance = entry.PageFactory();
                _pageCache[entry.Tag] = pageInstance;
            }
            else
            {
                // 兼容旧的直接页面实例方式
                pageInstance = entry.Page;
                if (pageInstance != null)
                    _pageCache[entry.Tag] = pageInstance;
            }

            if (pageInstance != null)
            {
                if (entry.Tag != DefaultTag)
                    ContentFrame.Navigate(pageInstance.GetType(), this, new EntranceNavigationTransitionInfo());
                else
                    ContentFrame.Content = pageInstance;
            }
        }
    }
    public void RegisterNavigationItem(BottomBarNavigationEntry entry)
    {
        var btn = new Avalonia.Controls.Button()
        {
            Content = entry.Title,
            Tag = Guid.NewGuid().ToString()
        };
        if (entry.IsDefault)
        {
            btn.Classes.Add("accent");
            // 对于默认页面，立即创建实例
            UserControl defaultPage;
            if (entry.PageFactory != null)
            {
                defaultPage = entry.PageFactory();
                _pageCache[entry.Tag] = defaultPage;
            }
            else
            {
                defaultPage = entry.Page;
                if (defaultPage != null)
                    _pageCache[entry.Tag] = defaultPage;
            }
            ContentFrame.Content = defaultPage;
        }

        btn.Click += NavigateTo;
        NavigationItems.Add(entry);
        NavigationButtons.Add(new BottomBarNavigationItemEntry()
        {
            Title = entry.Title,
            Page = entry.Page,
            PageFactory = entry.PageFactory,
            NavItem = btn,
            IsDefault = entry.IsDefault,
        });

        if (!entry.IsNoButton) BtnBox.Children.Add(btn);
    }
    public void NavigateTo(object? sender, RoutedEventArgs e)
    {
        var btnhost = (Avalonia.Controls.Button)sender;
        foreach (var it in BtnBox.Children)
        {
            var btn = (Avalonia.Controls.Button)it;
            NavigationButtons.Find(x => x.NavItem.Tag == btn.Tag).IsThis = false;
            btn.Classes.Clear();
        }

        var entry = NavigationButtons.Find(x => x.NavItem.Tag == btnhost.Tag);

        entry.IsThis = true;
        entry.NavItem.Classes.Add("accent");
        
        Nav(entry);
    }
}