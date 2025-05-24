using System;
using Avalonia.Controls;
using Avalonia.Media;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;

namespace RMCL.Models.Classes.Manager.StyleManager;

public class StyleManager
{
    public static void UpdateBackground()
    {
        Core.MainWindow.Background = Brushes.Transparent;
        var obj = GetBackgroundDate();

        switch (Config.Config.MainConfig.Background.ChooseModel)
        {
            case BackgroundModelEnum.None:
                Core.MainWindow.Background = Brush.Parse("#161616");
                break;
            case BackgroundModelEnum.Mica:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
                break;
            case BackgroundModelEnum.AcrylicBlur:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
                break;
        }
        
        Config.Config.SaveConfig();
    }

    public static Type GetObjectType(object obj)
    {
        return obj.GetType();
    }

    public static object? GetBackgroundDate()
    {
        var mod = Config.Config.MainConfig.Background.ChooseModel;
        return mod switch
        {
            BackgroundModelEnum.None => null,
            BackgroundModelEnum.Mica => null,
            BackgroundModelEnum.AcrylicBlur => null,
            BackgroundModelEnum.Glass => Config.Config.MainConfig.Background.Data[3],
            BackgroundModelEnum.Image => Config.Config.MainConfig.Background.Data[4],
            BackgroundModelEnum.Color => Config.Config.MainConfig.Background.Data[5],
            BackgroundModelEnum.Pack => Config.Config.MainConfig.Background.Data[6],
            _ => null
        };
    }
}