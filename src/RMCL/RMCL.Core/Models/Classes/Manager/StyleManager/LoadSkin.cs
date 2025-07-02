using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;
using RMCL.Base.Enum.BackCall;

namespace RMCL.Core.Models.Classes.Manager.StyleManager;

public class LoadSkin
{
    public static void ImportStyleConfigFile(string FileName)
    {
        var path = Path.GetFullPath(PathsDictionary.PathDictionary.SkinFolderExtract);
        if(Directory.Exists(path)) Directory.Delete(path,true);
        
        Directory.CreateDirectory(path);
        ExtractZipFile(FileName, path);
        
        var json = File.ReadAllText($"{path}/Style.json");
        var styleConfig = JsonSerializer.Deserialize<SkinStyleConfig>(json);

        if (styleConfig.PackVersion == -1)
        {
            LoadOldStyleConfigFile(path, styleConfig);
        }
        else
        {
            LoadNeoStyleConfigFile(path, styleConfig);
        }
    }

    public static void LoadNeoStyleConfigFile(string path, SkinStyleConfig styleConfig)
    {
        if (styleConfig.IsBackground)
        {
            Core.MainWindow.Background = styleConfig.ThemeColors.Theme == ThemeType.Dark
                ? Brush.Parse("#161616")
                : Brushes.AliceBlue;
            Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
            Core.MainWindow.InvalidateVisual();
            Core.MainWindow.BackOpacity.Opacity = 0;

            switch (styleConfig.BackgroundModel.ChooseModel)
            {
                case BackgroundModelEnum.None:
                    break;
                case BackgroundModelEnum.Mica:
                    Core.MainWindow.Background = Brushes.Transparent;
                    Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
                    break;
                case BackgroundModelEnum.AcrylicBlur:
                    Core.MainWindow.Background = Brushes.Transparent;
                    Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
                    break;
                case BackgroundModelEnum.Glass:
                    //Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Blur };
                    Core.MainWindow.Background = new SolidColorBrush()
                    {
                        Color = Color.Parse(styleConfig.BackgroundModel.ColorGlassEntry.HtmlColor)
                    };
                    break;
                case BackgroundModelEnum.Image:
                    if (Config.Config.MainConfig.Background.ImageEntry.ChooseIndex == -1) return;
                    Core.MainWindow.Background = new ImageBrush()
                    {
                        Source = new Bitmap(Path.Combine(path,styleConfig.Background)),
                        Stretch = Stretch.UniformToFill
                    };
                    Core.MainWindow.BackOpacity.Opacity =
                        (double)(100 - styleConfig.Opacity) / 100;
                    break;
            }

            BackCallManager.BackCallManager.Call(BackCallType.UpdateBackground);
        }

        if (styleConfig.IsButton)
        {
            Config.Config.MainConfig.ButtonStyle = styleConfig.ButtonStyle;
        }

        if (styleConfig.IsColor)
        {
            if (styleConfig.ThemeColors.ColorType == ColorType.System)
            {
                Core.FluentAvaloniaTheme.PreferUserAccentColor = true;
            }
            else
            {
                Core.FluentAvaloniaTheme.PreferUserAccentColor = false;
                Core.FluentAvaloniaTheme.CustomAccentColor = Color.Parse(styleConfig.ThemeColors.ThemeColors);
            }


            Config.Config.MainConfig.ThemeColors.Theme = styleConfig.ThemeColors.Theme;
        }
        Config.Config.SaveConfig();
    }

    public static void LoadOldStyleConfigFile(string path,SkinStyleConfig styleConfig)
    {
        var imagepath = $"{path}/{styleConfig.Background}";   
        using (var stream = new FileStream(imagepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            Core.MainWindow.Background = Brushes.Transparent;
            Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
            Core.MainWindow.InvalidateVisual();
            Core.MainWindow.BackOpacity.Opacity = 0;
                
            var bitmap = new Bitmap(stream);
            Core.MainWindow.Background = new ImageBrush()
            {
                Source = bitmap,
                Stretch = Stretch.UniformToFill,
                Opacity = 1
            };
            Core.MainWindow.BackOpacity.Opacity = (1 - styleConfig.Opacity);
        }   
    }
    
    private static void ExtractZipFile(string zipFilePath, string extractDirectory)
    {
        // 确保ZIP文件存在
        if (!File.Exists(zipFilePath))
        {
            Console.WriteLine("ZIP文件不存在！");
            return;
        }

        // 确保目标解压目录存在
        if (!Directory.Exists(extractDirectory))
        {
            Directory.CreateDirectory(extractDirectory);
        }

        // 解压ZIP文件
        ZipFile.ExtractToDirectory(zipFilePath, extractDirectory);

        Console.WriteLine($"ZIP文件已解压到：{extractDirectory}");
    }
}