namespace RMCL.Base.Entry.Java.Analyzer;

public class ExceptionAnalysisResult
{
    public int TotalExceptions { get; set; }
    public int UniqueExceptionTypes { get; set; }
    public string MostCommonException { get; set; }
    public Dictionary<string, int> ExceptionTypeStats { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> SeverityStats { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> CommonErrorLocations { get; set; } = new Dictionary<string, int>();
    public Dictionary<int, int> HourlyStats { get; set; } = new Dictionary<int, int>();
}