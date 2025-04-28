using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class BottomBar : UserControl
{
    public List<BottomBarNavigationEntry> NavigationItems { get; private set; } = new();
    private List<BottomBarNavigationItemEntry> NavigationButtons { get; set; } = new();
    public string DefaultTag { get; set; } = "Launch";
    public Frame ContentFrame { get; set; }
    public BottomBar()
    {
        InitializeComponent();
        Core.BottomBar = this;
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
        if (entry.Tag != "Launch")
        {
            Core.MainWindow.BackGrid.Height = Core.MainWindow.Bounds.Height - 70 - 48;
            Task.Run(() =>
            {
                Thread.Sleep(200);

                Dispatcher.UIThread.Invoke(() =>
                    Core.MainWindow.BackGrid.VerticalAlignment = VerticalAlignment.Stretch);

                Dispatcher.UIThread.Invoke(() =>
                    Core.MainWindow.BackGrid.Height = double.NaN);
            });
        }
        else
        {
            Core.MainWindow.BackGrid.Height = 0;
            Core.MainWindow.BackGrid.VerticalAlignment = VerticalAlignment.Bottom;
        }
        
        entry.Page.Open();
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                ContentFrame.Opacity = 0.1;
                ContentFrame.Margin = new Thickness(0, 20, 0, 50);
            });
            Thread.Sleep(100);
            Dispatcher.UIThread.Invoke(() =>
            {
                ContentFrame.Content = entry.Page;
                ContentFrame.Opacity = 1;
                ContentFrame.Margin = new Thickness(0, 0, 0, 70);
            });
        });
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