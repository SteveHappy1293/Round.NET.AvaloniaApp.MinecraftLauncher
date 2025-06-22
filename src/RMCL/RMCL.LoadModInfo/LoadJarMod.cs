using System.IO.Compression;
using RMCL.Base.Entry.Game.Client;
using RMCL.PathsDictionary;

namespace RMCL.LoadModInfo;

public class LoadJarMod
{
    public static ModInfo LoadJarModInfo(string filename)
    {
        Task.Run(() =>
        {
            // 指定Minecraft模组文件路径
            string modFilePath = filename;
            // 指定要提取的图标文件名
            string iconName = "icon.png";
            // 指定提取目标路径
            string extractIconPath =
                Path.GetFullPath(Path.Combine(PathDictionary.ClientModCacheFolder,
                    Path.GetFileName(filename) + "logo.png"));
            if(File.Exists(extractIconPath)) return;

            try
            {
                // 打开模组文件
                using (ZipArchive archive = ZipFile.OpenRead(modFilePath))
                {
                    // 遍历模组文件中的条目
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // 检查文件名是否匹配
                        if (entry.FullName.EndsWith(iconName, StringComparison.OrdinalIgnoreCase))
                        {
                            // 提取文件到指定路径
                            entry.ExtractToFile(extractIconPath, true);
                            Console.WriteLine($"图标文件 {iconName} 已成功提取到 {extractIconPath}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 捕获并处理异常
                Console.WriteLine($"操作失败：{ex.Message}");
            }

        });
        return new ModInfo();
    }
}