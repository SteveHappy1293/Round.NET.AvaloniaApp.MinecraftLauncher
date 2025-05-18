using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace RMCL.Views.Pages.TaskView;

public partial class TaskView : UserControl
{
    public TaskView()
    {
        InitializeComponent();

        Show();
    }

    public bool IsOpen { get; set; } = true;

    public void Show()
    {
        if (IsOpen)
        {
            BackGrid.Opacity = 0;
            MainPanel.Margin = new Thickness(320,0,-320,0);
            Task.Run(() =>
            {
                Thread.Sleep(500);
                Dispatcher.UIThread.Invoke(() =>
                    this.IsVisible = false);
            });
        }
        else
        {
            this.IsVisible = true;
            BackGrid.Opacity = 0.6;
            MainPanel.Margin = new Thickness(0);
        }
        IsOpen = !IsOpen;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Show();
    }

    private void BackGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Show();
    }

    public void DeleteTask(string uuid)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            foreach (var item in TasksView.Children)
            {
                if (ReferenceEquals(item.Tag, uuid))
                {
                    TasksView.Children.Remove(item);
                }
            }
        });
    }
    public void AddTask(UserControl view)
    {
        TasksView.Children.Add(view);
    }
}