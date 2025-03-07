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
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            try
            {
                var JavaList = JavaUtil.EnumerableJavaAsync();
                foreach (var javalist in JavaList.ToListAsync().Result)
                {
                    JavasList.Add(javalist);
                }
            }
            catch
            {
                List<string> javaPaths = FindJavaInstallations();
                if (javaPaths.Count > 0)
                {
                    foreach (var path in javaPaths)
                    {
                        Console.WriteLine(path);
                        Version version = GetJavaVersion(path);
                        var is64 = Is64BitJava(path);
                        if (version != null)
                        {
                            JavasList.Add(new JavaEntry
                            {
                                Is64bit = is64,
                                JavaType = "不知道",
                                JavaPath = path.Replace("java", "javaw"),
                                JavaVersion = version
                            });
                        }
                    }
                }
            }
        }
        else
        {
            var JavaList = JavaUtil.EnumerableJavaAsync();
            foreach (var javalist in JavaList.ToListAsync().Result)
            {
                JavasList.Add(javalist);
            }
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
        string jsresult = Regex.Unescape(JsonSerializer
            .Serialize(JavasList, new JsonSerializerOptions() { WriteIndented = true })
            .Replace("\\", "\\\\")); //获取结果并转换成正确的格式
        File.WriteAllText(Path.GetFullPath(JavaFileName), jsresult);
    }

    // 查找所有Java安装路径
    public static List<string> FindJavaInstallations()
    {
        List<string> javaPaths = new List<string>();

        // 检查 PATH 环境变量
        string pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathEnv))
        {
            foreach (string path in pathEnv.Split(Path.PathSeparator))
            {
                string javaPath = Path.Combine(path, "java.exe");
                if (File.Exists(javaPath) && !javaPaths.Contains(javaPath))
                {
                    javaPaths.Add(javaPath);
                }
            }
        }

        // 检查 JAVA_HOME 环境变量
        string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (!string.IsNullOrEmpty(javaHome))
        {
            string javaPath = Path.Combine(javaHome, "bin", "java.exe");
            if (File.Exists(javaPath) && !javaPaths.Contains(javaPath))
            {
                javaPaths.Add(javaPath);
            }
        }

        // 扫描常见的Java安装目录
        string[] commonJavaPaths = new[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Java"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Java")
        };

        foreach (string commonPath in commonJavaPaths)
        {
            if (Directory.Exists(commonPath))
            {
                foreach (string dir in Directory.GetDirectories(commonPath))
                {
                    string javaPath = Path.Combine(dir, "bin", "java.exe");
                    if (File.Exists(javaPath) && !javaPaths.Contains(javaPath))
                    {
                        javaPaths.Add(javaPath);
                    }
                }
            }
        }

        return javaPaths;
    }

    // 获取Java版本信息
    public static Version GetJavaVersion(string javaPath)
    {
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = javaPath,
                Arguments = "-version",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                using (StreamReader reader = process.StandardError)
                {
                    string versionOutput = reader.ReadToEnd();
                    return ParseJavaVersion(versionOutput);
                }
            }
        }
        catch
        {
            return null;
        }
    }
    
    // 判断Java是不是64位
    static bool Is64BitJava(string javaPath)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = javaPath,
                    Arguments = "-version",
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // 检查输出中是否包含 "64-Bit"
            return output.Contains("64-Bit");
        }
        catch
        {
            return false;
        }
    }

    // 解析Java版本信息
    public static Version ParseJavaVersion(string versionOutput)
    {
        if (!string.IsNullOrEmpty(versionOutput))
        {
            string[] parts = versionOutput.Split(new[] { ' ' }, 4); // 按空格分割，最多分成 3 部分
            if (parts.Length >= 3)
            {
                string versionString  = parts[2].Replace("\"", "").Replace("Java(TM)", "").Trim();
                // 将下划线替换为点号，并移除非数字和点号的字符
                versionString = System.Text.RegularExpressions.Regex.Replace(versionString, @"[^0-9.]", ".");

                // 确保版本号至少有两部分（Major 和 Minor）
                string[] versionParts = versionString.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (versionParts.Length >= 2)
                {
                    // 解析 Major、Minor、Build 和 Revision
                    int major = int.Parse(versionParts[0]);
                    int minor = int.Parse(versionParts[1]);
                    int build = versionParts.Length > 2 ? int.Parse(versionParts[2]) : 0;
                    int revision = versionParts.Length > 3 ? int.Parse(versionParts[3]) : 0;

                    return new Version(major, minor, build, revision);
                }
            }
        }

        return null;
    }
}