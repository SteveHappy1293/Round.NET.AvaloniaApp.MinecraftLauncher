using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia.Threading;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.EventArgs;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using MinecraftLaunch.Components.Downloader;
using MinecraftLaunch.Components.Installer;
using MinecraftLaunch.Utilities;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;

public class InstallGame
{
    public class InstallItemConfig
    {
        public string Name { get; set; }
        public InstallerBase Entry { get; set; }
    }
    public static async Task<IInstallEntry> DownloadGame(string Version)
    {
        var mc = await VanillaInstaller.EnumerableMinecraftAsync()
            .FirstAsync(x => x.McVersion.Equals(Version));

        return mc;
    }

    public static IInstallEntry InstallForge(string Version,ForgeInstallEntry forge, Action<object, InstallProgressChangedEventArgs> progressChanged)
    {
        var forgeInstaller = ForgeInstaller.Create(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
            FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath,
            forge);

        forgeInstaller.ProgressChanged += (_, __) => progressChanged(_, __);


        return forge;
    }
    public static IInstallEntry InstallFabric(string Version,FabricInstallEntry fabric, Action<object, InstallProgressChangedEventArgs> progressChanged)
    {
        var fabricInstaller = FabricInstaller.Create(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
            fabric);

        fabricInstaller.ProgressChanged += (_, __) => progressChanged(_, __);

        
        return fabric;
    }
    public static IInstallEntry InstallOptifine(string Version,OptifineInstallEntry Opti, Action<object, InstallProgressChangedEventArgs> progressChanged)
    {
        var optifineInstaller = OptifineInstaller.Create(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
            FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath,
            Opti);

        optifineInstaller.ProgressChanged += (_, __) => progressChanged(_, __);


        return Opti;
    }
    public static IInstallEntry InstallQuilt(string Version,QuiltInstallEntry Quilt, Action<object, InstallProgressChangedEventArgs> progressChanged)
    {
        var quiltInstaller = QuiltInstaller.Create(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
            Quilt);

        quiltInstaller.ProgressChanged += (_, __) => progressChanged(_, __);


        return Quilt;
    }

    public static bool InstallComposite(List<IInstallEntry> installs, string Name,Action<InstallProgressChangedEventArgs> progressChanged)
    {
        HttpUtil.Initialize();

        var installEntry = new List<IInstallEntry>();
        foreach (var install in installs)
        {
            installEntry.Add(install);
        }
        var installer5 = CompositeInstaller.Create(installEntry,
            Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
            FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath, Name);
        installer5.ProgressChanged += (_, arg) =>
        {
            progressChanged(arg);
            Console.WriteLine(
                $"{(arg.PrimaryStepName is InstallStep.Undefined ? "" : $"{arg.PrimaryStepName} - ")}{arg.StepName} - {arg.FinishedStepTaskCount}/{arg.TotalStepTaskCount} - {(arg.IsStepSupportSpeed ? $"{FileDownloader.GetSpeedText(arg.Speed)} - {arg.Progress * 100:0.00}%" : $"{arg.Progress * 100:0.00}%")}");

        };
            
        var minecraft5 = installer5.InstallAsync();
        Console.WriteLine(minecraft5.Id);
        return minecraft5 != null;
    }
}