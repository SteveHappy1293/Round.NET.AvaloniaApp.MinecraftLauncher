using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using FluentAvalonia.FluentIcons;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Mange.TilesMange;

public class TilesMange
{
    public static List<StackPanel> TileGroups = new();
    // public static WrapPanel TilesPanel = Core.MainHome.WrapPanel;
    public static WrapPanel TilesPanel = new WrapPanel();
    public class TileItem
    {
        public enum TiteStyleType
        {
            Big,
            Small,
            Normal,
            Long
        }
        
        public string Tag { get; set; } = string.Empty;
        
        public TiteStyleType TiteStyle { get; set; } = TiteStyleType.Small;
        public Action TiteEvent { get; set; }
        public string Text { get; set; } = string.Empty;
        public Control Content { get; set; } = new FluentIcon()
        {
            Foreground = Brushes.White,
            Icon = FluentIconSymbol.Home20Regular
        };
        public IBrush BackGroundBrush { get; set; } = new SolidColorBrush(Colors.Black,0.3);
    }
    public static StackPanel RegisterTileGroup()
    {
        var tileGroup = new StackPanel();
        TilesPanel.Children.Add(tileGroup);
        return tileGroup;
    }
    public static void RegisterTile(StackPanel tileGroup,TileItem tileItem)
    {
        var tile = new Button()
        {
            Margin = new Thickness(2),
            Background = tileItem.BackGroundBrush,
            BorderThickness = new Thickness(0),
            Padding = new Thickness(0),
        };
        tile.Click += (_, __) => tileItem.TiteEvent();
        if (tileItem.TiteStyle != TileItem.TiteStyleType.Small)
        {
            tile.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            tile.VerticalContentAlignment = VerticalAlignment.Stretch;
            tile.Content = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0),
                Children =
                {
                    new Label()
                    {
                        Content = tileItem.Text,
                        Margin = new Thickness(4),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom,
                    },
                    tileItem.Content
                }
            };
        }
        else
        {
            tile.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            tile.VerticalContentAlignment = VerticalAlignment.Stretch;
            tile.Content = new Viewbox()
            {
                Height = 20,
                Width = 20,
                Child = tileItem.Content
            };
        }

        switch (tileItem.TiteStyle)
        {
            case TileItem.TiteStyleType.Small:
                tile.Width = 40;
                tile.Height = 40;
                break;
            case TileItem.TiteStyleType.Normal:
                tile.Width = 80;
                tile.Height = 80;
                break;
            case TileItem.TiteStyleType.Big:
                tile.Width = 160;
                tile.Height = 160;
                break;
            case TileItem.TiteStyleType.Long:
                tile.Width = 160;
                tile.Height = 80;
                break;
        }
/*
        if (Config.Config.MainConfig.HomeBody == 1)
        {
            tileGroup.Children.Add(tile);
        }
        */
    }
}