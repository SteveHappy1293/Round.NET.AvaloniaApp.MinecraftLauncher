using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Utilities;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

public class FindJava
{
    public static bool IsFinish = false;
    public static List<JavaEntry> JavasList { get; set; } = new();
    public static void Find()
    {
        var JavaList = JavaUtil.EnumerableJavaAsync();
        foreach(var javalist in JavaList.ToListAsync().Result)
        {
            JavasList.Add(javalist);
        }
        IsFinish = true;
    }
    public const string JavaFileName = "../RMCL/RMCL.Config/Java.json";
    public static void LoadJava()
    {
        if (!File.Exists(JavaFileName))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(JavaFileName));
            Find();
            SaveJava();
        }
        
        var javajson = File.ReadAllText(Path.GetFullPath(JavaFileName));
        if (string.IsNullOrEmpty(javajson))
        {
            SaveJava();
        }
        else
        {
            try
            {
                JavasList = JsonSerializer.Deserialize<List<JavaEntry>>(javajson);
            }
            catch
            {
                SaveJava();
            }
        }
    }

    public static void SaveJava()
    {
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(JavasList, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(JavaFileName), jsresult);
    }
}