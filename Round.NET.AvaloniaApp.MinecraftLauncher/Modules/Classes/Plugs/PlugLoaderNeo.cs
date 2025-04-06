using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;

public class PlugLoaderNeo
{
    public static List<PlugEntry> Plugs = new();
    public readonly static string PlugMainDirPath = Path.GetFullPath("../RMCL/RMCL.Plugs");
    public static void LoadPlugs()
    {
        #region Init

        Directory.CreateDirectory(PlugMainDirPath);
        Directory.CreateDirectory(Path.Combine(PlugMainDirPath, "Neo"));
        Directory.CreateDirectory(Path.Combine(PlugMainDirPath, "Neo\\PlugPacks"));
        Directory.CreateDirectory(Path.Combine(PlugMainDirPath, "Neo\\UnpackPlugs"));
        Directory.CreateDirectory(Path.Combine(PlugMainDirPath, "Neo\\Plugs"));

        #endregion
        #region ReadPlugPacks

        foreach (var plgpack in Directory.GetFiles(Path.Combine(PlugMainDirPath, "Neo\\PlugPacks")))
        {
            var path = Path.Combine(PlugMainDirPath, $"Neo\\UnpackPlugs\\{Path.GetFileName(plgpack)}");
            if (File.Exists(path)) Directory.Delete(path);
            Directory.CreateDirectory(path);
            ZipFile.ExtractToDirectory(plgpack, path,true);
            
            var js = File.ReadAllText(Path.Combine(path, "index.json"));
            var enrty = JsonSerializer.Deserialize<PlugFileEnty>(js);

            try
            {
                Plugs.Add(new PlugEntry()
                {
                    Icon = new Bitmap(path+enrty.IconPath),
                    Writer = enrty.PlugWriter,
                    Name = enrty.PlugName,
                    Notes = enrty.PlugVersion
                });
            }catch{ continue; }
        }

        #endregion
        #region CopyLib
        
        foreach (var plg in Directory.GetDirectories(Path.Combine(PlugMainDirPath, "Neo\\UnpackPlugs"))) // 变量插件包
        {
            foreach (var lib in Directory.GetFiles(Path.Combine(plg, $"lib"))) // 便利插件包lib
            {
                var locallibs = Directory.GetFiles("./"); // 获取当前已有的lib
                var loclabs = new List<string>();
                foreach (var onelib in locallibs) // 便利当前已有的lib
                {
                    loclabs.Add(Path.GetFileName(onelib));
                }

                if (!loclabs.Contains(Path.GetFileName(plg)))
                {
                    try
                    {
                        File.Copy(lib, Path.GetFileName(lib),true);
                    }catch { }
                }
            }
        }
        
        #endregion
        #region CopyPlug

        foreach (var plg in Directory.GetDirectories(Path.Combine(PlugMainDirPath, "Neo\\UnpackPlugs"))) // 便利解压包
        {
            var plgname = Path.GetFileName(Directory.GetFiles(Path.Combine(plg, "plug"))[0]);
            var plgpath = Path.Combine(plg,$"plug\\{plgname}");

            try
            {
                File.Copy(plgpath, Path.Combine(PlugMainDirPath,"Neo\\Plugs", plgname), true);
            }catch{ continue; }
        }

        #endregion

        #region LoadDll

        foreach (var pl in Directory.GetFiles(Path.Combine(PlugMainDirPath, "Neo\\Plugs")))
        {
            LoadDll(pl);
        }

        #endregion
    }

    public static void LoadDll(string libfile)
    {
        RLogs.WriteLog(libfile);
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
                    var item = new PlugsLoader.PlugConfig
                    {
                        FileName = libfile,
                    };
                    if (mainMethod != null)
                    {
                        mainMethod.Invoke(mainInstance, null);
                    }
                }
            }
        }
        catch (BadImageFormatException) { }
        catch (FileLoadException) { }
    }
}