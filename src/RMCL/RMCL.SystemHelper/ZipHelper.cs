using System.IO.Compression;

namespace RMCL.SystemHelper;

public class ZipHelper
{
    public static void ZipFolders(string[] paths, string outputZipPath)
    {
        // 确保输出目录存在
        var outputDirectory = Path.GetDirectoryName(outputZipPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // 如果ZIP文件已存在，删除它
        if (File.Exists(outputZipPath))
        {
            File.Delete(outputZipPath);
        }

        // 创建新的ZIP文件
        using (var zipArchive = ZipFile.Open(outputZipPath, ZipArchiveMode.Create))
        {
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    // 如果是文件，直接添加到ZIP根目录
                    var fileName = Path.GetFileName(path);
                    zipArchive.CreateEntryFromFile(path, fileName);
                }
                else if (Directory.Exists(path))
                {
                    // 如果是文件夹，按原有逻辑处理
                    var folderName = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    
                    foreach (var file in files)
                    {
                        var relativePath = Path.Combine(folderName, file.Substring(path.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                        zipArchive.CreateEntryFromFile(file, relativePath);
                    }
                }
                else
                {
                    Console.WriteLine($"警告: 路径 '{path}' 既不是文件也不是文件夹，跳过。");
                }
            }
        }
    }
}