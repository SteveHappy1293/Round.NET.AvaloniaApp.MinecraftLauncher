using System;
using System.Collections.Generic;

namespace RMCL.SystemHelper;

public class SequenceString
{
    private List<SequenceStringEntry> Bodys = new();
    private int MaxCharCount = 0;

    public void Add(SequenceStringEntry item)
    {
        Bodys.Add(item);
        
        Bodys.ForEach(e =>
        {
            if (GetDisplayLength(e.Title) > MaxCharCount)
            {
                MaxCharCount = GetDisplayLength(e.Title);
            }
        });
    }

    public string GetResult()
    {
        var result = "";

        Bodys.ForEach(e =>
        {
            result += $"{e.Title}{new string(' ',(MaxCharCount-GetDisplayLength(e.Title))*2)}     ： {e.Text}\n";
        });
        return result.Substring(0,result.Length-1);
    }
    
    private int GetDisplayLength(string input)
    {
        int length = 0;
        foreach (char c in input)
        {
            // 判断字符是否为中文、日文、韩文等（CJK 统一表意文字）
            if (c >= '\u4e00' && c <= '\u9fff')
            {
                length += 2; // 中文占 2 个字符宽度
            }
            else
            {
                length += 1; // 其他字符占 1 个字符宽度
            }
        }
        return length;
    }
}

public class SequenceStringEntry
{
    public string Title { get; set; } = String.Empty;
    public string Text { get; set; } = String.Empty;
}