using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Issues;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.NetWork.Issues;

public class IssuesCore
{
    public static List<IssuesItemEnrty> Issues { private set; get; } = new();

    public static bool Load()
    {
        string apiUrl = "https://api.github.com/repos/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/issues?per_page=100";
        
        // 使用 HttpClient 发送请求
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36 Edg/135.0.0.0");

            try
            {
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                
                // 反序列化 JSON 到 Issue 对象列表
                Issues = JsonConvert.DeserializeObject<List<IssuesItemEnrty>>(responseBody);
                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return false;
            }
        }
    }
}