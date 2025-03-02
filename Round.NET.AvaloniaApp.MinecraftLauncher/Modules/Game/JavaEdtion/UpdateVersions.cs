using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion;

public class VersionManifest
{
    [JsonPropertyName("latest")]
    public Latest Latest { get; set; }

    [JsonPropertyName("versions")]
    public List<Version> Versions { get; set; }
}

public class Latest
{
    [JsonPropertyName("release")]
    public string Release { get; set; }

    [JsonPropertyName("snapshot")]
    public string Snapshot { get; set; }
}

public class Version
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("releaseTime")]
    public DateTime ReleaseTime { get; set; }
}

public class UpdateVersions
{
    public static VersionManifest GetVersions()
    {
        string url = "https://piston-meta.mojang.com/mc/game/version_manifest.json";
        VersionManifest manifest = GetVersionManifestAsync(url);

        return manifest;
    }
    
    private static VersionManifest GetVersionManifestAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            // 设置请求头
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<VersionManifest>(content);
                }
                else
                {
                    throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {   
                throw new Exception($"Failed to retrieve data. Reason: {e.Message}");
            }

            return null;
        }
    }
}