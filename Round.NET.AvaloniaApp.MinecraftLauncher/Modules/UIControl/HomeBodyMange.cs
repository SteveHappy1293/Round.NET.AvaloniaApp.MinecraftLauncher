using Avalonia.Controls;
using Avalonia.Media;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
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
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.Folder
                }
            });
            TilesMange.RegisterTile(group1, new TilesMange.TileItem()
            {
                Text = "设置",
                TiteEvent = ()=>{
                    Core.NavigationBar.NavTo("Setting");
                },
                TiteStyle = TilesMange.TileItem.TiteStyleType.Big,
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.Cog6Tooth
                }
            });
            TilesMange.RegisterTile(group1, new TilesMange.TileItem()
            {
                Text = "启动游戏",
                TiteEvent = ()=>{
                    Core.MainHome.laun();
                },
                TiteStyle = TilesMange.TileItem.TiteStyleType.Small,
                Content = new HeroIcon()
                {
                    Foreground = Brushes.White,
                    Type = IconType.RocketLaunch
                }
            });
        }
        else if (Config.Config.MainConfig.HomeBody == 2)
        {
            
        }
    }
}