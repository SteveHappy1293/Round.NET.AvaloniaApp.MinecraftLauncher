using System.Text.Json.Serialization;
using RMCL.Base.Enum;

namespace RMCL.Base.Entry.Assets.Center;

public class AssetsIndexItemEntry
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    public AssetsTypeEnum Type { get; set; }
}