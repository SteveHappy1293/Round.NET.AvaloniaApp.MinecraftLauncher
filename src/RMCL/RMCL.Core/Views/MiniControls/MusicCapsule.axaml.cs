using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;

namespace RMCL.Core.Views.MiniControls
{
    public partial class MusicCapsule : UserControl
    {
        private CancellationTokenSource? _resetMarginCts;
        private readonly object _resetLock = new object();

        public MusicCapsule()
        {
            InitializeComponent();
        }

        public async void SetVolume(double volume)
        {
            ProgressBar.Value = volume;
            TextBlock.Text = $"音量 ({(int)volume}%)";
            // 更新UI显示
            this.Border.Margin = new Thickness(0, 100, 0, 0);
            
            // 取消之前的重置任务
            lock (_resetLock)
            {
                _resetMarginCts?.Cancel();
                _resetMarginCts = new CancellationTokenSource();
            }

            try
            {
                // 等待2秒后重置
                await Task.Delay(2000, _resetMarginCts.Token);
                
                // 确保在UI线程上执行
                Dispatcher.UIThread.Post(() => 
                {
                    this.Border.Margin = new Thickness(0, 0, 0, 0);
                });
            }
            catch (TaskCanceledException)
            {
                // 如果被取消，不做任何操作
            }
        }
    }
}