using System.Text.RegularExpressions;
using RMCL.Base.Entry.Java.Analyzer;

namespace RMCL.LogAnalyzer.Minecraft;

public class JavaExceptionParser
{
    // 匹配异常头部的正则表达式
    private static readonly Regex ExceptionHeaderRegex = new Regex(
        @"^(?:Exception in thread ""([^""]*)""\s+)?([a-zA-Z_][a-zA-Z0-9_.]*(?:Exception|Error))(?::\s*(.*))?$",
        RegexOptions.Compiled | RegexOptions.Multiline);

    // 匹配堆栈跟踪的正则表达式
    private static readonly Regex StackTraceRegex = new Regex(
        @"^\s*at\s+([a-zA-Z_][a-zA-Z0-9_.$]*)\.([a-zA-Z_][a-zA-Z0-9_$]*)\(([^)]*)\)$",
        RegexOptions.Compiled | RegexOptions.Multiline);

    // 匹配文件名和行号的正则表达式
    private static readonly Regex FileInfoRegex = new Regex(
        @"([a-zA-Z_][a-zA-Z0-9_]*\.java):(\d+)",
        RegexOptions.Compiled);

    public static List<JavaException> ParseExceptions(string logText)
    {
        var exceptions = new List<JavaException>();
        var lines = logText.Split('\n');
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            var match = ExceptionHeaderRegex.Match(line);
            
            if (match.Success)
            {
                var exception = new JavaException
                {
                    ThreadName = match.Groups[1].Value,
                    ExceptionType = match.Groups[2].Value,
                    Message = match.Groups[3].Value,
                    Timestamp = DateTime.Now, // 实际应用中需要从日志中解析时间戳
                    OriginalText = line
                };

                // 解析堆栈跟踪
                i++; // 移动到下一行
                while (i < lines.Length && lines[i].Trim().StartsWith("at "))
                {
                    var stackFrame = ParseStackFrame(lines[i].Trim());
                    if (stackFrame != null)
                    {
                        exception.StackTrace.Add(stackFrame);
                    }
                    i++;
                }
                i--; // 回退一行，因为外层循环会自动递增

                exceptions.Add(exception);
            }
        }

        return exceptions;
    }

    private static StackFrame ParseStackFrame(string stackLine)
    {
        var match = StackTraceRegex.Match(stackLine);
        if (!match.Success) return null;

        var className = match.Groups[1].Value;
        var methodName = match.Groups[2].Value;
        var locationInfo = match.Groups[3].Value;

        var stackFrame = new StackFrame
        {
            ClassName = className,
            MethodName = methodName,
            FullText = stackLine
        };

        // 解析文件名和行号
        var fileMatch = FileInfoRegex.Match(locationInfo);
        if (fileMatch.Success)
        {
            stackFrame.FileName = fileMatch.Groups[1].Value;
            if (int.TryParse(fileMatch.Groups[2].Value, out int lineNumber))
            {
                stackFrame.LineNumber = lineNumber;
            }
        }

        return stackFrame;
    }
}