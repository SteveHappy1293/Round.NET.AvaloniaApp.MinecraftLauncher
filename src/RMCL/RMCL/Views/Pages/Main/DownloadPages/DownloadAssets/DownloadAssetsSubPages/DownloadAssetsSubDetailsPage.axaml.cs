using System;
using Avalonia.Controls;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;

namespace RMCL.Views.Pages.Main.DownloadPages.DownloadAssets.DownloadAssetsSubPages;

public partial class DownloadAssetsSubDetailsPage : UserControl
{
    public ModInfo Info { get; set; }
    public DownloadAssetsSubDetailsPage()
    {
        InitializeComponent();
    }

    public void Update()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            // 检查 Info 是否为空
            if (Info == null)
            {
                Console.WriteLine("Info is null!");
                return;
            }

            // 检查 Screenshots 是否为空
            if (Info.Screenshots == null || Info.Screenshots.Count == 0)
            {
                Console.WriteLine("No screenshots available.");
                return;
            }

            // 测试添加一个固定按钮
            TestBox.Children.Add(new Button { Content = "Test Button" });

            // 遍历添加
            foreach (var x in Info.Screenshots)
            {
                Console.WriteLine(x.Url); // 确认数据正确
                TestBox.Children.Add(new Button() { Content = "aa" });
            }
        });
    }
}