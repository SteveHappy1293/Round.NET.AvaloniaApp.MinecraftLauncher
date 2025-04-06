using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.ExceptionMessage;

public class ExceptionMessage
{
    public static readonly string ExceptionsPath = Path.GetFullPath("../RMCL/RMCL.Exception/Exceptions/");
    public static void LogException(ExceptionEntry exceptionEntry)
    {
        string fileName = Path.Combine(ExceptionsPath,$"RMCL3-Exception-{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000,9999)}.json");
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(exceptionEntry, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        if (!Directory.Exists(ExceptionsPath))
        {
            Directory.CreateDirectory(ExceptionsPath);
        }

        if (File.Exists(Path.GetFullPath(fileName)))
        {
            LogException(exceptionEntry);
            return;
        }
        File.WriteAllText(Path.GetFullPath(fileName), jsresult);
    }

    public static void CleanException()
    {
        if (Directory.Exists(ExceptionsPath))
        {
            Directory.Delete(ExceptionsPath, true);
        }
    }
    public static List<ExceptionEntry> GetExceptions()
    {
        List<ExceptionEntry> exceptions = new List<ExceptionEntry>();
        if (!Directory.Exists(ExceptionsPath))
        {
            Directory.CreateDirectory(ExceptionsPath);
            return exceptions;
        }

        foreach (var file in Directory.GetFiles(ExceptionsPath))
        {
            var json = File.ReadAllText(file);
            exceptions.Add(JsonSerializer.Deserialize<ExceptionEntry>(json));
        }
        
        return exceptions;
    }

    public static ExceptionEnum GetExceptionSeverity(Exception ex)
    {
        return ex switch
        {
            NullReferenceException => ExceptionEnum.Critical,
            ArgumentNullException => ExceptionEnum.Error,
            ArgumentException => ExceptionEnum.Error,
            FileNotFoundException => ExceptionEnum.Warning,
            UnauthorizedAccessException => ExceptionEnum.Error,
            OutOfMemoryException => ExceptionEnum.Critical,
            StackOverflowException => ExceptionEnum.Critical,
            TimeoutException => ExceptionEnum.Error,
            _ => ex.Message.Contains("致命") || ex.Message.Contains("严重") 
                ? ExceptionEnum.Critical 
                : ExceptionEnum.Error
        };
    }
    public static string GetExceptionLevelText(ExceptionEnum level)
    {
        return level switch
        {
            ExceptionEnum.Information => "信息",
            ExceptionEnum.Warning => "警告",
            ExceptionEnum.Error => "错误",
            ExceptionEnum.Critical => "危险",
            _ => "未知"
        };
    }
}