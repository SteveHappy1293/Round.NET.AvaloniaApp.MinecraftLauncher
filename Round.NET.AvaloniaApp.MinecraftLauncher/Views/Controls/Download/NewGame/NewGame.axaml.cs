using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Resolver;
using MinecraftLaunch.Components.Downloader;
using MinecraftLaunch.Components.Installer;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddModLoader;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;

public partial class NewGame : UserControl
{
    private IEnumerable<ForgeInstallEntry> forgebuild;
    private IEnumerable<OptiFineInstallEntity> optifinebuild;
    private IEnumerable<FabricBuildEntry> fabricbuild;
    private IEnumerable<QuiltBuildEntry> quiltbuild;
    public NewGame(string version)
    {
        InitializeComponent();
        ChooseVersionBox.Description = version;
        ShowNameBox.Text = version;
        VersionTitle.Content = version;
        Task.Run(async () =>
        { 
            forgebuild = (await ForgeInstaller.EnumerableFromVersionAsync(version));
            optifinebuild = (await OptifineInstaller.EnumerableFromVersionAsync(version));
            fabricbuild = (await FabricInstaller.EnumerableFromVersionAsync(version)); 
            quiltbuild = (await QuiltInstaller.EnumerableFromVersionAsync(version));
            foreach (var mod in forgebuild)
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
            
            foreach (var mod in optifinebuild)
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
            
            foreach (var mod in fabricbuild)
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
            
            foreach (var mod in quiltbuild)
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
        });
    }

    private void BackExpander_OnClick(object? sender, RoutedEventArgs e)
    {
        // var con = (ContentDialog)this.Parent;
        // con.Hide();
        
        
        ((Frame)(this.Parent)).IsVisible = false;
        ((Pages.Main.Download)((Grid)(((Frame)(this.Parent)).Parent)).Parent).MainGrid.IsVisible = true;
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
        
        var dow = new DownloadGame.DownloadGame();
        dow.Version = VersionTitle.Content.ToString();
        SystemMessageTaskMange.AddTask(dow,SystemMessageTaskMange.TaskType.Download);
        
        // var con = (ContentDialog)this.Parent;
        // con.Hide();
        ((Frame)(this.Parent)).IsVisible = false;
        ((Pages.Main.Download)((Grid)(((Frame)(this.Parent)).Parent)).Parent).MainGrid.IsVisible = true;
    }
    public void GetModLoader()
    {
        string forge = string.Empty;
        string optifine = string.Empty;
        string fabric = string.Empty;
        string quilt = string.Empty;

        foreach (var forgete in ForgeBox.Children)
        {
            var radioButton = forgete as RadioButton;
            if (radioButton.IsChecked == true)
            {
                forge = radioButton.Content.ToString();
            }
        }
        foreach (var fabricte in FabricBox.Children)
        {
            var radioButton = fabricte as RadioButton;
            if (radioButton.IsChecked == true)
            {
                fabric = radioButton.Content.ToString();
            }
        }
        foreach (var optifinete in OptiFineBox.Children)
        {
            var radioButton = optifinete as RadioButton;
            if (radioButton.IsChecked == true)
            {
                optifine = radioButton.Content.ToString();
            }
        }
        foreach (var quiltte in QuiltBox.Children)
        {
            var radioButton = quiltte as RadioButton;
            if (radioButton.IsChecked == true)
            {
                quilt = radioButton.Content.ToString();
            }
        }
    }
}