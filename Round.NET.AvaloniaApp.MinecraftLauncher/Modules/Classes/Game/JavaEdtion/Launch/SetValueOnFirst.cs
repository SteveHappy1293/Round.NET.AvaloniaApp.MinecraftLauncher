using System.IO;
using DynamicData;
using FluentAvalonia.Core;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;

public class SetValueOnFirst
{
    public static void SetLanguage(string Version)
    {
        var path = $"{Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path}/versions/{Version}/options.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "lang:zh_cn");
        }
        else
        {
            var lines = File.ReadAllLines(path);
            if(lines.Contains("lang:")) File.WriteAllText(path, "lang:zh_cn");
            
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("lang:"))
                {
                    lines[i]="lang:zh_cn";
                }
            }
            File.WriteAllLines(path, lines);
        }
    }

    public static void SetGamma(string Version)
    {
        //gamma:0.5
        var path = $"{Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path}/versions/{Version}/options.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "gamma:1000");
        }
        else
        {
            var lines = File.ReadAllLines(path);
            if(lines.Contains("gamma:")) File.WriteAllText(path, "gamma:1000");
            
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("gamma:"))
                {
                    lines[i]="gamma:100.0";
                }
            }
            File.WriteAllLines(path, lines);
        }
    }
}