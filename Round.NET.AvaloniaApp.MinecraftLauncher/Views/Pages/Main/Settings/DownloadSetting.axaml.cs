using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using MinecraftLaunch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class DownloadSetting : UserControl
{
    public bool IsEdit { get; set; } = false;
    public DownloadSetting()
    {
        InitializeComponent();
        DownloadThreadsCountSlider.Value = Config.MainConfig.DownloadThreads;
        DownloadThreadsCountLabel.Content = $"下载线程 （{(int)DownloadThreadsCountSlider.Value}）：";
        IsEdit = true;
    }

    private void DownloadThreadsCountSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEdit)
        {
            DownloadThreadsCountLabel.Content = $"下载线程 （{(int)DownloadThreadsCountSlider.Value}）：";
            Config.MainConfig.DownloadThreads = (int)DownloadThreadsCountSlider.Value;
            Config.SaveConfig();
            
            DownloadMirrorManager.MaxThread = Config.MainConfig.DownloadThreads;
            DownloadMirrorManager.IsEnableMirror = false;
        }
    }
}