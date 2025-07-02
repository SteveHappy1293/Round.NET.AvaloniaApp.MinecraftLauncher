using System.Diagnostics;

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
}