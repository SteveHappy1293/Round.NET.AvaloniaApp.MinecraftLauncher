using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using RMCLPlugToolsEntry;

namespace RMCLPlugTools.Classes;

public class Packing
{
    public static void GoPacking(PlugConfigEntry Config)
    {
        Directory.CreateDirectory("RMCL.Packing");
        if (Directory.Exists($"RMCL.Packing\\Plug"))
        {
            Directory.Delete($"RMCL.Packing\\Plug", true);
        }
        Directory.CreateDirectory("RMCL.Packing\\Plug");
        Directory.CreateDirectory("RMCL.Packing\\Plug\\plug");
        Directory.CreateDirectory("RMCL.Packing\\Plug\\lib");
        Directory.CreateDirectory("RMCL.Packing\\Plug\\assets");
        
        File.Copy(Config.PlugIcon.Replace("\"",""), $"RMCL.Packing\\Plug\\assets\\{Path.GetFileName(Config.PlugIcon.Replace("\"",""))}");
        File.Copy(Config.PlugMainDll.Replace("\"",""), $"RMCL.Packing\\Plug\\plug\\{Path.GetFileName(Config.PlugMainDll.Replace("\"",""))}");
        
        FileSystem.CopyDirectory(Config.PlugMainDir.Replace("\"",""), "RMCL.Packing\\Plug\\lib", UIOption.AllDialogs,
            UICancelOption.DoNothing);

        foreach (var lib in Directory.EnumerateFiles("RMCL.Packing\\Plug\\lib"))
        {
            if (Path.GetFileName(lib) == Path.GetFileName(Config.PlugMainDll))
            {
                File.Delete(lib);
            }
        }

        var Entry = new PlugFileEnty()
        {
            PlugWriter = Config.PlugWriter,
            PlugName = Config.PlugName,
            IconPath = $"\\assets\\{Path.GetFileName(Config.PlugIcon.Replace("\"",""))}",
            PlugVersion = Config.PlugVersion
        };
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(Entry, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText("RMCL.Packing\\Plug\\index.json",jsresult);
        if(File.Exists("RMCL.Packing\\Plug.rplk")) File.Delete("RMCL.Packing\\Plug.rplk");
        ZipFile.CreateFromDirectory("RMCL.Packing\\Plug\\", "RMCL.Packing\\Plug.rplk");
    }
}