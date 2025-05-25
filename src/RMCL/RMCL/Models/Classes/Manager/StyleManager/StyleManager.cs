using System;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Config;

namespace RMCL.Models.Classes.Manager.StyleManager;

public class StyleManager
{
    public static void UpdateBackground()
    {
        Core.MainWindow.Background = Brushes.Transparent;
        Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
        Core.MainWindow.InvalidateVisual();

        switch (Config.Config.MainConfig.Background.ChooseModel)
        {
            case BackgroundModelEnum.None:
                Core.MainWindow.Background = Config.Config.MainConfig.Theme == ThemeType.Dark ? Brush.Parse("#161616") : Brushes.White;
                break;
            case BackgroundModelEnum.Mica:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
                break;
            case BackgroundModelEnum.AcrylicBlur:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
                break;
            case BackgroundModelEnum.Glass:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Blur };
                Core.MainWindow.Background = new SolidColorBrush()
                {
                    Color = Color.Parse(Config.Config.MainConfig.Background.ColorGlassEntry.HtmlColor)
                };

                break;
        }
    }

    public static Type GetObjectType(object obj)
    {
        return obj.GetType();
    }
}