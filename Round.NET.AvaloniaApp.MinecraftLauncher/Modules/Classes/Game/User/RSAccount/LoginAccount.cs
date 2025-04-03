using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User.RSAccount;

public class LoginAccount
{
    public int Port = new Random().Next(1000,65500);
    public string LocalIPAddress = String.Empty;
    public LoginAccountServer Server { get; set; }
    public LoginAccount()
    {
        Server = new LoginAccountServer(Port);
        Server.Start();
        
        var LocalIP = $"http://127.0.0.1:{Port}";
        
        byte[] bytes = Encoding.UTF8.GetBytes(LocalIP);
        string base64String = Convert.ToBase64String(bytes);
        
        var BaseLocalIP = base64String;
        LocalIPAddress = $"{Config.Config.MainConfig.LoginServer}/login?clientID={BaseLocalIP}";
    }

    public void Login()
    {
        OpenUrl(LocalIPAddress);
    }
    public void OpenUrl(string url)
    {
        try
        {
            // 使用 Process.Start 打开默认浏览器
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            // 如果 Process.Start 失败，可能是由于 UseShellExecute = true 在非 Windows 系统上不支持
            RLogs.WriteLog("Failed to open URL using Process.Start. Trying alternative method...");
            RLogs.WriteLog(ex.Message);

            // 尝试使用平台特定的命令
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // 在 Linux 上使用 xdg-open
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // 在 macOS 上使用 open
                Process.Start("open", url);
            }
            else
            {
                RLogs.WriteLog("Unsupported platform. Unable to open URL.");
            }
        }
    }
}