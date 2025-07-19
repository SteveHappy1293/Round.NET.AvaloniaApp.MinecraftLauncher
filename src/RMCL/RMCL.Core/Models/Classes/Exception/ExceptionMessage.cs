using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using RMCL.Modules.Entry;
using RMCL.Modules.Enum;
using RMCL.PathsDictionary;
using RMCL.SystemHelper;

namespace RMCL.Core.Models.Classes.ExceptionMessage;

public class ExceptionMessage
{
    public static readonly string ExceptionsPath = Path.GetFullPath(PathsDictionary.PathDictionary.ExceptionFolder);
    public static void LogException(ExceptionEntry exceptionEntry)
    {
        string dirpath = Path.Combine(ExceptionsPath,
            $"{DateTime.Now:yyyy.MM.dd HHmmss} No.{new Random().Next(1000, 9999)}");
        string fileName = Path.Combine(dirpath, "Exception.json");
        string pakefileName = Path.Combine(dirpath, "Pack.rexp");
        string jsresult = Regex.Unescape(JsonSerializer.Serialize(exceptionEntry, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
        if (!Directory.Exists(ExceptionsPath))
        {
            Directory.CreateDirectory(ExceptionsPath);
        }
        Directory.CreateDirectory(dirpath);

        if (File.Exists(Path.GetFullPath(fileName)))
        {
            LogException(exceptionEntry);
            return;
        }

        File.WriteAllText(Path.GetFullPath(fileName), jsresult);
        var logpath = Path.Combine(dirpath, "Log.log");
        File.Copy(Logger.ConsoleRedirector.FileName, logpath);
        PackingConfigPack(pakefileName,fileName,logpath);
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

    public static ExceptionEnum GetExceptionSeverity(System.Exception ex)
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

    public static void PackingConfigPack(string packname,string expfilename,string logpath)
    {
        ZipHelper.ZipFolders(new[]
        {
            Path.Combine(PathDictionary.RootPath,"RMCL.Config"),
            Path.Combine(PathDictionary.RootPath,"RMCL.Plugin"),
            Path.Combine(PathDictionary.RootPath,"RMCL.Style"),
            expfilename,
            logpath
        }, packname);
    }
}