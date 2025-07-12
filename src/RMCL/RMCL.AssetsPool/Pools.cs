namespace RMCL.AssetsPool;

public class Pools
{
    public static AvaloniaResourcesPool AvaloniaResourcesPool = new(
        inactiveTimeout: TimeSpan.FromSeconds(5),
        cleanupInterval: TimeSpan.FromSeconds(10));
    
    public static ImageResourcePool ImageResourcePool = new ImageResourcePool(
        inactiveTimeout: TimeSpan.FromSeconds(5),
        cleanupInterval: TimeSpan.FromSeconds(10));
}