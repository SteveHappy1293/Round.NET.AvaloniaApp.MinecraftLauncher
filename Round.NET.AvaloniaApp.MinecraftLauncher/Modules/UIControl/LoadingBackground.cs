using System;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class LoadingBackground
{
    public static void Load()
    {
        if (Config.Config.MainConfig.BackModlue == 1)
        {
            if (Config.Config.MainConfig.BackImage != String.Empty)
            {
                using (var stream = new FileStream(Config.Config.MainConfig.BackImage, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var bitmap = new Bitmap(stream);
                    Core.MainWindow.Background = new ImageBrush()
                    {
                        Source = bitmap,
                        Stretch = Stretch.UniformToFill,
                        Opacity = 0.5
                    };
                }   
            }
        }
        else if (Config.Config.MainConfig.BackModlue == 0)
        {
            Core.MainWindow.Background = Brushes.Transparent;
        }else if (Config.Config.MainConfig.BackModlue == 2)
        {
            Core.MainWindow.Background = Brush.Parse("#101010");
        }
    }
}