using System;
using System.Security.Cryptography;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

public class RLogs
{
    public static void WriteLog(dynamic log)
    {
        var lo = $"[{DateTime.Now:HH:mm:ss}] {log}";
        
        Console.WriteLine(lo);
    }
}