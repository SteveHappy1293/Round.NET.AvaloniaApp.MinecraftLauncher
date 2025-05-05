using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Info;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class SystemTaskBox : UserControl
{
    public SystemTaskBox()
    {
        InitializeComponent();
        Core.SystemTask = this;
        Show();
        Task.Run(() =>
        {
            while (true)
            {
                var time = DateTime.Now.ToString("HH:mm:ss");
                Dispatcher.UIThread.Invoke(()=>TimeBox.Content = time);
                Thread.Sleep(100);
            }
        });
    }

    public void UpdateMessage()
    {
        MessageListBox.Children.Clear();
        Task.Run(() =>
        {
            try
            {
                for (int i = Message.Messages.Count - 1; i >= 0; i--)
                {
                    var e = Message.Messages[i];
                    // 处理 message
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        var messagebox = new MessageInfoBox(e.Message, e.Title)
                        {
                            Margin = new Thickness(0,5,0,5)
                        };

                        MessageListBox.Children.Add(messagebox);
                    });
                }
            }
            catch
            {
                 
            }
        });
    }

    public void Show()
    {
        if (IsVisible)
        {
            foreach (var VARIABLE in SystemMessageTaskMange.Tasks)
            {
                VARIABLE.OnMessageCenterOpen();
            }
            //MainPanel.Margin = new Thickness(8, 8, -400, 8);
            //MainPanel.Opacity = 0;
            BackGrid.Opacity = 0;
            //TimeBox.Margin = new Thickness(-50,50);
            //Trip1Box.Margin = new Thickness(-50,160);
            //MessageScrollViewer.Margin = new Thickness(-40 - 290, 40, 290, 0);
            Task.Run(() =>
            {
                Thread.Sleep(100);
                Dispatcher.UIThread.Invoke(() => this.IsVisible = false);
            });
        }
        else
        {
            //MainPanel.Margin = new Thickness(8);
            //MainPanel.Opacity = 1;
            BackGrid.Opacity = 1;
            this.IsVisible = true;
            //TimeBox.Margin = new Thickness(50);
            //Trip1Box.Margin = new Thickness(50,160);
            //MessageScrollViewer.Margin = new Thickness(0,0,8,8);
            UpdateMessage();
        }
    }

    private void BackGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Core.SystemTask.Show();
    }
}