using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using MinecraftLaunch.Components.Downloader;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.DownloadGame;

public partial class DownloadGame : UserControl
{
    private string _version;

    public string Version
    {
        get => _version;
        set
        {
            _version = value;
            this.TitleLabel.Content = TitleLabel.Content.ToString().Replace("{Version}",_version);
        }
    }
    public string Tuid { get; set; } = string.Empty;
    public List<Object> Modloaders { get; set; } = new();
    public DownloadGame()
    {
        InitializeComponent();
    }

    public void StartDownloadAsync()
    {
        // // 检查 Modloaders 是否为空
        // if (Modloaders == null || !Modloaders.Any())
        // {
        //     Message.Show("下载任务", "没有可安装的 Mod 加载器。", InfoBarSeverity.Warning);
        //     return;
        // }

        // 显示进度条
        // foreach (var modloader in Modloaders)
        // {
        //     // Console.WriteLine($"Starting download of {typeof(ForgeInstallEntry)}");
        //     if (modloader.GetType().ToString().Contains(typeof(ForgeInstallEntry).ToString()))
        //     {
        //         ForgeJDBar.IsVisible = true;
        //     }
        //     else if (modloader.GetType().ToString().Contains(typeof(FabricInstallEntry).ToString()))
        //     {
        //         FabricJDBar.IsVisible = true;
        //     }
        //     else if (modloader.GetType().ToString().Contains(typeof(OptifineInstallEntry).ToString()))
        //     {
        //         OptfineJDBar.IsVisible = true;
        //     }
        //     else if (modloader.GetType().ToString().Contains(typeof(QuiltInstallEntry).ToString()))
        //     {
        //         QuiteJDBar.IsVisible = true;
        //     }
        // }

        Message.Show("下载任务", "下载任务已添加至后台。", InfoBarSeverity.Success);
        
        Task.Run(async () =>
        {
            #region 以往版本

            // // 下载游戏本体
            // var result = InstallGame.DownloadGame(Version, (args) =>
            // {
            //     Dispatcher.UIThread.Invoke(() =>
            //     {
            //         JDBar.Value = args.Progress * 100;
            //         JDLabel.Content = $"当前进度：安装本体 {args.Progress * 100:0.00} %";
            //     });
            // });
            //
            // if (!result)
            // {
            //     Message.Show("下载任务", $"游戏核心 {Version} 安装失败。", InfoBarSeverity.Error);
            //     return;
            // }
            //
            // // 安装 Mod 加载器
            // var modCount = Modloaders.Count;
            // var completedCount = 0;
            //
            // foreach (var modloader in Modloaders)
            // {
            //     Console.WriteLine(modloader);
            //     if (modloader is ValueTask<ForgeInstallEntry> forgeTask)
            //     {
            //         var forgeEntry = await forgeTask;
            //         InstallGame.InstallForge(Version, forgeEntry, (_, args) =>
            //         {
            //             Dispatcher.UIThread.Invoke(() =>
            //             {
            //                 ForgeJDBar.Value = args.Progress * 100;
            //                 JDLabel.Content = $"当前进度：安装 Forge {args.Progress * 100:0.00} %";
            //             });
            //         });
            //         completedCount++;
            //     }
            //     else if (modloader is ValueTask<FabricInstallEntry> fabricTask)
            //     {
            //         var fabricEntry = await fabricTask;
            //         InstallGame.InstallFabric(Version, fabricEntry, (_, args) =>
            //         {
            //             Dispatcher.UIThread.Invoke(() =>
            //             {
            //                 FabricJDBar.Value = args.Progress * 100;
            //                 JDLabel.Content = $"当前进度：安装 Fabric {args.Progress * 100:0.00} %";
            //             });
            //         });
            //         completedCount++;
            //     }
            //     else if (modloader is ValueTask<OptifineInstallEntry> optifineTask)
            //     {
            //         var optifineEntry = await optifineTask;
            //         InstallGame.InstallOptifine(Version, optifineEntry, (_, args) =>
            //         {
            //             Dispatcher.UIThread.Invoke(() =>
            //             {
            //                 OptfineJDBar.Value = args.Progress * 100;
            //                 JDLabel.Content = $"当前进度：安装 OptiFine {args.Progress * 100:0.00} %";
            //             });
            //         });
            //         completedCount++;
            //     }
            //     else if (modloader is ValueTask<QuiltInstallEntry> qValueTask)
            //     {
            //         var quiltEntry = await qValueTask;
            //         InstallGame.InstallQuilt(Version, quiltEntry, (_, args) =>
            //         {
            //             Dispatcher.UIThread.Invoke(() =>
            //             {
            //                 QuiteJDBar.Value = args.Progress * 100;
            //                 JDLabel.Content = $"当前进度：安装 Quilt {args.Progress * 100:0.00} %";
            //             });
            //         });
            //         completedCount++;
            //     }
            //
            //     if (completedCount == modCount)
            //     {
            //         SystemMessageTaskMange.DeleteTask(Tuid);
            //     }
            // }

            #endregion

            var installs = new List<IInstallEntry>();
            installs.Add(await InstallGame.DownloadGame(Version));
            foreach (var modloader in Modloaders)
            {
                Console.WriteLine(modloader);
                if (modloader is ValueTask<ForgeInstallEntry> forgeTask)
                {
                    installs.Add(await forgeTask);
                }
                else if (modloader is ValueTask<FabricInstallEntry> fabricTask)
                {
                    installs.Add(await fabricTask);
                }
                else if (modloader is ValueTask<OptifineInstallEntry> optifineTask)
                {
                    installs.Add(await optifineTask);
                }
                else if (modloader is ValueTask<QuiltInstallEntry> qValueTask)
                {
                    installs.Add(await qValueTask);
                }
            }

            InstallGame.InstallComposite(installs, "a", (args) =>
            {
                Dispatcher.UIThread.Invoke(() => JDBar.Value = (int)(args.Progress * 100));
                Dispatcher.UIThread.Invoke(()=>JDLabel.Content = $"当前进度：{args.Progress*100:0.00} %");
            });
        });
    }
}