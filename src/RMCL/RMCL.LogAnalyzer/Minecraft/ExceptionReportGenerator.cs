using RMCL.Base.Entry.Java.Analyzer;

public class ExceptionReportGenerator
{
    public static string GenerateReport(ExceptionAnalysisResult analysis)
    {
        var report = new System.Text.StringBuilder();
        
        report.AppendLine("=== Java异常分析报告 ===");
        report.AppendLine($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        report.AppendLine();
        
        // 总体统计
        report.AppendLine("## 总体统计");
        report.AppendLine($"异常总数: {analysis.TotalExceptions}");
        report.AppendLine($"异常类型数: {analysis.UniqueExceptionTypes}");
        report.AppendLine($"最常见异常: {analysis.MostCommonException}");
        report.AppendLine();
        
        // 按类型统计
        report.AppendLine("## 异常类型统计");
        foreach (var stat in analysis.ExceptionTypeStats.OrderByDescending(kvp => kvp.Value))
        {
            var percentage = (double)stat.Value / analysis.TotalExceptions * 100;
            report.AppendLine($"{stat.Key}: {stat.Value} ({percentage:F1}%)");
        }
        report.AppendLine();
        
        // 按严重程度统计
        report.AppendLine("## 严重程度统计");
        foreach (var stat in analysis.SeverityStats.OrderByDescending(kvp => kvp.Value))
        {
            report.AppendLine($"{stat.Key}: {stat.Value}");
        }
        report.AppendLine();
        
        // 常见错误位置
        report.AppendLine("## 常见错误位置 (Top 10)");
        foreach (var location in analysis.CommonErrorLocations)
        {
            report.AppendLine($"{location.Key}: {location.Value}次");
        }
        report.AppendLine();
        
        // 时间分布
        report.AppendLine("## 按小时分布");
        for (int hour = 0; hour < 24; hour++)
        {
            var count = analysis.HourlyStats.GetValueOrDefault(hour, 0);
            if (count > 0)
            {
                report.AppendLine($"{hour:D2}:00-{hour:D2}:59: {count}次");
            }
        }
        
        return report.ToString();
    }
}