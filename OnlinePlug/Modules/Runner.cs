using System.Diagnostics;
using System.Text;

namespace OnlinePlug.Modules;

public class Runner
{
    public string FilePath { get; set; }
    public Action<string> OutputAction { get; set; }

    public Runner(string filePath = "")
    {
        FilePath = filePath;
    }

    public Process Run()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = FilePath,
            RedirectStandardOutput = true,  // 重定向标准输出
            RedirectStandardError = true,   // 重定向错误输出
            UseShellExecute = false,       // 不使用系统shell启动
            CreateNoWindow = true,         // 不创建窗口
            WindowStyle = ProcessWindowStyle.Hidden // 隐藏窗口（双重保障）
        };

        var outputBuilder = new StringBuilder();
        var process = new Process { StartInfo = processStartInfo };

        // 设置输出数据接收事件
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                outputBuilder.AppendLine(e.Data);
                OutputAction?.Invoke(e.Data);
            }
        };

        // 设置错误数据接收事件
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                outputBuilder.AppendLine("[ERROR] " + e.Data);
                OutputAction?.Invoke(e.Data);
            }
        };

        try
        {
            // 启动进程
            process.Start();
            
            // 开始异步读取输出
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        catch (Exception ex)
        {
            
        }
        return process;
    }
}