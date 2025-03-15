using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Round.NET.MinecraftPlayerSkin.Library;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class MinecraftPlayerSkin
{
    private static HttpClient httpClient = new HttpClient();

    public event Action<string> OnProgressUpdate;
    public string Skin  { get; private set; }
    public string Head  { get; private set; }
    public string Body  { get; private set; }
    public string RightHand { get; private set; }
    public string LeftHand { get; private set; }
    public string RightLeg { get; private set; }
    public string LeftLeg { get; private set; }
    private string uuid = string.Empty;
    public string SkinJson  { get; private set; }

    // 通过玩家名称获取UUID
    public async Task<bool> GetPlayerAttribute(string playerName)
    {
        OnProgressUpdate?.Invoke("获取玩家UUID中...");
        string uuidUrl = $"https://api.mojang.com/users/profiles/minecraft/{playerName}";
        string response = await httpClient.GetStringAsync(uuidUrl);

        // 解析JSON获取UUID
        using JsonDocument doc = JsonDocument.Parse(response);
        JsonElement root = doc.RootElement;
        uuid = root.GetProperty("id").GetString();
        OnProgressUpdate?.Invoke("获取玩家UUID完毕");

        await GetPlayerSkinsJsonAsync();
        await GetPlayerSkin();
        return true;
    }

    public async Task GetPlayerSkinsJsonAsync()
    {
        Debug.WriteLine(uuid);
        string url = $"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}";
        string response = await httpClient.GetStringAsync(url);
        
        var jsonDocument = JsonDocument.Parse(response);
        var root = jsonDocument.RootElement;

        // 获取properties数组
        var properties = root.GetProperty("properties");

        foreach (var property in properties.EnumerateArray())
        {
            if (property.GetProperty("name").GetString() == "textures")
            {
                // 获取value字段
                string value = property.GetProperty("value").GetString();
                Console.WriteLine(value);
                var bytes = Convert.FromBase64String(value);
                SkinJson = Encoding.UTF8.GetString(bytes);
            }
        }
    }

    public async Task GetPlayerSkin()
    {
        try
        {
            // 解析 JSON 数据
            JObject jsonObject = JObject.Parse(SkinJson);

            // 提取 SKIN 的 url
            string url = jsonObject["textures"]["SKIN"]["url"].ToString();

            // 输出 SKIN 的 url
            Console.WriteLine("SKIN URL: " + url);

            // 下载皮肤图像
            using (HttpClient client = new HttpClient())
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                // 动态设置请求头
                request.Headers.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                request.Headers.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");

                // 发送请求
                HttpResponseMessage response = await client.SendAsync(request);

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}");
                    Console.WriteLine($"错误信息: {ex.Message}");
                    return;
                }

                // 读取响应内容
                byte[] imageData = await response.Content.ReadAsByteArrayAsync();

                if (imageData == null || imageData.Length == 0)
                {
                    Console.WriteLine("下载的图像数据为空");
                    return;
                }

                Skin = Convert.ToBase64String(imageData);

                // 使用 SkinResolver 裁剪图像并转换为 Base64
                var skinResolver = new SkinResolver(imageData);

                // 裁剪头像并转换为 Base64
                var headImage = skinResolver.CropSkinHeadBitmap();
                Head = ImageToBase64(headImage);

                // 裁剪身体并转换为 Base64
                var bodyImage = skinResolver.CropSkinBodyBitmap();
                Body = ImageToBase64(bodyImage);

                // 裁剪右手并转换为 Base64
                var rightHandImage = skinResolver.CropRightHandBitmap();
                RightHand = ImageToBase64(rightHandImage);

                // 裁剪左手并转换为 Base64
                var leftHandImage = skinResolver.CropLeftHandBitmap();
                LeftHand = ImageToBase64(leftHandImage);

                // 裁剪右腿并转换为 Base64
                var rightLegImage = skinResolver.CropRightLegBitmap();
                RightLeg = ImageToBase64(rightLegImage);

                // 裁剪左腿并转换为 Base64
                var leftLegImage = skinResolver.CropLeftLegBitmap();
                LeftLeg = ImageToBase64(leftLegImage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
        }
    }

    private string ImageToBase64(Image<Rgba32> image)
    {
        using var memoryStream = new System.IO.MemoryStream();
        image.SaveAsPng(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }
}