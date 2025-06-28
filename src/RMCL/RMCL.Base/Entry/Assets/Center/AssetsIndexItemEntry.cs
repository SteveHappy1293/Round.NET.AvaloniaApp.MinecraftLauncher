using System.Text.Json.Serialization;

namespace RMCL.Base.Entry.Assets.Center;

public class AssetsIndexItemEntry
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; }
}