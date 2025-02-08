using Newtonsoft.Json.Linq;

namespace MCLauncher.Versions;

public class Versions
{
    public static async Task<List<VersionInfo>> GetAllVersions()
    {
        var versions = await GetVersions("https://github.moeyy.xyz/https://raw.githubusercontent.com/MCMrARM/mc-w10-versiondb/refs/heads/master/versions.json.min");
        
        // 打印所有版本信息
        foreach (var version in versions)
        {
            Console.WriteLine($"Version: {version.Version}, UUID: {version.UUID}, Revision: {version.Revision}");
        }
        
        return versions;
    }
    private static async Task<List<VersionInfo>> GetVersions(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // 下载 JSON 数据
                string json = await client.GetStringAsync(url);

                // 解析 JSON 数据
                JArray versionArray = JArray.Parse(json);
                List<VersionInfo> versions = new List<VersionInfo>();

                foreach (var versionObj in versionArray)
                {
                    // 确保每个版本对象是一个数组
                    if (versionObj is JArray versionArrayItem)
                    {
                        // 提取版本号、UUID 和修订号
                        string version = versionArrayItem[0].ToString();
                        string uuid = versionArrayItem[1].ToString();
                        string revision = versionArrayItem[2].ToString();

                        // 添加到版本信息列表
                        versions.Add(new VersionInfo
                        {
                            Version = version,
                            UUID = uuid,
                            Revision = revision
                        });
                    }
                }

                return versions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching or parsing versions: {ex.Message}");
                return new List<VersionInfo>();
            }
        }
    }
}

public class VersionInfo
{
    public string Version { get; set; }
    public string UUID { get; set; }
    public string Revision { get; set; }
}