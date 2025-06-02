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
        if (entry.Tag != DefaultTag)
            ContentFrame.Navigate(entry.Page.GetType(), this, new EntranceNavigationTransitionInfo());
        else ContentFrame.Content = entry.Page;
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
            ContentFrame.Content = entry.Page;
        }

        btn.Click += NavigateTo;
        NavigationItems.Add(entry);
        NavigationButtons.Add(new BottomBarNavigationItemEntry()
        {
            Title = entry.Title,
            Page = entry.Page,
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