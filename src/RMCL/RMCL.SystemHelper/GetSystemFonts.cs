using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Media;

namespace RMCL.SystemHelper;

public class GetSystemFonts
{
    private static List<string> _systemFontsCache;

    public static List<string> GetSystemFontFamilies()
    {
        // 使用缓存避免重复查询
        if (_systemFontsCache != null)
            return _systemFontsCache;
    
        try
        {
            var fontManager = FontManager.Current;
            var fontFamilies = new List<string>();
        
            foreach (var fontFamily in fontManager.SystemFonts)
            {
                if (!string.IsNullOrWhiteSpace(fontFamily.Name))
                {
                    fontFamilies.Add(fontFamily.Name);
                }
            }
        
            _systemFontsCache = fontFamilies.OrderBy(x => x).ToList();
            return _systemFontsCache;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"获取系统字体失败: {ex.Message}");
            return new List<string> { "Arial", "Courier New", "Times New Roman" }; // 返回默认字体列表
        }
    }
}