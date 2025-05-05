using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Memory;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class JavaSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{});
    }
    public JavaSetting()
    {
        InitializeComponent();
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(RefreshMer);
                Thread.Sleep(1000);
                try
                {
                    if (Config.MainConfig.JavaUseMemory != int.Parse(MerInputBox.Text))
                    {
                        IsEnabled = false;
                        MerInputBox.Text = Config.MainConfig.JavaUseMemory.ToString();
                        IsEnabled = true;
                    }
                }catch{ }
            }
        });
        
        Task.Run(() => {
            // 更新 UI
            Modules.Java.FindJava.LoadJava();
            Thread.Sleep(300);
            Dispatcher.UIThread.Invoke(() =>
            {
                LoadJava();
            });
            
            IsEdit = true;
        }); // 更新Java设置下拉框
        MerInputBox.Text = Config.MainConfig.JavaUseMemory.ToString();
        JavaCheckBox.IsChecked = Config.MainConfig.IsLaunchJavaMemory;
    }

    public void RefreshMer()
    {
        // 获取总物理内存
        ulong totalMemory = Memory.GetTotalMemoryInBytes();
        ulong freeMemory = Memory.GetAvailableMemoryInBytes();
        UseMer.Maximum = totalMemory / 1024 / 1024; // 总内存（MB）
        MCMer.Maximum = totalMemory / 1024 / 1024; // 总内存（MB）

        // 计算总内存和已用内存的GB值
        double totalMemoryGB = (double)totalMemory / 1024 / 1024 / 1024;
        double usedMemoryGB = (double)(totalMemory - freeMemory) / 1024 / 1024 / 1024;
        double javaUseMemoryGB = (double)Config.MainConfig.JavaUseMemory / 1024; // Java使用的内存（GB）

        // 计算百分比
        double usedMemoryPercentage = (totalMemory - freeMemory) * 100.0 / totalMemory;
        double javaUseMemoryPercentage = Config.MainConfig.JavaUseMemory * 100.0 / (totalMemory / 1024 / 1024); // Java使用的内存占总内存的百分比

        // 设置标签内容
        PCMerLabel.Content = $"{totalMemoryGB:0.00} GB";
        UseMerLabel.Content = $"{usedMemoryGB:0.00} GB ({usedMemoryPercentage:0.00}%)";
        McMerLabel.Content = $"{javaUseMemoryGB:0.00} GB ({javaUseMemoryPercentage:0.00}%)";

        // 设置进度条的值
        UseMer.Value = (totalMemory - freeMemory) / 1024 / 1024; // 已用内存（MB）
        MCMer.Value = (ulong)Config.MainConfig.JavaUseMemory;
        // MCMer.Value = (totalMemory - freeMemory) / 1024 / 1024 + (ulong)Config.MainConfig.JavaUseMemory; // 已用内存 + Java使用的内存（MB）
    }
    public bool IsEdit { get; set; } = false;

    private void JavaComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) //Java选择下拉框选择回调
    {
        if (IsEdit)
        {
            Config.MainConfig.SelectedJava = JavaComboBox.SelectedIndex;
            Config.SaveConfig();
        }
    }

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            MerInputBox.IsEnabled = !(bool)((CheckBox)sender).IsChecked;
            Config.MainConfig.IsLaunchJavaMemory = (bool)((CheckBox)sender).IsChecked;
            if ((bool)((CheckBox)sender).IsChecked)
            {
                Config.MainConfig.JavaUseMemory = 4096;
                Config.SaveConfig();
            }
        }
    }

    private void MerInputBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (IsEdit)
        {
            try
            {
                Config.MainConfig.JavaUseMemory = int.Parse(((TextBox)sender).Text);
            }
            catch
            {
                Message.Show("错误","请输入数字！",InfoBarSeverity.Error);
            }
        }
    }

    private void RefuseJava_OnClick(object? sender, RoutedEventArgs e)
    {
        RefuseJava.Content = new ProgressRing()
        {
            Width = 15,
            Height = 15
        };
        RefuseJava.IsEnabled = false;
        
        Task.Run(() =>
        {
            Modules.Java.FindJava.JavasList.Clear();
            Modules.Java.FindJava.Find();
            Modules.Java.FindJava.SaveJava();
            Modules.Java.FindJava.LoadJava();
            Dispatcher.UIThread.Invoke(() =>
            {
                LoadJava();
                RefuseJava.Content = "刷新";
                RefuseJava.IsEnabled = true;
            });
        });
    }

    private void LoadJava()
    {
        IsEdit = false;
        JavaComboBox.Items.Clear();
        JavaComboBox.Items.Add(new ComboBoxItem() { Content = "[Auto] RMCL 自己选择合适的 Java 版本" });
        foreach (var java in Modules.Java.FindJava.JavasList)
        {
            JavaComboBox.Items.Add(new ComboBoxItem
            {
                Content = $"[{java.JavaVersion}] {java.JavaPath}"
            });
        }
        JavaComboBox.SelectedIndex = Config.MainConfig.SelectedJava;
        IsEdit = true;
    }
}