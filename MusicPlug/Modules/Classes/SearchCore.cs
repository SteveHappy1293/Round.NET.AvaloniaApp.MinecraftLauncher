using Newtonsoft.Json;

namespace MusicPlug.Modules;

public class SearchCore
{
    public static SongResponse Search(string text)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                // 设置 User-Agent 避免被拒绝
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                var response = httpClient.GetAsync(
                    $"https://music.163.com/api/search/get?s={Uri.EscapeDataString(text)}&type=1&offset=0&limit=100").Result;

                response.EnsureSuccessStatusCode(); // 确保请求成功

                var jsonString = response.Content.ReadAsStringAsync().Result;
                // Console.WriteLine(jsonString);

                var obj = JsonConvert.DeserializeObject<SongResponse>(jsonString);
                return obj;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求失败: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                return null;
            }
        }
    }
}