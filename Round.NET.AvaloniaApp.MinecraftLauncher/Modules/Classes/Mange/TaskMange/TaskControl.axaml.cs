using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Base.Models.Network;
using MinecraftLaunch.Components.Downloader;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.DownloadGame;

public partial class TaskControl : UserControl
{
    public TaskControl()
    {
        SourceName = this.GetType().Name;
        InitializeComponent();
    }

    private string _title = "新任务";
    private string _sourceName = string.Empty;
    private Importance _importance = Importance.Normal;
    private DateTime _time = DateTime.Now;
    private bool _closable = true;
    private Control _content;
    public string Title
    {
        get
        {
            return _title;
        }
        protected set
        {
            TitleLabel.Content = value;
            _title = value;
        }
    }
    public string SourceName
    {
        get
        {
            return _sourceName;
        }
        protected set
        {
            _sourceName = value;
        }
    }

    public Importance Importance
    {
        get
        {
            return _importance;
        }
        protected set
        {
            _importance = value;
        }
    }

    public DateTime Time
    {
        get
        {
            return _time;
        }
        protected set
        {
            _time = value;
        }
    }

    public bool Closable
    {
        get
        {
            return _closable;
        }
        protected set
        {
            _closable = value;
        }
    }
    public static readonly StyledProperty<Control> TaskContentProperty =
        AvaloniaProperty.Register<TaskControl, Control>("ContentProperty");
    public Control TaskContent
    {
        get
        {
            return GetValue(TaskContentProperty);
        }
        protected set
        {
            ContentPresenter.Content = value;
            SetValue(TaskContentProperty, value);
        }
    }

    public void UpdateUI()
    {
        ContentPresenter.Content = Content;
    }
    public void Stop(StopReason reason)
    {
        
    }
}

public enum Importance
{
    Low = 0,
    Normal = 1,
    High = 2,
}

public enum StopReason
{
    None,
    UserCanceled,
    ApplicationExit,
}