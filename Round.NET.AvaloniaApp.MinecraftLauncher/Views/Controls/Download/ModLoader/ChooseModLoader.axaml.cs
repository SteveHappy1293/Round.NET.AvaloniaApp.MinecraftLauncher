using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddModLoader;

public partial class ChooseModLoader : UserControl
{
    private IEnumerable<ForgeInstallEntry> forgebuild;
    private IEnumerable<OptiFineInstallEntity> optifinebuild;
    private IEnumerable<FabricBuildEntry> fabricbuild;
    private IEnumerable<QuiltBuildEntry> quiltbuild;
    public ChooseModLoader()
    {
        InitializeComponent();
        var version = "1.12.2";

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

        if (forge != string.Empty)
        {
            
        }
    }
}