using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class ImageDownloader
{
    private readonly HttpClient _httpClient;

    public ImageDownloader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public string FilePath { get; private set; }
    /// <summary>
    /// 从URL下载图片并保存到指定路径
    /// </summary>
    /// <param name="imageUrl">图片URL</param>
    /// <param name="savePath">保存路径（包含文件名和扩展名）</param>
    /// <returns>保存的文件路径</returns>
    public async Task<string> DownloadImageAsync(string imageUrl, string savePath)
    {
        try
        {
            // 验证URL
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("图片URL不能为空");
            }

            // 验证保存路径
            if (string.IsNullOrWhiteSpace(savePath))
            {
                throw new ArgumentException("保存路径不能为空");
            }

            // 创建目录（如果不存在）
            var directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 发送HTTP请求获取图片
            using (var response = await _httpClient.GetAsync(imageUrl))
            {
                response.EnsureSuccessStatusCode();

                // 确定文件扩展名
                var extension = GetImageExtension(response.Content.Headers.ContentType.MediaType);
                var finalPath = string.IsNullOrEmpty(Path.GetExtension(savePath))
                    ? $"{savePath}{extension}"
                    : savePath;
                FilePath = finalPath;

                // 保存文件
                using (var fileStream = File.Create(finalPath))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                return finalPath;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"下载图片失败: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 根据MIME类型获取图片扩展名
    /// </summary>
    private string GetImageExtension(string mimeType)
    {
        return mimeType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/webp" => ".webp",
            "image/svg+xml" => ".svg",
            _ => ".dat" // 未知类型使用默认扩展名
        };
    }
}