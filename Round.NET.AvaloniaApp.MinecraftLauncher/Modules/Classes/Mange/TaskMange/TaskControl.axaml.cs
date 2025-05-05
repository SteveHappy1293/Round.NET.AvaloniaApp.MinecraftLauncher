using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Downloader;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class TaskControl : UserControl
{
    public TaskControl()
    {
        SourceName = this.GetType().Name;
        InitializeComponent();
    }

    private string _title = "新任务";
    private string _sourceName = string.Empty;
    private string _description = string.Empty;
    private Importance _importance = Importance.Normal;
    private DateTime _time = DateTime.Now;
    private bool _closable = true;
    private Symbol _icon = Symbol.Code;
    private Control? _content = null;

    public Symbol Icon
    {
        get
        {
            return _icon;
        }
        set
        {
            SymbolIcon.Symbol = value;
            _icon = value;
        }
    }
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

    public string Description
    {
        get
        {
            return _description;
        }
        protected set
        {
            DescriptionLabel.Content = value;
            _description = value;
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
    
    public void Stop(StopReason reason = StopReason.None)
    {
        SystemMessageTaskMange.DeleteTask(this);
    }

    public void OnMessageCenterOpen()
    {
        var time = DateTime.Now - Time;
        if (time.TotalSeconds <= 60)
        {
            TimeLabel.Content = $"{(int)Math.Truncate(time.TotalSeconds)} 秒前，开始于 {Time}";
        }
        else if (time.TotalMinutes <= 60)
        {
            TimeLabel.Content = $"{(int)Math.Truncate(time.TotalMinutes)} 分钟前，开始于 {Time}";
        }
        else if (time.TotalHours <= 12)
        {
            TimeLabel.Content = $"{(int)Math.Truncate(time.TotalHours)} 小时前，开始于 {Time}";
        }
        else
        {
            TimeLabel.Content = $"{time}";
        }
    }

    public void OnMessageCenterClose()
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