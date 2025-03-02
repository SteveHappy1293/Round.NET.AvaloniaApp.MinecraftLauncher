using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using FluentAvalonia.Styling;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using SkiaSharp;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class StyleMange
{
    public static void Load()
    {
        if (Config.Config.MainConfig.BackModlue == 2)
        {
            if (Config.Config.MainConfig.BackImage != String.Empty)
            {
                using (var stream = new FileStream(Config.Config.MainConfig.BackImage, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Core.MainWindow.Background = Brush.Parse("#101010");
                    Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
                    var bitmap = new Bitmap(stream);
                    Core.MainWindow.Background = new ImageBrush()
                    {
                        Source = bitmap,
                        Stretch = Stretch.UniformToFill,
                        Opacity = Config.Config.MainConfig.BackOpacity,
                    };
                }   
                /*var col = GetDominantColorAfterMonetFilter(Config.Config.MainConfig.BackImage);
// 动态创建 FluentAvaloniaTheme
                var fluentTheme = new FluentAvaloniaTheme
                {
                    CustomAccentColor = Color.FromRgb(col.Red, col.Green, col.Blue) // 设置自定义主题色
                };

// 将 FluentAvaloniaTheme 添加到应用程序的全局样式
                if (Application.Current != null)
                {
                    // 移除旧的 FluentAvaloniaTheme（如果存在）
                    var existingTheme = Application.Current.Styles.OfType<FluentAvaloniaTheme>().FirstOrDefault();
                    if (existingTheme != null)
                    {
                        Application.Current.Styles.Remove(existingTheme);
                    }

                    // 添加新的 FluentAvaloniaTheme
                    Application.Current.Styles.Add(fluentTheme);
                }*/
            }
        }
        else if (Config.Config.MainConfig.BackModlue == 0)
        {
            Core.MainWindow.Background = Brushes.Transparent;
            Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
        }else if (Config.Config.MainConfig.BackModlue == 1)
        {
            Core.MainWindow.Background = Brushes.Transparent;
            Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
        }else if (Config.Config.MainConfig.BackModlue == 3)
        {
            ImportStyleConfigFile(Config.Config.MainConfig.StyleFile);
        }
        else if(Config.Config.MainConfig.BackModlue == 4)
        {
            Core.MainWindow.Background = Brush.Parse("#101010");
        }
    }
    private class StyleConfig
    {
        public int Model { get; set; } = 0;
        public double Opacity { get; set; } = 100;
        public string Background { get; set; } = string.Empty;
    }
    public static void ExportStyleConfigFile(string FileName)
    {
        var path = Path.GetFullPath("../RMCL/RMCL.Style");
        if (!Directory.Exists($"{path}/Temp"))
        {
            Directory.CreateDirectory($"{path}/Temp");
        }
        else
        {
            Directory.Delete($"{path}/Temp",true);
            Directory.CreateDirectory($"{path}/Temp");
        }
        Directory.CreateDirectory(path + "/Temp/Image");
        
        var styleConfig = new StyleConfig();
        styleConfig.Model = Config.Config.MainConfig.BackModlue;
        styleConfig.Opacity = Config.Config.MainConfig.BackOpacity;
        styleConfig.Background = $"/Image/Background.{Path.GetExtension(Config.Config.MainConfig.BackImage).Replace(".","")}";
        
        if (styleConfig.Model == 2)
        {
            File.Copy(Config.Config.MainConfig.BackImage, $"{path}/Temp/Image/Background.{Path.GetExtension(Config.Config.MainConfig.BackImage).Replace(".","")}");
        }
        var json = Regex.Unescape(JsonSerializer.Serialize(styleConfig, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\"));
        File.WriteAllText(path + "/Temp/Style.json", json);
        CreateZipFile(path + "/Temp", FileName);
    }
    public static void ImportStyleConfigFile(string FileName)
    {
        var path = Path.GetFullPath("../RMCL/RMCL.Style/Extract");
        if(Directory.Exists(path)) Directory.Delete(path,true);
        
        Directory.CreateDirectory(path);
        ExtractZipFile(FileName, path);
        
        var json = File.ReadAllText($"{path}/Style.json");
        var styleConfig = JsonSerializer.Deserialize<StyleConfig>(json);

        if (styleConfig.Model == 2)
        {
            var imagepath = $"{path}/{styleConfig.Background}";   
            using (var stream = new FileStream(imagepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Core.MainWindow.Background = Brush.Parse("#101010");
                Core.MainWindow.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
                var bitmap = new Bitmap(stream);
                Core.MainWindow.Background = new ImageBrush()
                {
                    Source = bitmap,
                    Stretch = Stretch.UniformToFill,
                    Opacity = styleConfig.Opacity,
                };// 获取莫奈滤镜后的重点颜色
                
                
                /*var col = GetDominantColorAfterMonetFilter(imagepath);

// 动态创建 FluentAvaloniaTheme
                var fluentTheme = new FluentAvaloniaTheme
                {
                    CustomAccentColor = Color.FromRgb(col.Red, col.Green, col.Blue) // 设置自定义主题色
                };

// 将 FluentAvaloniaTheme 添加到应用程序的全局样式
                if (Application.Current != null)
                {
                    // 移除旧的 FluentAvaloniaTheme（如果存在）
                    var existingTheme = Application.Current.Styles.OfType<FluentAvaloniaTheme>().FirstOrDefault();
                    if (existingTheme != null)
                    {
                        Application.Current.Styles.Remove(existingTheme);
                    }

                    // 添加新的 FluentAvaloniaTheme
                    Application.Current.Styles.Add(fluentTheme);
                }*/
            }   
        }
    }
    static void CreateZipFile(string sourceDirectory, string zipFilePath)
    {
        // 确保源文件夹存在
        if (!Directory.Exists(sourceDirectory))
        {
            RLogs.WriteLog("源文件夹不存在！");
            return;
        }

        // 确保目标文件夹存在
        string targetDirectory = Path.GetDirectoryName(zipFilePath);
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        // 创建ZIP文件
        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }
        ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);

        RLogs.WriteLog($"ZIP文件已创建：{zipFilePath}");
    }
    static void ExtractZipFile(string zipFilePath, string extractDirectory)
    {
        // 确保ZIP文件存在
        if (!File.Exists(zipFilePath))
        {
            RLogs.WriteLog("ZIP文件不存在！");
            return;
        }

        // 确保目标解压目录存在
        if (!Directory.Exists(extractDirectory))
        {
            Directory.CreateDirectory(extractDirectory);
        }

        // 解压ZIP文件
        ZipFile.ExtractToDirectory(zipFilePath, extractDirectory);

        RLogs.WriteLog($"ZIP文件已解压到：{extractDirectory}");
    }

     public static SKColor GetDominantColorAfterMonetFilter(string inputPath)
    {
        // 加载图像
        using (var skImage = SKImage.FromEncodedData(inputPath))
        {
            if (skImage == null)
            {
                Console.WriteLine("无法加载图像。");
                return SKColor.Empty;
            }

            // 将SKImage转换为SKBitmap以便修改像素
            var skBitmap = SKBitmap.FromImage(skImage);
            if (skBitmap == null)
            {
                Console.WriteLine("无法将图像转换为位图。");
                return SKColor.Empty;
            }

            // 用于统计颜色频率的字典
            var colorFrequency = new Dictionary<SKColor, int>();

            // 遍历图像的每个像素
            for (int y = 0; y < skBitmap.Height; y++)
            {
                for (int x = 0; x < skBitmap.Width; x++)
                {
                    // 获取当前像素的颜色
                    var pixel = skBitmap.GetPixel(x, y);

                    // 转换为莫奈风格的色调
                    var monetColor = ApplyMonetFilter(pixel);

                    // 设置新图像中的像素颜色
                    skBitmap.SetPixel(x, y, monetColor);

                    // 统计颜色频率
                    if (colorFrequency.ContainsKey(monetColor))
                    {
                        colorFrequency[monetColor]++;
                    }
                    else
                    {
                        colorFrequency[monetColor] = 1;
                    }
                }
            }

            // 提取出现频率最高的颜色
            var dominantColor = colorFrequency.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

            Console.WriteLine($"重点颜色: R={dominantColor.Red}, G={dominantColor.Green}, B={dominantColor.Blue}");
            skImage.Dispose();
            skBitmap.Dispose();
            return dominantColor;
        }
    }

    private static SKColor ApplyMonetFilter(SKColor originalColor)
    {
        // 莫奈风格的色调通常较为柔和，偏向于蓝绿色调
        // 这里我们简单地调整颜色的RGB值来模拟这种效果

        byte r = originalColor.Red;
        byte g = originalColor.Green;
        byte b = originalColor.Blue;

        // 增加蓝色和绿色的分量，减少红色的分量
        int newR = (int)(r * 0.7);
        int newG = (int)(g * 0.9);
        int newB = (int)(b * 1.1);

        // 确保颜色值在0-255之间
        newR = Math.Min(255, Math.Max(0, newR));
        newG = Math.Min(255, Math.Max(0, newG));
        newB = Math.Min(255, Math.Max(0, newB));

        return new SKColor((byte)newR, (byte)newG, (byte)newB);
    }
}