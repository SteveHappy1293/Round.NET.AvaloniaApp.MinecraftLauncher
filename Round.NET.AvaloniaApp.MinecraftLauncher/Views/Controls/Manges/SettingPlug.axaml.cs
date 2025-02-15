using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Plugs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Manges;

public partial class SettingPlug : UserControl
{
    private PlugsLoader.PlugConfig Config;
    public SettingPlug(PlugsLoader.PlugConfig plug)
    {
        InitializeComponent();
        InformationBox.Content = $"介绍：{plug.Title}";
        Config = plug;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var originalFilePath = Config.FileName;
        string directory = Path.GetDirectoryName(originalFilePath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);

        // 新后缀名
        string newExtension = ".plug";

        // 构造新的文件路径
        string newFilePath = directory + "\\" + fileNameWithoutExtension + newExtension;

        // 重命名文件
        File.Move(originalFilePath, newFilePath);

        RLogs.WriteLog($"文件后缀已更改为：{newFilePath}");
        var con = (ContentDialog)this.Parent;
        con.Hide();
    }
}