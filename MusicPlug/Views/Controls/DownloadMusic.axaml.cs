using System.Net;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json.Linq;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;

namespace MusicPlug.Views.Controls;

public partial class DownloadMusic : UserControl
{
    public Song MusicInfo { get; set; }
    public DownloadMusic()
    {
        InitializeComponent();
    }

    public async void Download()
    {
        try
        {
            var mp3url = $"https://music.163.com/song/media/outer/url?id={MusicInfo.Id}";
            var lyricsurl = $"https://music.163.com/api/song/media?id={MusicInfo.Id}";  

            // 创建保存目录
            var saveDir = Path.Combine("../RMCL/RMCL.MusicPlug/Music", MusicInfo.Id.ToString());
            var json = JsonSerializer.Serialize(MusicInfo);
            Directory.CreateDirectory(saveDir);

            string audioPath = Path.Combine(saveDir, $"{MusicInfo.Id}.mp3");
            string lyricsPath = Path.Combine(saveDir, $"{MusicInfo.Id}.lrc");
            string jsonPath = Path.Combine(saveDir, $"index.json");
            File.WriteAllText(jsonPath, json);

            // 重置进度条
            JDBar.Value = 0;

            // 下载音频文件
            JDTitleBox.Text = "正在下载音频...";
            await DownloadFileWithProgressAsync(mp3url, audioPath, (progress) =>
            {
                JDBar.Value = progress;
                JDTitleBox.Text = $"正在下载音频... {progress}%";
            });

            // 下载歌词
            JDTitleBox.Text = "正在下载歌词...";
            JDBar.Value = 0;
            await DownloadLyricsWithProgressAsync(lyricsurl, lyricsPath, (progress) =>
            {
                JDBar.Value = progress;
                JDTitleBox.Text = $"正在下载歌词... {progress}%";
            });

            JDTitleBox.Text = "下载完成！";
            JDBar.Value = 100;
            
            var par = ((ContentDialog)this.Parent);
            par.Hide();
            Message.Show("音乐",$"音乐 {MusicInfo.Name} 下载完毕！",InfoBarSeverity.Success);
        }
        catch (Exception ex)
        {
            JDTitleBox.Text = $"下载失败: {ex.Message}";

            var par = ((ContentDialog)this.Parent);
            par.Hide();
        }
    }

    private async Task DownloadFileWithProgressAsync(string url, string savePath, Action<int> progressCallback)
    {
        const int maxRetries = 3;
        int retryCount = 0;
        bool success = false;

        while (retryCount < maxRetries && !success)
        {
            try
            {
                // 使用HttpClientHandler并允许自动重定向
                var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = true, // 允许自动跟随重定向
                    MaxAutomaticRedirections = 5 // 最大重定向次数
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // 添加必要的请求头
                    client.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Referer", "https://music.163.com/");
                    client.DefaultRequestHeaders.Add("Accept", "audio/webm,audio/ogg,audio/wav,audio/*;q=0.9");

                    // 先发送HEAD请求获取最终URL（可选）
                    var headRequest = new HttpRequestMessage(HttpMethod.Head, url);
                    using (var headResponse = await client.SendAsync(headRequest))
                    {
                        // 检查是否是重定向
                        if (headResponse.StatusCode == HttpStatusCode.Redirect ||
                            headResponse.StatusCode == HttpStatusCode.MovedPermanently)
                        {
                            // 获取重定向后的实际URL
                            var finalUrl = headResponse.Headers.Location?.AbsoluteUri ?? url;
                            url = finalUrl;
                        }
                    }

                    // 正式下载
                    using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new HttpRequestException($"服务器返回错误: {response.StatusCode}");
                        }

                        // 检查Content-Type确保是音频文件
                        var contentType = response.Content.Headers.ContentType?.MediaType;
                        if (contentType == null || !contentType.StartsWith("audio/"))
                        {
                            throw new Exception("返回的不是音频文件，可能是版权限制");
                        }

                        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                        var receivedBytes = 0L;
                        var buffer = new byte[8192];

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            int bytesRead;
                            do
                            {
                                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                if (bytesRead > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    receivedBytes += bytesRead;

                                    if (totalBytes > 0)
                                    {
                                        var progress = (int)((double)receivedBytes / totalBytes * 100);
                                        progressCallback?.Invoke(progress);
                                    }
                                }
                            } while (bytesRead > 0);
                        }
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    if (File.Exists(savePath))
                    {
                        try
                        {
                            File.Delete(savePath);
                        }
                        catch
                        {
                        }
                    }

                    throw new Exception($"下载失败，重试{maxRetries}次后仍然出错: {ex.Message}");
                }

                await Task.Delay(1000 * retryCount);
                progressCallback?.Invoke(0);
            }
        }
    }

    private async Task DownloadLyricsWithProgressAsync(string url, string savePath, Action<int> progressCallback)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");

            // 模拟进度更新，因为歌词文件通常很小
            for (int i = 0; i <= 100; i += 10)
            {
                progressCallback?.Invoke(i);
                await Task.Delay(50);
            }

            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);
            var lyrics = json["lyric"]?.ToString();

            if (!string.IsNullOrEmpty(lyrics))
            {
                await File.WriteAllTextAsync(savePath, lyrics);
                progressCallback?.Invoke(100);
            }
        }
    }
}