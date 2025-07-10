using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace RMCL.Controls.Container;

public partial class ChildFrame : UserControl
{
    public Action ShowedCallBack { get; set; } = () => { };
    public Action ClosedCallBack { get; set; } = () => { };
    public ChildFrame()
    {
        InitializeComponent();
        
        // 订阅布局更新事件
        this.LayoutUpdated += OnLayoutUpdated;
    }

    private void OnLayoutUpdated(object sender, System.EventArgs e)
    {
        // 当布局更新时，可以在这里获取最新尺寸
        // var currentHeight = this.Bounds.Height;
    }
    public void Show(UserControl Page)
    {
        ShowedCallBack.Invoke();
        // 在设置新内容前释放旧内容
        if (MainFrame.Content is IDisposable disposableContent)
        {
            disposableContent.Dispose();
        }
        MainFrame.Content = Page;
        this.IsVisible = true;
        
        // 强制布局更新并等待测量完成
        Dispatcher.UIThread.Post(() => 
        {
            // 现在应该能获取到正确的高度
            var hei = this.Bounds.Height;
            
            MainFrame.Margin = new Thickness(0, hei, 0, -hei);
            Back111.Margin = new Thickness(0, hei, 0, -hei);
            MainFrame.Opacity = 0; // 初始透明
            Back111.Opacity = 0;
            
            Task.Run(() =>
            {
                Thread.Sleep(50); // 短暂延迟让UI响应
                Dispatcher.UIThread.Invoke(() => 
                {
                    MainFrame.Opacity = 1;
                    Back111.Opacity = 1;
                    MainFrame.Margin = new Thickness(0, 0, 0, 0);
                    Back111.Margin = new Thickness(0);
                });
            });
        });
    }

    public void Close()
    {
        ClosedCallBack.Invoke();
        if (MainFrame.Content is IDisposable disposableContent)
        {
            disposableContent.Dispose();
        }
        // 确保在UI线程上获取高度
        Dispatcher.UIThread.Post(() => 
        {
            var hei = this.Bounds.Height;
            
            MainFrame.Margin = new Thickness(0, hei, 0, -hei);
            Back111.Margin = new Thickness(0, hei, 0, -hei);
            MainFrame.Opacity = 0;
            Back111.Opacity = 0;
            
            Task.Run(() =>
            {
                Thread.Sleep(300);
                Dispatcher.UIThread.Invoke(() => this.IsVisible = false);
            });
        });
    }
}