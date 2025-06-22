using System.Reflection;

namespace RMCL.Plug;

public static class LoadPlugs
{
    public static List<string> Plugs = new List<string>();
    public static void LoadPlugins(string pluginsDirectory)
    {
        if (!Directory.Exists(pluginsDirectory)) Directory.CreateDirectory(pluginsDirectory);
        
        foreach (var dll in Directory.GetFiles(pluginsDirectory, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(dll);
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IPlug).IsAssignableFrom(type))
                {
                    if (Activator.CreateInstance(type) is IPlug plugin)
                    {
                        try
                        {
                            Console.WriteLine($"Registering plugin: {plugin.Name}");
                            // 注册或执行插件
                            plugin.Execute();
                            Plugs.Add(plugin.Name);
                            Console.WriteLine($"Registered plugin: {plugin.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Registered plugin {plugin.Name} Error: {ex}");
                        }
                    }
                }
            }
        }
    }
}