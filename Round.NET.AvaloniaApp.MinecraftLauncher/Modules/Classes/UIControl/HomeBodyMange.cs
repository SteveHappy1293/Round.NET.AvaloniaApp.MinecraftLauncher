using Avalonia.Controls;
using Avalonia.Media;
using FluentAvalonia.FluentIcons;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Mange.TilesMange;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class HomeBodyMange
{
    public static void Load()
    {
        TilesMange.TilesPanel.Children.Clear();
        if (Config.Config.MainConfig.HomeBody == 1)
        {
            var group1 = TilesMange.RegisterTileGroup();
            TilesMange.RegisterTile(group1, new TilesMange.TileItem()
            {
                Text = "管理",
                TiteEvent = ()=>{
                    Core.NavigationBar.NavTo("Mange");
                },
                TiteStyle = TilesMange.TileItem.TiteStyleType.Long,
                Content = new FluentIcon()
                {
                    Foreground = Brushes.White,
                    Icon = FluentIconSymbol.Folder20Regular,
                    Width = 32,
                    Height = 32
                }
            });
            TilesMange.RegisterTile(group1, new TilesMange.TileItem()
            {
                Text = "设置",
                TiteEvent = ()=>{
                    Core.NavigationBar.NavTo("Setting");
                },
                TiteStyle = TilesMange.TileItem.TiteStyleType.Big,
                Content = new FluentIcon()
                {
                    Foreground = Brushes.White,
                    Icon = FluentIconSymbol.Settings20Regular,
                    Width = 32,
                    Height = 32
                }
            });
            TilesMange.RegisterTile(group1, new TilesMange.TileItem()
            {
                Text = "启动游戏",
                TiteEvent = ()=>{
                    Core.MainWindow.MainView.laun();
                },
                TiteStyle = TilesMange.TileItem.TiteStyleType.Small,
                Content = new FluentIcon()
                {
                    Foreground = Brushes.White,
                    Icon = FluentIconSymbol.Airplane24Regular,
                    Width = 32,
                    Height = 32
                }
            });
        }
        else if (Config.Config.MainConfig.HomeBody == 2)
        {
            
        }
    }
}