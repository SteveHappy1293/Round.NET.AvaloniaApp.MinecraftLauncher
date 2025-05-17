using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.Button;

public partial class TaskButton : UserControl
{
    public TaskButton()
    {
        InitializeComponent();
        UpdateTaskStatus(0);
    }

    public void UpdateTaskStatus(int num)
    {
        if (num <= 0)
        {
            StatusDisplay.Text = "";
            TextBored.IsVisible = false;
            Width = 32;
        }
        else
        {
            StatusDisplay.Text = $"当前有{num}个任务正在运行";
            TextBored.IsVisible = true;
            Width = Double.NaN;
        }
    }
}