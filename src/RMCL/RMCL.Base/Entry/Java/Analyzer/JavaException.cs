namespace RMCL.Base.Entry.Java.Analyzer;

public class JavaException
{
    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public List<StackFrame> StackTrace { get; set; } = new List<StackFrame>();
    public DateTime Timestamp { get; set; }
    public string ThreadName { get; set; }
    public string OriginalText { get; set; }
}

public class StackFrame
{
    public string ClassName { get; set; }
    public string MethodName { get; set; }
    public string FileName { get; set; }
    public int LineNumber { get; set; }
    public string FullText { get; set; }
}