using System;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Config;

namespace RMCL.Models.Classes.Manager.StyleManager;

public class StyleManager
{
    public static void UpdateBackground()
    {
        Core.MainWindow.Background = Brushes.Transparent;
        Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
        Core.MainWindow.InvalidateVisual();
        Core.MainWindow.BackOpacity.Opacity = 0;

        switch (Config.Config.MainConfig.Background.ChooseModel)
        {
            case BackgroundModelEnum.None:
                Core.MainWindow.Background = Config.Config.MainConfig.Theme == ThemeType.Dark ? Brush.Parse("#161616") : Brushes.AliceBlue;
                break;
            case BackgroundModelEnum.Mica:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
                break;
            case BackgroundModelEnum.AcrylicBlur:
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
                break;
            case BackgroundModelEnum.Glass:
                //Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Blur };
                Core.MainWindow.Background = new SolidColorBrush()
                {
                    Color = Color.Parse(Config.Config.MainConfig.Background.ColorGlassEntry.HtmlColor)
                };
                break;
            case BackgroundModelEnum.Image:
                if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex == -1) return;
                Core.MainWindow.Background = new ImageBrush()
                {
                    Source = new Bitmap(Config.Config.MainConfig.Background.ImageEntry.ImagePaths[Config.Config.MainConfig.Background.ImageEntry.ChooseIndex]),
                    Stretch = Stretch.UniformToFill
                };
                Core.MainWindow.BackOpacity.Opacity =
                    (double)(100 - Config.Config.MainConfig.Background.ImageEntry.Opacity) / 100;
                break;
        }
    }

    public static Type GetObjectType(object obj)
    {
        return obj.GetType();
    }
}