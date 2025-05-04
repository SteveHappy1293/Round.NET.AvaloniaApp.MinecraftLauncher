using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
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
            if (Tasks.Count != 0)
            {
                _message = $"RMCL 进程队列：当前有 {Tasks.Count} 条进程或通知";
            }
            else
            {
                _message = _systemMessage;
            }
            return _message;   
        }
    }

    public static void UpdateTaskUI(Guid tuid,Control control)
    {
        foreach (ContentPresenter controlpresenter in Core.SystemTask.TaskListBox.Children)
        {
            if (controlpresenter.Tag is Guid g && g == tuid)
                controlpresenter.Content = control;
        }
    }
    public static List<TaskControl> Tasks = new List<TaskControl>();
    public static void AddTask(TaskControl task)
    {
        Core.SystemTask.Show();
        Task.Run(()=>
        {
            Thread.Sleep(800);
            Dispatcher.UIThread.Invoke(() =>
            {
                //content.Margin = new Thickness(380, 5, -380, 0);
                // content.Opacity = 0;
                Tasks.Add(task);
                Core.SystemTask.TaskListBox.Children.Add(task);
                
                //content.Margin = new Thickness(0, 5, 0, 0);
                // content.Opacity = 1;
            });
        });  
    }

    public static void DeleteTask(Guid tuid,StopReason stopReason = StopReason.None)
    {
        //var task = Core.SystemTask.Tasks[tuid];
      //  task.Stop(stopReason);
     //   Core.SystemTask.TaskListBox.Children.Remove(task.UIControl);
        /*
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
        }*/
    }
}