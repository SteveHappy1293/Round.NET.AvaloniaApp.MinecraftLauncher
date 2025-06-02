using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FluentAvalonia.UI.Controls;

namespace RMCL.Models.Classes;

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
}