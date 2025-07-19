namespace RMCL.AssetsPool;

public class Pools
{
    public static AvaloniaResourcesPool AvaloniaResourcesPool = new(
        inactiveTimeout: TimeSpan.FromMinutes(5), // 增加到5分钟
        cleanupInterval: TimeSpan.FromMinutes(2)); // 增加到2分钟

    public static ImageResourcePool ImageResourcePool = new ImageResourcePool(
        inactiveTimeout: TimeSpan.FromMinutes(5), // 增加到5分钟
        cleanupInterval: TimeSpan.FromMinutes(2)); // 增加到2分钟
}