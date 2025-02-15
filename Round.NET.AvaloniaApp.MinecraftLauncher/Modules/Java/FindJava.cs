using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Components.Fetcher;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

public class FindJava
{
    public static bool IsFinish = false;
    public static List<JavaEntry> JavasList { get; set; } = new();
    public static void Find()
    {
        //实例化
        JavaFetcher javaFetcher = new JavaFetcher();
        var JavaList = javaFetcher.Fetch();
        RLogs.WriteLog("您的设备总共有" + JavaList.Length + "个Java，它们是：");
        foreach(var javalist in JavaList)
        {
            JavasList.Add(new()
            {
                JavaPath = javalist.JavaPath,
                JavaVersion = javalist.JavaVersion,
                JavaSlugVersion =  javalist.JavaSlugVersion,
                JavaDirectoryPath = javalist.JavaDirectoryPath
            });
            RLogs.WriteLog("Java路径：" + javalist.JavaPath + "，Java版本：" + javalist.JavaVersion + "，是否为64位：" + javalist.Is64Bit);
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