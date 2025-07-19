using System.Collections.Concurrent;
using Avalonia.Media.Imaging;

namespace RMCL.AssetsPool;

public class ImageResourcePool : IDisposable
{
    private readonly ConcurrentDictionary<string, (Bitmap image, DateTime lastAccess)> _pool = new();
    private readonly Timer _cleanupTimer;
    private readonly TimeSpan _inactiveTimeout;
    private readonly object _disposeLock = new();
    private bool _disposed;

    /// <summary>
    /// 初始化图片资源池
    /// </summary>
    /// <param name="inactiveTimeout">未访问超时时间</param>
    /// <param name="cleanupInterval">清理检查间隔</param>
    public ImageResourcePool(TimeSpan inactiveTimeout, TimeSpan cleanupInterval)
    {
        _inactiveTimeout = inactiveTimeout;
        _cleanupTimer = new Timer(CleanupCallback, null, cleanupInterval, cleanupInterval);
    }

    /// <summary>
    /// 获取图片资源
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    /// <returns>Bitmap 对象</returns>
    public Bitmap GetImage(string imagePath,int width = 0)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(ImageResourcePool));

        // 创建缓存键，包含宽度信息以支持不同尺寸的缓存
        var cacheKey = width > 0 ? $"{imagePath}_{width}" : imagePath;

        // 尝试从缓存获取
        if (_pool.TryGetValue(cacheKey, out var entry))
        {
            // 更新最后访问时间
            _pool[cacheKey] = (entry.image, DateTime.Now);
            return entry.image;
        }

        // 加载新图片，使用更高效的方式
        Bitmap image;
        try
        {
            if (width > 0)
            {
                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan);
                image = Bitmap.DecodeToWidth(stream, width);
            }
            else
            {
                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan);
                image = new Bitmap(stream);
            }

            _pool[cacheKey] = (image, DateTime.Now);
            return image;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load image {imagePath}: {ex.Message}");
            // 返回一个默认的空白图片或重新抛出异常
            throw;
        }
    }

    /// <summary>
    /// 预加载图片到资源池
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    public void PreloadImage(string imagePath, int width = 0)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(ImageResourcePool));

        var cacheKey = width > 0 ? $"{imagePath}_{width}" : imagePath;
        if (!_pool.ContainsKey(cacheKey))
        {
            try
            {
                Bitmap image;
                if (width > 0)
                {
                    using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan);
                    image = Bitmap.DecodeToWidth(stream, width);
                }
                else
                {
                    using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan);
                    image = new Bitmap(stream);
                }
                _pool[cacheKey] = (image, DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to preload image {imagePath}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 异步预加载图片到资源池
    /// </summary>
    /// <param name="imagePath">图片路径</param>
    /// <param name="width">目标宽度</param>
    public async Task PreloadImageAsync(string imagePath, int width = 0)
    {
        await Task.Run(() => PreloadImage(imagePath, width));
    }

    /// <summary>
    /// 批量异步预加载图片
    /// </summary>
    /// <param name="imagePaths">图片路径列表</param>
    /// <param name="width">目标宽度</param>
    public async Task PreloadImagesAsync(IEnumerable<string> imagePaths, int width = 0)
    {
        var tasks = imagePaths.Select(path => PreloadImageAsync(path, width));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// 清理回调
    /// </summary>
    private void CleanupCallback(object state)
    {
        lock (_disposeLock)
        {
            if (_disposed) return;

            var now = DateTime.Now;
            foreach (var kvp in _pool)
            {
                if (now - kvp.Value.lastAccess > _inactiveTimeout)
                {
                    if (_pool.TryRemove(kvp.Key, out var oldEntry))
                    {
                        oldEntry.image.Dispose();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 手动清理所有未使用的资源
    /// </summary>
    public void Cleanup()
    {
        CleanupCallback(null);
    }

    /// <summary>
    /// 释放资源池
    /// </summary>
    public void Dispose()
    {
        lock (_disposeLock)
        {
            if (_disposed) return;
            _disposed = true;

            _cleanupTimer?.Dispose();

            foreach (var kvp in _pool)
            {
                kvp.Value.image.Dispose();
            }
            _pool.Clear();
        }
        GC.SuppressFinalize(this);
    }

    ~ImageResourcePool()
    {
        Dispose();
    }
}