using FluentAvalonia.Styling;
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

    public static FluentAvaloniaTheme FluentAvaloniaTheme = new()
    {
        UseSystemFontOnWindows = true
    };
}