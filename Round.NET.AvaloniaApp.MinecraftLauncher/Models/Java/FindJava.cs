using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Components.Fetcher;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Models.Java;

public class FindJava
{
    public static bool IsFinish = false;
    public static List<JavaEntry> JavasList { get; set; } = new();
    public static void Find()
    {
        //实例化
        JavaFetcher javaFetcher = new JavaFetcher();
        var JavaList = javaFetcher.Fetch();
        Debug.WriteLine("您的设备总共有" + JavaList.Length + "个Java，它们是：");
        foreach(var javalist in JavaList)
        {
            JavasList.Add(new()
            {
                JavaPath = javalist.JavaPath,
                JavaVersion = javalist.JavaVersion,
                JavaSlugVersion =  javalist.JavaSlugVersion,
                JavaDirectoryPath = javalist.JavaDirectoryPath
            });
            // Console.WriteLine("Java路径：" + javalist.JavaPath + "，Java版本：" + javalist.JavaVersion + "，是否为64位：" + javalist.Is64Bit);
        }
        IsFinish = true;
    }
}