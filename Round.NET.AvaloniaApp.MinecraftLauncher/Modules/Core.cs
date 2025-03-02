using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

public class Core
{
    public static MainWindow MainWindow { get; set; }
    public static SystemTaskBox SystemTask { get; set; }
    public static SystemMessageBox SystemMessage { get; set; }
    public static Download DownloadPage { get; set; } = new();
    public static Views.Pages.Main.Mange MangePage { get; set; } = new();
    public static Setting SettingPage { get; set; } = new();
    public static SystemNavigationBar NavigationBar { get; set; }
    public static List<API.SearchItemConfig> SearchItems { get; set; } = new();
    public class API
    {
        public class NavigationRouteConfig
        {
            public string Route { get; set; }
            public string Title { get; set; }
            public UserControl Page { get; set; }
            public IconType Icon { get; set; }
            public bool IsFoot { get; set; } = false;
        }
        public class SearchItemConfig
        {
            public enum ItemType
            {
                Mod,
                Setting,
                Manger,
                Game,
                More
            }
            public List<string> Key { get; set; } = new ();
            public string Title { get; set; } = string.Empty;
            public ItemType Type { get; set; } = ItemType.More;
            public Action<string> Action { get; set; }
        }
        public static void RegisterNavigationRoute(NavigationRouteConfig config)
        {
            NavigationBar.RegisterRoute(config);
        } // 注册边栏
        public static void RegisterDownloadPage(NavigationRouteConfig config)
        {
            DownloadPage.RegisterRoute(config);
        } // 注册下载页
        public static void RegisterMangePage(NavigationRouteConfig config)
        {
            MangePage.RegisterRoute(config);
        } // 注册管理页
        public static void RegisterSettingPage(NavigationRouteConfig config)
        {
            SettingPage.RegisterRoute(config);
        } // 注册设置页
        public static void RegisterSearchItem(SearchItemConfig config)
        {
            SearchItems.Add(config);
        } // 注册搜索项
    }
}