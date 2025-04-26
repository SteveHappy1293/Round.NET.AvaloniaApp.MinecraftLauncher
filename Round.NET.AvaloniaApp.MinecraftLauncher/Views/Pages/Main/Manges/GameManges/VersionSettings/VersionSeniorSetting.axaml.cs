using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.GameManges.VersionSettings;

public partial class VersionSeniorSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{new MenuItem{Header = "添加用户"},new MenuItem{Header = "刷新"}});
    }
    private string _version;

    public string version
    {
        get
        {
            return _version;
        }
        set
        {
            Dispatcher.UIThread.InvokeAsync(() => {
                _version = value;
            });
        }
    }

    public VersionSeniorSetting()
    {
        InitializeComponent();
    }
}