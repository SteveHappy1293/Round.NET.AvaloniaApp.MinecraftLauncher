using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using RMCL.Config;

namespace RMCL.Core.Models.Classes.Manager.StyleManager;

public class ColorHelper
{
    public static IBrush GetBackColor()
    {
        if (Config.Config.MainConfig.Theme == ThemeType.Dark)
        {
            return Brush.Parse("#373737");
        }
        else
        {
            return Brush.Parse("#CDCDCD");
        }
    }

    public static Color GetDominantColor(Bitmap bitmap)
    {
        var colors = new Dictionary<Color, int>();
        var size = bitmap.PixelSize;

        // 计算正确的 stride (每行字节数)
        // 对于32位RGBA格式，通常 stride = width * 4
        int stride = size.Width * 4;
        var bufferSize = stride * size.Height;
        var pixelBytes = new byte[bufferSize];

        // 使用不安全代码或固定内存来复制像素数据
        unsafe
        {
            fixed (byte* ptr = pixelBytes)
            {
                // 使用 IntPtr 重载
                bitmap.CopyPixels(
                    new PixelRect(0, 0, size.Width, size.Height),
                    (IntPtr)ptr,
                    bufferSize,
                    stride);
            }
        }

        for (int i = 0; i < pixelBytes.Length; i += 4)
        {
            byte b = pixelBytes[i];
            byte g = pixelBytes[i + 1];
            byte r = pixelBytes[i + 2];
            byte a = pixelBytes[i + 3];

            // 忽略透明或半透明像素
            if (a < 128) continue;

            var color = Color.FromRgb(r, g, b);

            // 简化颜色以减少变化（可选）
            var simplifiedColor = Color.FromRgb(
                (byte)(color.R / 10 * 10),
                (byte)(color.G / 10 * 10),
                (byte)(color.B / 10 * 10));

            if (colors.ContainsKey(simplifiedColor))
                colors[simplifiedColor]++;
            else
                colors[simplifiedColor] = 1;
        }

        return colors.OrderByDescending(x => x.Value).First().Key;
    }
}