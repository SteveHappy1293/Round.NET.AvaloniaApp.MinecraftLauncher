using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Downloader;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using SkiaSharp;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class StyleMange
{
    public static void Load(Window window)
    {
        if (Config.Config.MainConfig.BackModlue == 2)
        {
            if (Config.Config.MainConfig.BackImage != String.Empty)
            {
                using (var stream = new FileStream(Config.Config.MainConfig.BackImage, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    window.Background = Brush.Parse("#101010");
                    window.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
                    var bitmap = new Bitmap(stream);
                    window.Background = new ImageBrush()
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
            window.Background = Brushes.Transparent;
            window.TransparencyLevelHint = new[] { WindowTransparencyLevel.Mica };
        }else if (Config.Config.MainConfig.BackModlue == 1)
        {
            window.Background = Brushes.Transparent;
            window.TransparencyLevelHint = new[] { WindowTransparencyLevel.AcrylicBlur };
        }else if (Config.Config.MainConfig.BackModlue == 3) // 网络随机图片
        {
            LoadRandomBackground(window);
        }
        else if (Config.Config.MainConfig.BackModlue == 4)
        {
            ImportStyleConfigFile(Config.Config.MainConfig.StyleFile,window);
        }
        else if(Config.Config.MainConfig.BackModlue == 5)
        {
            window.Background = Brush.Parse("#101010");
        }
    }

    public static void LoadRandomBackground(Window window)
    {
        window.Background = Brush.Parse("#101010");
        if (!Directory.Exists(Path.Combine("../RMCL/RMCL.Style/Wallpaper")))
        {
            Directory.CreateDirectory(Path.Combine("../RMCL/RMCL.Style/Wallpaper"));
        }

        var files = Directory.GetFiles(Path.Combine("../RMCL/RMCL.Style/Wallpaper"));
        var num = files.Count();
        if (num == 0)
        {
            Task.Run(() =>
            {
                string apiUrl = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8&mkt=zh-CN";

                // 1. 获取 Bing 图片数据
                using (HttpClient client = new HttpClient())
                {
                    string jsonResponse = client.GetStringAsync(apiUrl).Result;

                    // 2. 解析 JSON 数据
                    JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    var images = doc.RootElement.GetProperty("images").EnumerateArray();

                    // 3. 随机选择一个图片
                    var random = new Random();
                    int index = random.Next(0, 8); // 随机选择 0 到 7 的索引
                    var selectedImage = images.ElementAt(index);

                    string imageUrl = "https://cn.bing.com" + selectedImage.GetProperty("url").GetString();
                    string imageTitle = selectedImage.GetProperty("title").GetString();
                    string imageDate = selectedImage.GetProperty("enddate").GetString();
                    imageUrl = imageUrl.Replace("1920x1080", "UHD");

                    Console.WriteLine($"Selected Image: {imageTitle}");
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Image Date: {imageDate}");

                    // 4. 使用 Downloader 库下载图片
                    var downloader = new DownloadService();
                    string destinationFilePath = $"../RMCL/RMCL.Style/Wallpaper/{imageDate}.jpg"; // 文件名使用日期
                    downloader.DownloadFileCompleted += (s, e) =>
                        Message.Message.Show("个性化", "已加载网络壁纸！", InfoBarSeverity.Success);
                    downloader.DownloadFileTaskAsync(imageUrl, destinationFilePath);

                    Console.WriteLine($"Image downloaded to: {Path.GetFullPath(destinationFilePath)}");
                }
            });
        }
        else
        {
            Random rnd = new Random();
            var index = rnd.Next(0, num);

            try
            {
                using (var stream = new FileStream(files[index], FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    window.Background = Brush.Parse("#101010");
                    window.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
                    var bitmap = new Bitmap(stream);
                    window.Background = new ImageBrush()
                    {
                        Source = bitmap,
                        Stretch = Stretch.UniformToFill,
                        Opacity = Config.Config.MainConfig.BackOpacity,
                    };
                }
            }
            catch
            {
                Message.Message.Show("个性化", "加载壁纸出错", InfoBarSeverity.Error);
            }

            Task.Run(() =>
            {
                string apiUrl = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8&mkt=zh-CN";

                // 1. 获取 Bing 图片数据
                using (HttpClient client = new HttpClient())
                {
                    string jsonResponse = client.GetStringAsync(apiUrl).Result;

                    // 2. 解析 JSON 数据
                    JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    var images = doc.RootElement.GetProperty("images").EnumerateArray();

                    // 3. 随机选择一个图片
                    var random = new Random();
                    int index = random.Next(0, 8); // 随机选择 0 到 7 的索引
                    var selectedImage = images.ElementAt(index);

                    string imageUrl = "https://cn.bing.com" + selectedImage.GetProperty("url").GetString();
                    string imageTitle = selectedImage.GetProperty("title").GetString();
                    string imageDate = selectedImage.GetProperty("enddate").GetString();
                    imageUrl = imageUrl.Replace("1920x1080", "UHD");

                    Console.WriteLine($"Selected Image: {imageTitle}");
                    Console.WriteLine($"Image URL: {imageUrl}");
                    Console.WriteLine($"Image Date: {imageDate}");

                    // 4. 使用 Downloader 库下载图片
                    var downloader = new DownloadService();
                    string destinationFilePath = $"../RMCL/RMCL.Style/Wallpaper/{imageDate}.jpg"; // 文件名使用日期
                    downloader.DownloadFileCompleted += (s, e) =>
                        Message.Message.Show("个性化", "已加载网络壁纸！", InfoBarSeverity.Success);
                    downloader.DownloadFileTaskAsync(imageUrl, destinationFilePath);

                    Console.WriteLine($"Image downloaded to: {Path.GetFullPath(destinationFilePath)}");
                }
            });
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
    public static void ImportStyleConfigFile(string FileName,Window window)
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
                window.Background = Brush.Parse("#101010");
                window.TransparencyLevelHint = new[] { WindowTransparencyLevel.None };
                var bitmap = new Bitmap(stream);
                window.Background = new ImageBrush()
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
}