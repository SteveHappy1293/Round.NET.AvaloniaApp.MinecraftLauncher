using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.User;

public class UserInfoSerch
{
    private readonly HttpClient _httpClient;

    public UserInfoSerch()
    {
        _httpClient = new HttpClient();
    }

    // 获取玩家的UUID
    public async Task<string> GetUuidByUsername(string username)
    {
        var response = await _httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("UUID获取失败");
        }

        var content = await response.Content.ReadAsStringAsync();
        using (JsonDocument jsonDocument = JsonDocument.Parse(content))
        {
            return jsonDocument.RootElement.GetProperty("id").GetString();
        }
    }

    // 获取玩家的皮肤URL
    public async Task<string> GetSkinUrlByUuid(string uuid)
    {
        var response = await _httpClient.GetAsync($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("档案获取失败");
        }

        var content = await response.Content.ReadAsStringAsync();
        using (JsonDocument jsonDocument = JsonDocument.Parse(content))
        {
            var propertiesArray = jsonDocument.RootElement.GetProperty("properties").EnumerateArray();

            foreach (var property in propertiesArray)
            {
                if (property.GetProperty("name").GetString() == "textures")
                {
                    var texturesBase64 = property.GetProperty("value").GetString();
                    var decodedTexturesJson = JsonSerializer.Deserialize<JsonDocument>(Convert.FromBase64String(texturesBase64));
                    return decodedTexturesJson.RootElement.GetProperty("SKIN").GetProperty("url").GetString();
                }
            }

            throw new Exception("皮肤URL获取失败");
        }
    }
}