using System;
using Avalonia.Threading;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Debug;

public class Fps
{
    private int _frameCount;
    private DateTime _lastTime = DateTime.Now;
    public Action<long> OnUpdate;
    public Fps()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16) // ~60Hz
        };

        timer.Tick += (_, _) =>
        {
            _frameCount++;

            if ((DateTime.Now - _lastTime).TotalSeconds >= 1.0)
            {
                double fps = _frameCount / (DateTime.Now - _lastTime).TotalSeconds;
                // Console.WriteLine($"FPS: {fps:0.00}");
                Dispatcher.UIThread.Invoke(() => OnUpdate((long)fps));

                _frameCount = 0;
                _lastTime = DateTime.Now;
            }
        };

        timer.Start();
    }
}