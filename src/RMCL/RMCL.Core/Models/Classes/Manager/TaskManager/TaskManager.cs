using System;
using System.Collections.Generic;
using Avalonia.Controls;
using RMCL.Controls.Download;
using RMCL.Controls.TaskContentControl;

namespace RMCL.Core.Models.Classes.Manager.TaskManager;

public class TaskManager
{
    public class TaskEntry
    {
        public string Name { get; set; }
        public UserControl TaskView { get; set; }
        public string UUID { get; set; } = Guid.NewGuid().ToString().Replace("-","");
    }
    public static List<TaskEntry> TaskList = new();

    public static string AddTask(TaskControl taskView)
    {
        var it = new TaskEntry { TaskView = taskView };
        taskView.Tag = it.UUID;
        taskView.UUID = it.UUID;
        TaskList.Add(it);
        Core.TaskView.AddTask(taskView);
        RefuseUI();
        
        return it.UUID;
    }

    public static void DeleteTask(string uuid)
    {
        Core.TaskView.DeleteTask(uuid);
        TaskList.RemoveAll(x => x.UUID == uuid);
        RefuseUI();
    }

    public static void RefuseUI()
    {
        Core.MainWindow.UpdateStatus(TaskList.Count);
    }
}