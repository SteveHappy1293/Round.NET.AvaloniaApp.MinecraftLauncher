using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.DownloadGame;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

public class SystemMessageTaskMange
{
    private static readonly string _systemMessage = "RMCL 进程队列：当前无后台进程和通知。";
    private static string _message = _systemMessage;
    public static string Message
    {
        get
        {
            if (Core.SystemTask.Tasks.Count != 0)
            {
                _message = $"RMCL 进程队列：当前有 {Core.SystemTask.Tasks.Count} 条进程或通知";
            }
            else
            {
                _message = _systemMessage;
            }
            return _message;   
        }
    }

    public enum TaskType
    {
        Download,
        Information,
        Launch,
        Plug
    }
    public class TaskConfig
    {
        public string TUID { get; set; } = Guid.NewGuid().ToString();
        public UserControl Body { get; set; }
        public TaskType Type { get; set; }
    }
    public static string AddTask(UserControl content)
    {
        var uuid = Guid.NewGuid().ToString();
        Core.SystemTask.Show();
        Task.Run(()=>
        {
            Thread.Sleep(800);
            Dispatcher.UIThread.Invoke(() =>
            {
                var con = new TaskConfig()
                {
                    Body = content,
                    Type = TaskType.Download,
                    TUID = uuid
                };
                content.Margin = new Thickness(380, 5, -380, 0);
                // content.Opacity = 0;
                Core.SystemTask.Tasks.Add(con);
                Core.SystemTask.TaskListBox.Children.Add(con.Body);
                
                content.Margin = new Thickness(0, 5, 0, 0);
                // content.Opacity = 1;
            });
        });  
        return uuid;
    }

    public static void DeleteTask(string tuid)
    {
        foreach (var con in Core.SystemTask.Tasks)
        {
            if (con.TUID == tuid)
            {
                Dispatcher.UIThread.Invoke(() => con.Body.Margin = new Thickness(380, 5, -380, 0));
                // con.Body.Opacity = 0;
                Task.Run(() =>
                {
                    Task.Run(() =>
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            if (Core.SystemTask.IsVisible == true)
                            {
                                if (Core.SystemTask.Tasks.Count == 1)
                                {
                                    Task.Run(() =>
                                    {
                                        Thread.Sleep(500);
                                        Dispatcher.UIThread.Invoke(() => Core.SystemTask.Show());
                                    });
                                }
                            }
                        });
                    });
                    Thread.Sleep(800);
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        Core.SystemTask.TaskListBox.Children.Remove(con.Body);
                        Core.SystemTask.Tasks.Remove(con);
                    });
                });
                break;
            }
        }
    }
}