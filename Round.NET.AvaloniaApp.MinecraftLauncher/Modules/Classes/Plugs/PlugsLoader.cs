using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;

public class PlugsLoader
{
    public class PlugConfig
    {
        public string Title { get; set; } = "这个插件很懒，还没介绍捏...";
        public string FileName { get; set; }
    }

    public static List<PlugConfig> Plugs = new();
    public static void LoadingPlug()
    {
        if (!Config.Config.MainConfig.IsUsePlug) return;
        if (!Directory.Exists(Path.GetFullPath("../RMCL/RMCL.Plugs")))
        {
            Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.Plugs"));
            return;
        }

        foreach (var libfile in Directory.GetFiles(Path.GetFullPath("../RMCL/RMCL.Plugs")))
        {
            if (libfile.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(libfile);
                    if (assembly != null)
                    {
                        var namespaces = Path.GetFileNameWithoutExtension(libfile);
                        Type mainType = assembly.GetType($"{namespaces}.Main");
                        if (mainType != null)
                        {
                            object mainInstance = Activator.CreateInstance(mainType);
                            MethodInfo mainMethod = mainType.GetMethod("InitPlug");
                            var item = new PlugConfig
                            {
                                FileName = libfile,
                            };
                            if (mainMethod != null)
                            {
                                mainMethod.Invoke(mainInstance, null);
                            }
                            string title = GetPlugTitle(mainType);
                            if (!string.IsNullOrEmpty(title))
                            {
                                RLogs.WriteLog($"Plug Title: {title}");
                                item.Title = title;
                            }
                            
                            Plugs.Add(item);
                        }
                    }
                }
                catch (BadImageFormatException) { }
                catch (FileLoadException) { }
            }
        }
    }
    private static string GetPlugTitle(Type mainType)
    {
        PropertyInfo titleProperty = mainType.GetProperty("Title");
        if (titleProperty != null)
        {
            object titleValue = titleProperty.GetValue(null);
            return titleValue?.ToString();
        }
        return null;
    }

    public static void DisablePlug(PlugConfig plugConfig)
    {
        
    }
}