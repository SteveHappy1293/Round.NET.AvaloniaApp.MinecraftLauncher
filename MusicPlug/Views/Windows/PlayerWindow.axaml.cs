using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using MusicPlug.Modules;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Animation.Easings;
using Avalonia.Input;

namespace MusicPlug.Views.Windows
{
    public partial class PlayerWindow : Window
    {
        private bool _isDragging = false;
        private ulong musicID = 0;
        private List<(TimeSpan Time, string Lyric)> _lyrics;

        public PlayerWindow()
        {
            InitializeComponent();

            // 设置窗口打开时的回调
            this.Opened += (sender, e) =>
            {
                PositionWindow();
                SetPlaybackCallback();
            };

            // 设置窗口关闭时的清理逻辑（如果需要）
            this.Closing += (sender, e) =>
            {
                // 如果需要清理资源，可以在这里进行
                // 但不要移除 PlayerCore.BackAction 的设置
            };
        }

        private void PositionWindow()
        {
            var screen = Screens.ScreenFromVisual(this);
            if (screen == null) return;

            var workingArea = screen.WorkingArea;

            var left = workingArea.Width - this.Width - 80;
            var top = workingArea.Height - this.Height - 80;

            this.Position = new PixelPoint((int)left, (int)top);
            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }
        public double newvalue = 0;
        private void SetPlaybackCallback()
        {
            PlayerCore.BackAction = (span, timeSpan) =>
            {
                try
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        try
                        {
                            ProgressSlider.Value = timeSpan.TotalMicroseconds;
                            if (ProgressSlider.Maximum != span.TotalMicroseconds) ProgressSlider.Maximum = span.TotalMicroseconds;
                            DurationText.Text = $"-{(span - timeSpan).ToString(@"mm\:ss")}";
                            CurrentTimeText.Text = $"{timeSpan.ToString(@"mm\:ss")}";
                            PlayerBox.Text = PlayerCore.MusicInfo.Name;
                            newvalue = timeSpan.TotalMicroseconds;
                            if (PlayerCore.MusicInfo.Id != musicID)
                            {
                                musicID = PlayerCore.MusicInfo.Id;
                                
                                var saveDir = Path.Combine(Path.GetFullPath("../RMCL/RMCL.MusicPlug/Music"), musicID.ToString());
                                string lyricsPath = Path.Combine(saveDir, $"{musicID}.lrc");
                                LoadLyrics(Path.GetFullPath(lyricsPath));
                            };

                            // 更新歌词显示
                            UpdateLyricsDisplay(timeSpan);
                        }
                        catch { }
                    });
                }
                catch { }
            };
        }

        private void ProgressSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.OldValue != newvalue) PlayerCore.SetPosition((long)ProgressSlider.Value);
        }
        
        bool IsTimestampedLyricLine(string line)
        {
            // 检查是否以 '[' 开头，并且包含 ']'
            if (!line.StartsWith("[") || !line.Contains("]"))
            {
                return false;
            }
            
            // 提取时间戳部分
            int bracketIndex = line.IndexOf(']');
            if (bracketIndex == -1) return false;

            string timePart = line.Substring(1, bracketIndex - 1);

            // 尝试解析时间戳
            TimeSpan time;
            return TimeSpan.TryParseExact(timePart, @"mm\:ss\.ff", null, out time);
        }
        private void LoadLyrics(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"Lyrics file not found: {filePath}");
                return;
            }
            
            _lyrics = new List<(TimeSpan Time, string Lyric)>();
            var lc = System.IO.File.ReadAllLines(filePath);
            foreach (var lci in lc)
            {
                if (!IsTimestampedLyricLine(lci)) continue;

                // 提取时间戳部分和歌词部分
                int bracketIndex = lci.IndexOf(']');
                if (bracketIndex == -1) continue; // 如果没有找到 ']'，跳过这一行

                string timePart = lci.Substring(1, bracketIndex - 1); // 去掉 '['
                string lyricPart = lci.Substring(bracketIndex + 1).Trim(); // 提取歌词部分并去掉多余空格

                // 尝试解析时间戳
                if (TimeSpan.TryParseExact(timePart, @"mm\:ss\.ff", null, out TimeSpan time))
                {
                    _lyrics.Add((Time: time, Lyric: lyricPart));
                }
                else
                {
                    Console.WriteLine($"Failed to parse timestamp: {timePart}");
                }
            }

            // 清空歌词面板
            LyricsPanel.Children.Clear();

            // 添加歌词到面板
            foreach (var (time, lyric) in _lyrics)
            {
                Console.WriteLine(lyric);
                var textBlock = new TextBlock
                {
                    Text = lyric,
                    Opacity = 0.5 // 默认不透明度
                };
                LyricsPanel.Children.Add(textBlock);
            }
        }

        private void UpdateLyricsDisplay(TimeSpan currentTime)
        {
            if (_lyrics == null || _lyrics.Count == 0) return;

            // 找到最接近 currentTime 的歌词
            var currentLyric = _lyrics.LastOrDefault(l => l.Time <= currentTime|| currentTime == TimeSpan.Zero);

            // 设置所有歌词的透明度为 0.5
            foreach (var child in LyricsPanel.Children)
            {
                if (child is TextBlock textBlock)
                {
                    textBlock.Opacity = 0.5;
                }
            }

            // 找到当前歌词对应的 TextBlock 并设置透明度为 1.0
            var currentTextBlock = LyricsPanel.Children
                .OfType<TextBlock>()
                .FirstOrDefault(tb => tb.Text == currentLyric.Lyric);

            if (currentTextBlock != null)
            {
                currentTextBlock.Opacity = 1.0; // 高亮当前歌词
                
                double targetOffset = currentTextBlock.Bounds.Top;

                // 设置 VerticalOffset
                lcScrollViewer.Offset = new Vector(0,  targetOffset);
            }
        }
    }
}