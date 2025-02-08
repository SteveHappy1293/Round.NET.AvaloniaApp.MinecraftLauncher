using System;
using System.IO;
using System.Reflection;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;

public class PlugsLoader
{
    public static void LoadingPlug()
    {
        if (!Directory.Exists(Path.GetFullPath("../RMCL.Plugs")))
        {
            Directory.CreateDirectory(Path.GetFullPath("../RMCL.Plugs"));
            return;
        }

        foreach (var libfile in Directory.GetFiles(Path.GetFullPath("../RMCL.Plugs")))
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
    }
}