using System;
using MinecraftLaunch.Base.Models.Game;
using Round.NET.FindJava.Library;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

public class RJavaToJava
{
    public static JavaEntry ToJavaEntry(RJavaEntry java)
    {
        return new JavaEntry()
        {
            JavaPath = java.JavaPath,
            JavaVersion = new Version(java.JavaVersion)
        };
    }
}