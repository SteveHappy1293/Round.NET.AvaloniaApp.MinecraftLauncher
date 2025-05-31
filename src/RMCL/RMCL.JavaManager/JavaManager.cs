using System.Text.Json;
using RMCL.Base.Entry.Java;
using RMCL.PathsDictionary;

namespace RMCL.JavaManager;

public class JavaManager
{
    private const string JsonConfigFileName = PathDictionary.JavaConfigPath;
    public static JavaRootEntry JavaRoot = new();
    
    public static void LoadConfig()
    {
        if (!File.Exists(Path.GetFullPath(JsonConfigFileName)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(JsonConfigFileName));
            SaveConfig();
            return;
        }
        
        var json = File.ReadAllText(Path.GetFullPath(JsonConfigFileName));
        if (string.IsNullOrEmpty(json))
        {
            SaveConfig();
        }
        else
        {
            try
            {
                JavaRoot = JsonSerializer.Deserialize<JavaRootEntry>(json);
            }
            catch
            {
                SaveConfig();
            }
        }
    }

    public static void SaveConfig()
    {
        string jsresult = JsonSerializer.Serialize(JavaRoot, new JsonSerializerOptions() { WriteIndented = true }); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(JsonConfigFileName), jsresult);
    }
}