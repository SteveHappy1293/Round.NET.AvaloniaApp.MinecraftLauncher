using RMCL.Base.Entry.Java.Analyzer;

namespace RMCL.LogAnalyzer.Minecraft;

public class JavaExceptionAnalyzer
{
    public static ExceptionAnalysisResult AnalyzeExceptions(List<JavaException> exceptions)
    {
        var result = new ExceptionAnalysisResult();

        // 按类型统计
        result.ExceptionTypeStats = exceptions
            .GroupBy(e => e.ExceptionType)
            .ToDictionary(g => g.Key, g => g.Count());

        // 按严重程度统计
        result.SeverityStats = exceptions
            .Select(e => JavaExceptionClassifier.ClassifyException(e))
            .GroupBy(c => c.Severity)
            .ToDictionary(g => g.Key, g => g.Count());

        // 最常见的错误位置
        result.CommonErrorLocations = exceptions
            .Where(e => e.StackTrace.Any())
            .SelectMany(e => e.StackTrace)
            .GroupBy(s => $"{s.ClassName}.{s.MethodName}")
            .OrderByDescending(g => g.Count())
            .Take(10)
            .ToDictionary(g => g.Key, g => g.Count());

        // 时间分析（按小时统计）
        result.HourlyStats = exceptions
            .GroupBy(e => e.Timestamp.Hour)
            .ToDictionary(g => g.Key, g => g.Count());

        // 总体统计
        result.TotalExceptions = exceptions.Count;
        result.UniqueExceptionTypes = result.ExceptionTypeStats.Count;
        result.MostCommonException = result.ExceptionTypeStats
            .OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault().Key ?? "无";

        return result;
    }
}