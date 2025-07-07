using RMCL.Base.Entry.Java.Analyzer;

namespace RMCL.LogAnalyzer.Minecraft;

public class MinecraftLogAnalyzer
{
    public static string AnalyzerOutText(string logContent)
    {
        try
        {
            // 解析异常
            var exceptions = JavaExceptionParser.ParseExceptions(logContent);
            Console.WriteLine($"解析到 {exceptions.Count} 个异常");
            
            // 分析异常
            var analysis = JavaExceptionAnalyzer.AnalyzeExceptions(exceptions);
            
            // 生成报告
            var report = ExceptionReportGenerator.GenerateReport(analysis);
            Console.WriteLine(report);
            
            // 可选：保存报告到文件
            return report;
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"分析过程中发生错误: {ex.Message}");
        }

        return "";
    }
    
    public static ExceptionAnalysisResult Analyzer(string logContent)
    {
        try
        {
            var exceptions = JavaExceptionParser.ParseExceptions(logContent);
            Console.WriteLine($"解析到 {exceptions.Count} 个异常");
            
            // 分析异常
            var analysis = JavaExceptionAnalyzer.AnalyzeExceptions(exceptions);
            return analysis;
        }catch (Exception ex)
        {
            Console.WriteLine($"分析过程中发生错误: {ex.Message}");
        }

        return null;
    }
    
    public static void AnalyzeLogFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"文件不存在: {filePath}");
            return;
        }
        
        string content = File.ReadAllText(filePath);
        var exceptions = JavaExceptionParser.ParseExceptions(content);
        var analysis = JavaExceptionAnalyzer.AnalyzeExceptions(exceptions);
        var report = ExceptionReportGenerator.GenerateReport(analysis);
        
        Console.WriteLine(report);
    }
}