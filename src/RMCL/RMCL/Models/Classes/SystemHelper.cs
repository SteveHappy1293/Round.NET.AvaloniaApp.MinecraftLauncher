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
}