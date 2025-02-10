using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

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
        var path = Path.GetFullPath("../RMCL.Style");
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
        var path = Path.GetFullPath("../RMCL.Style/Extract");
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
                };
            }   
        }
    }
    static void CreateZipFile(string sourceDirectory, string zipFilePath)
    {
        // 确保源文件夹存在
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine("源文件夹不存在！");
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

        Console.WriteLine($"ZIP文件已创建：{zipFilePath}");
    }
    static void ExtractZipFile(string zipFilePath, string extractDirectory)
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