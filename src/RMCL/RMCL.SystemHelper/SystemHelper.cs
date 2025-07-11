using System.Diagnostics;
using System.IO.Compression;

namespace RMCL.SystemHelper;

public class SystemHelper
{
    public static class FileExplorer
    {
        public static void OpenFolder(string folderPath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = $"\"{folderPath}\"",
                UseShellExecute = true // 允许操作系统处理文件夹路径
            };

            try
            {
                Process.Start(startInfo);
            }catch{ }
        }
    }

    public static void OpenFile(string imagePath)
    {
        try
        {
            // 在Windows、Linux和macOS上，Process.Start通常能正确处理默认程序
            Process.Start(new ProcessStartInfo
            {
                FileName = imagePath,
                UseShellExecute = true  // 必须设置为true才能使用默认程序打开
            });
        }
        catch (Exception ex)
        {
            // 处理异常，例如没有默认程序关联该文件类型
            Console.WriteLine($"Error opening image: {ex.Message}");
        }
    }
    public static void OpenUrl(string url)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", url);
            }
            else if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", url);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法打开浏览器: {ex.Message}");
        }
    }

    public static string GetFolderName(string path)
    {
        string folderPath = path;
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        return dirInfo.Name;
    }
    
    public static string GetAppConfigDirectory()
    {
        // 选择适合的 SpecialFolder
        var folder = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData,
            Environment.SpecialFolderOption.Create); // 确保目录存在
        
        // 组合应用专属路径
        return Path.Combine(folder, "RoundStudio");
    }
    
    public static void CreateZipFromFolder(string sourceFolderPath, string zipFilePath)
    {
        // 确保源文件夹存在
        if (!Directory.Exists(sourceFolderPath))
        {
            throw new DirectoryNotFoundException($"源文件夹不存在: {sourceFolderPath}");
        }

        // 创建ZIP文件
        using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
        {
            // 获取文件夹中所有文件
            string[] files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);
        
            foreach (string file in files)
            {
                // 获取相对路径
                string relativePath = file.Substring(sourceFolderPath.Length).TrimStart(Path.DirectorySeparatorChar);
            
                // 在ZIP中创建条目
                ZipArchiveEntry entry = archive.CreateEntry(relativePath, CompressionLevel.Optimal);
            
                // 写入文件内容
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                using (Stream entryStream = entry.Open())
                {
                    fileStream.CopyTo(entryStream);
                }
            }
        }
    
        Console.WriteLine($"已成功创建ZIP文件: {zipFilePath}");
    }
}