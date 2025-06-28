using Avalonia.Media;
using RMCL.Config;

namespace RMCL.Core.Models.Classes.Manager.StyleManager;

public class ColorHelper
{
    public static IBrush GetBackColor()
    {
        if (Config.Config.MainConfig.Theme == ThemeType.Dark)
        {
            return Brush.Parse("#373737");
        }
        else
        {
            return Brush.Parse("#CDCDCD");
        }
    }
}