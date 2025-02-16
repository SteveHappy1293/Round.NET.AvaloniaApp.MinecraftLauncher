using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Base.Models.Network;
using MinecraftLaunch.Components.Installer;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;

public partial class NewGame : UserControl
{
    private IAsyncEnumerable<ForgeInstallEntry> forgebuild;
    private IAsyncEnumerable<OptifineInstallEntry> optifinebuild;
    private IAsyncEnumerable<FabricInstallEntry> fabricbuild;
    private IAsyncEnumerable<QuiltInstallEntry> quiltbuild;
    private List<Object> installedObjects = new List<Object>();
    public NewGame(string version)
    {
        InitializeComponent();
        ChooseVersionBox.Description = version;
        ShowNameBox.Text = version;
        VersionTitle.Content = version;
        InitializeUIAsync(version);
    }

    public async Task InitializeUIAsync(string version)
    {
        forgebuild = (ForgeInstaller.EnumerableForgeAsync(version));
        optifinebuild = (OptifineInstaller.EnumerableOptifineAsync(version));
        fabricbuild = (FabricInstaller.EnumerableFabricAsync(version)); 
        quiltbuild = (QuiltInstaller.EnumerableQuiltAsync(version));
        await foreach (var mod in forgebuild)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                ForgeBox.Children.Add(new RadioButton()
                {
                    Content = mod.ForgeVersion,
                    Margin = new Thickness(5),
                });
            });
        }
            
        await foreach (var mod in optifinebuild)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                OptiFineBox.Children.Add(new RadioButton()
                {
                    Content = mod.FileName.Replace(".jar", ""),
                    Margin = new Thickness(5),
                });
            });
        }
            
        await foreach (var mod in fabricbuild)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                FabricBox.Children.Add(new RadioButton()
                {
                    Content = mod.BuildVersion,
                    Margin = new Thickness(5),
                });
            });
        }
            
        await foreach (var mod in quiltbuild)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                QuiltBox.Children.Add(new RadioButton()
                {
                    Content = mod.BuildVersion,
                    Margin = new Thickness(5),
                });
            });
        }
    }

    private void BackExpander_OnClick(object? sender, RoutedEventArgs e)
    {
        // var con = (ContentDialog)this.Parent;
        // con.Hide();
        
        
        ((Frame)(this.Parent)).IsVisible = false;
        ((Pages.Main.Downloads.DownloadGamePage)((Grid)(((Frame)(this.Parent)).Parent)).Parent).MainGrid.IsVisible = true;
    }

    private void ModLoaderExpander_OnClick(object? sender, RoutedEventArgs e)
    {
        // var con = new ChooseModLoader(VersionTitle.Content.ToString());
        //
        // ContentDialog contentDialog = new ContentDialog();
        // contentDialog.Title = "添加模组加载器";
        // contentDialog.PrimaryButtonText = "取消";
        // contentDialog.CloseButtonText = "确定";
        // contentDialog.Content = con;
        // contentDialog.ShowAsync(Core.MainWindow);
        // contentDialog.CloseButtonClick += (_, __) =>
        // {
        //     con.GetModLoader();
        // };
    }

    private async void InstallButton_OnClick(object? sender, RoutedEventArgs e)
    {
        GetModLoader();
        
        var dow = new DownloadGame.DownloadGame();
        dow.Version = VersionTitle.Content.ToString();
        dow.Tuid = SystemMessageTaskMange.AddTask(dow);
        dow.Modloaders = installedObjects;
        dow.StartDownloadAsync();
        
        
        // var con = (ContentDialog)this.Parent;
        // con.Hide();
        ((Frame)(this.Parent)).IsVisible = false;
        ((Pages.Main.Downloads.DownloadGamePage)((Grid)(((Frame)(this.Parent)).Parent)).Parent).MainGrid.IsVisible = true;
    }
    public void GetModLoader()
    {
        // 清空之前的安装对象
        installedObjects.Clear();

        // 处理每个盒子
        ProcessBox(ForgeBox.Children, forgebuild);
        ProcessBox(FabricBox.Children, fabricbuild);
        ProcessBox(OptiFineBox.Children, optifinebuild);
        ProcessBox(QuiltBox.Children, quiltbuild);
    }
    private void ProcessBox<T>(IReadOnlyList<Control> boxChildren, IAsyncEnumerable<T> buildList)
    {
        for (int i = 0; i < boxChildren.Count; i++)
        {
            var radioButton = boxChildren[i] as RadioButton;
            if (radioButton != null && radioButton.IsChecked == true)
            {
                string selectedContent = radioButton.Content.ToString();
                // 使用 LINQ 查询匹配的条目
                object selectedBuild = null;
                if (buildList is IAsyncEnumerable<ForgeInstallEntry> forgeEntries)
                {
                    selectedBuild = forgeEntries.FirstAsync(b => b.ForgeVersion == selectedContent);
                }
                else if (buildList is IAsyncEnumerable<FabricInstallEntry> fabricEntries)
                {
                    selectedBuild = fabricEntries.FirstAsync(b => b.BuildVersion == selectedContent);
                }
                else if (buildList is IAsyncEnumerable<OptifineInstallEntry> optiFineEntries)
                {
                    selectedBuild = optiFineEntries.FirstAsync(b => b.FileName == selectedContent+".jar");
                }
                else if (buildList is IAsyncEnumerable<QuiltInstallEntry> quiltEntries)
                {
                    selectedBuild = quiltEntries.FirstAsync(b => b.BuildVersion== selectedContent);
                }
                
                installedObjects.Add(selectedBuild);
            }
        }
    }
}