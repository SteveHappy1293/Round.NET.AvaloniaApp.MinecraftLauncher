namespace RMCL.AssetsPool;

public class Pools
{
    public static ImageResourcePool ImageResourcePool = new ImageResourcePool(
        inactiveTimeout: TimeSpan.FromSeconds(5),
        cleanupInterval: TimeSpan.FromSeconds(10));
}