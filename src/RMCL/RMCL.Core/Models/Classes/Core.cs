using System;
using FluentAvalonia.Styling;
using RMCL.AssetsPool;
using RMCL.Controls.Container;
using RMCL.Core.Views;
using RMCL.Core.Views.Pages.Main.MessageView;
using RMCL.Core.Views.Pages.TaskView;
using RMCL.MusicPlayer;

namespace RMCL.Core.Models.Classes;

public class Core
{
    public static MainWindow MainWindow;
    public static BottomBar BottomBar;
    public static TaskView TaskView;
    public static ChildFrame ChildFrame;
    public static MessageShowBox MessageShowBox;
    public static Music Music = new()
    {
        Loop = true
    };
    public static ImageResourcePool ImageResourcePool = new ImageResourcePool(
        inactiveTimeout: TimeSpan.FromSeconds(5),
        cleanupInterval: TimeSpan.FromSeconds(10));

    public static FluentAvaloniaTheme FluentAvaloniaTheme = new()
    {
        UseSystemFontOnWindows = true
    };
}