using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using RMCL.Base.Entry.Style;

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

        if (styleConfig.Model == 2)
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