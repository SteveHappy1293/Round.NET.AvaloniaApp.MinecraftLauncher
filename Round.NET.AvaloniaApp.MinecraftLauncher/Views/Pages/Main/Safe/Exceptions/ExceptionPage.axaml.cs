using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.ExceptionMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Safe.Exceptions;

public partial class ExceptionPage : UserControl
{
    public ExceptionPage()
    {
        InitializeComponent();
        this.HomeIcon1.Click = (sender, args) =>
        {
            Core.MainWindow.MainView.ContentFrame.Opacity = 0;
            Core.MainWindow.MainView.ContentFrame.Content = new Grid();
            Core.MainWindow.MainView.MainContent.Opacity = 1;
            Core.NavigationBar.Opacity = 1;
        };

        this.Loaded += ((sender, args) => Load());
    }

    public IBrush GetExceptionBrushes(ExceptionEnum level)
    {
        return level switch
        {
            ExceptionEnum.Information => Brush.Parse("#9cc4e4"),
            ExceptionEnum.Warning => Brush.Parse("#fce100"),
            ExceptionEnum.Error => Brush.Parse("#ff99a4"),
            ExceptionEnum.Critical => Brush.Parse("#c42b1c"),
            _ => Brushes.Gray
        };
    }
    public void Load()
    {
        LoadingControl.IsVisible = true;
        NullControl.IsVisible = false;
        ExStackPanel.IsVisible = false;
        ExStackPanel.Children.Clear();
        Task.Run(() =>
        {
            // Thread.Sleep(1000);
            var exs = Modules.ExceptionMessage.ExceptionMessage.GetExceptions();
            var sortedExceptions = exs
                .OrderByDescending(e => e.RecordTime)
                .ToList();
            if (sortedExceptions.Count <= 0)
            {
                Dispatcher.UIThread.Invoke(() =>
                    NullControl.IsVisible = true);
                Dispatcher.UIThread.Invoke(() =>
                    LoadingControl.IsVisible = false);
                return;
            }
            foreach (var ex in sortedExceptions)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var liteam = new ListBoxItem()
                    {
                        Padding = new Thickness(15, 10),
                        Content = new StackPanel()
                        {
                            Children =
                            {
                                new DockPanel()
                                {
                                    Children =
                                    {
                                        new TextBlock()
                                        {
                                            Text = $"[{ExceptionMessage.GetExceptionLevelText(ex.ExceptionType)}]",
                                            FontSize = 18,
                                            Foreground = GetExceptionBrushes(ex.ExceptionType)
                                        },
                                        new TextBlock()
                                        {
                                            Text = $"发生于 {ex.RecordTime.ToString()} 的异常",
                                            FontSize = 18,
                                            Margin = new Thickness(5,0)
                                        },
                                    }
                                },
                                new TextBlock()
                                {
                                    Text = $"{ex.Exception}",
                                    FontSize = 13,
                                    TextTrimming = TextTrimming.CharacterEllipsis,
                                    Foreground = Brushes.Gray
                                }
                            }
                        }
                    };
                    ExStackPanel.Children.Add(liteam);
                });
            }
            
            Dispatcher.UIThread.Invoke(() =>
                LoadingControl.IsVisible = false);
            Dispatcher.UIThread.Invoke(() =>
                ExStackPanel.IsVisible = true);
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var con = new ContentDialog()
        {
            Title = "清除记录",
            Content = "你确定要清除所有异常记录吗？",
            CloseButtonText = "取消",
            PrimaryButtonText = "确定",
            DefaultButton = ContentDialogButton.Close
        };
        con.PrimaryButtonClick += (s, args) =>
        {
            ExceptionMessage.CleanException();
            Load();
        };
        con.ShowAsync();
    }
}