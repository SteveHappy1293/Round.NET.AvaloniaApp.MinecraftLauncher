using RMCL.Base.Entry.Java.Analyzer;

public class JavaExceptionClassifier
{
    private static readonly Dictionary<string, ExceptionCategory> ExceptionCategories = 
        new Dictionary<string, ExceptionCategory>
        {
            { "NullPointerException", new ExceptionCategory("空指针异常", "尝试访问空对象的属性或方法", "HIGH") },
            { "ClassNotFoundException", new ExceptionCategory("类找不到异常", "运行时找不到指定的类", "MEDIUM") },
            { "OutOfMemoryError", new ExceptionCategory("内存溢出错误", "JVM内存不足", "CRITICAL") },
            { "ArrayIndexOutOfBoundsException", new ExceptionCategory("数组越界异常", "数组索引超出范围", "HIGH") },
            { "IllegalArgumentException", new ExceptionCategory("非法参数异常", "方法接收到不合法的参数", "MEDIUM") },
            { "NumberFormatException", new ExceptionCategory("数字格式异常", "字符串转换为数字时格式错误", "LOW") },
            { "FileNotFoundException", new ExceptionCategory("文件找不到异常", "无法找到指定的文件", "MEDIUM") },
            { "IOException", new ExceptionCategory("IO异常", "输入输出操作失败", "MEDIUM") },
            { "SQLException", new ExceptionCategory("SQL异常", "数据库操作失败", "HIGH") },
            { "ConnectException", new ExceptionCategory("连接异常", "网络连接失败", "HIGH") }
        };

    public static ExceptionCategory ClassifyException(JavaException exception)
    {
        var exceptionName = exception.ExceptionType.Split('.').Last();
        
        if (ExceptionCategories.TryGetValue(exceptionName, out var category))
        {
            return category;
        }

        return new ExceptionCategory("未知异常", "未分类的异常类型", "UNKNOWN");
    }
}