using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using MusicPlug.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace MusicPlug.Views.Pages.SubPages
{
    public partial class LocalMusic : UserControl, IPage
    {
        private readonly DispatcherTimer _refreshTimer;
        private readonly string _musicBasePath = Path.Combine("../RMCL/RMCL.MusicPlug/Music");
        private readonly StackPanel _musicListPanel;

        public LocalMusic()
        {
            InitializeComponent();
            _musicListPanel = this.FindControl<StackPanel>("MusicListPanel");

            // 设置定时刷新 (每30秒检查一次)
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _refreshTimer.Tick += async (s, e) => await LoadLocalMusic();
            _refreshTimer.Start();

            // 初始加载
            LoadLocalMusic().ConfigureAwait(false);
        }

        public void Open()
        {
            Core.MainWindow.ChangeMenuItems(new List<MenuItem>());
        }

        private async Task LoadLocalMusic()
        {
            try
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _musicListPanel.Children.Clear();
                });

                if (!Directory.Exists(_musicBasePath))
                {
                    Directory.CreateDirectory(_musicBasePath);
                    return;
                }

                var musicDirs = Directory.GetDirectories(_musicBasePath);
                foreach (var dir in musicDirs)
                {
                    var jsonPath = Path.Combine(dir, "index.json");
                    if (!File.Exists(jsonPath)) continue;

                    var json = File.ReadAllText(jsonPath);
                    var musicInfo = JsonSerializer.Deserialize<Song>(json);

                    var musicItem = new MusicItem
                    {
                        Id = musicInfo.Id,
                        Title = musicInfo.Name ?? "未知歌曲",
                        Artist = string.Join("、", musicInfo.Artists.Select(x => x.Name)) ?? "未知艺术家",
                        AudioPath = Path.Combine(dir, $"{musicInfo.Id}.mp3"),
                        LyricsPath = Path.Combine(dir, $"{musicInfo.Id}.lrc"),
                        JsonPath = jsonPath
                    };

                    // 加载封面图片
                    var coverPath = Path.Combine(dir, "cover.jpg");
                    Bitmap coverImage = null;
                    if (File.Exists(coverPath))
                    {
                        await using var stream = File.OpenRead(coverPath);
                        coverImage = new Bitmap(stream);
                    }

                    // 创建 UI 元素
                    var musicItemPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 5),
                        Background = new SolidColorBrush(Color.Parse("#20000000")),
                    };

                    // 专辑封面
                    var coverImageControl = new Image
                    {
                        Width = 50,
                        Height = 50,
                        Stretch = Avalonia.Media.Stretch.UniformToFill,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                    if (coverImage != null)
                    {
                        coverImageControl.Source = coverImage;
                    }

                    // 歌曲信息
                    var songInfoPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    var titleTextBlock = new TextBlock
                    {
                        Text = musicItem.Title,
                        FontSize = 16,
                        FontWeight = FontWeight.Bold
                    };
                    var artistTextBlock = new TextBlock
                    {
                        Text = musicItem.Artist,
                        FontSize = 14,
                        Opacity = 0.8
                    };
                    songInfoPanel.Children.Add(titleTextBlock);
                    songInfoPanel.Children.Add(artistTextBlock);

                    // 播放按钮
                    var playButton = new Button
                    {
                        Content = "播放",
                        Width = 80,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                    };
                    playButton.Click += (sender, e) =>
                    {
                        PlayerCore.MusicInfo = musicInfo;
                        PlayMusic(musicItem);
                    };

                    // 添加到 StackPanel
                    musicItemPanel.Children.Add(coverImageControl);
                    musicItemPanel.Children.Add(songInfoPanel);
                    musicItemPanel.Children.Add(playButton);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        _musicListPanel.Children.Add(musicItemPanel);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载本地音乐失败: {ex.Message}");
            }
        }

        private void RefreshButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            LoadLocalMusic().ConfigureAwait(false);
        }

        // 播放命令
        public void PlayMusic(MusicItem item)
        {
            if (File.Exists(item.AudioPath))
            {
                // 停止当前播放
                PlayerCore.StopPlayback();
                // 更换音频文件
                PlayerCore.ChangeAudio(item.AudioPath);
                // 开始播放
                PlayerCore.TogglePlayPause();
            }
        }
    }

    public class MusicItem
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AudioPath { get; set; }
        public string LyricsPath { get; set; }
        public string JsonPath { get; set; }
        public Bitmap CoverImage { get; set; }
    }
}