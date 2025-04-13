using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.NetWork.Organization;

public class OrganizationCore
{
    public static OrganizationEnrty.ConfigResponse Organization { get; set; } = new ();
    public static void LoadOrganizationConfig()
    {
        if(string.IsNullOrEmpty(Config.Config.MainConfig.OrganizationUrl)) return;
        if(string.IsNullOrWhiteSpace(Config.Config.MainConfig.OrganizationUrl)) return;
        
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Config.Config.MainConfig.OrganizationUrl),
            Content = new StringContent("{\"Type\":\"Config\"}", Encoding.UTF8, "application/json")
        };

        try
        {
            var response = client.SendAsync(request).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(responseBody);
            Organization = JsonSerializer.Deserialize<OrganizationEnrty.ConfigResponse>(responseBody);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Config.Config.MainConfig.JavaUseMemory = OrganizationCore.Organization.Data.Config.MaxMemory;
        Config.Config.MainConfig.DownloadThreads = Organization.Data.Config.MaxDownloadThreadCount;
        Config.Config.SaveConfig();
    }
}