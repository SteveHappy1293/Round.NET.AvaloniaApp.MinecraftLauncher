using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

public class Updater
{
    private readonly HttpClient _httpClient;
    private readonly Action<string,string> _callback;

    public Updater(Action<string,string> callback)
    {
        _httpClient = new HttpClient();

        // 添加默认请求头
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Updater/1.0"); // GitHub API 要求 User-Agent 头

        _callback = callback;
    }

    public async Task GetDownloadUrlAsync(string url)
    {
        try
        {
            // 访问URL并获取结果
            string response = await _httpClient.GetStringAsync(url);

            // 解析JSON数据
            var releases = JsonSerializer.Deserialize<List<Release>>(response);

            if (releases == null || releases.Count == 0)
            {
                throw new Exception("No releases found.");
            }

            // 获取第一个Release（假设只有一个Release）
            var release = releases[0];

            // 获取与当前系统一致的下载地址
            string downloadUrl = GetDownloadUrlForCurrentSystem(release.assets);

            if (string.IsNullOrEmpty(downloadUrl))
            {
                throw new Exception("No matching download URL found for the current system.");
            }

            // 调用回调函数
            _callback?.Invoke(release.name,downloadUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private string GetDownloadUrlForCurrentSystem(List<Asset> assets)
    {
        string systemType = GetCurrentSystemType();
        string systemArchitecture = GetCurrentSystemArchitecture();

        foreach (var asset in assets)
        {
            string assetName = asset.name.ToLower();

            // 检查系统类型和架构是否匹配
            if (assetName.Contains(systemType) && assetName.Contains(systemArchitecture))
            {
                return asset.browser_download_url;
            }
        }

        return null;
    }
    private string GetCurrentSystemArchitecture()
    {
        var architecture = RuntimeInformation.ProcessArchitecture;

        switch (architecture)
        {
            case Architecture.X64:
                return "x64";
            case Architecture.Arm64:
                return "arm64";
            case Architecture.X86:
                return "x86";
            case Architecture.Arm:
                return "arm";
            default:
                throw new PlatformNotSupportedException($"Unsupported architecture: {architecture}");
        }
    }
    private string GetCurrentSystemType()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "win";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "linux";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "osx";
        }

        throw new PlatformNotSupportedException("Unsupported operating system.");
    }
}