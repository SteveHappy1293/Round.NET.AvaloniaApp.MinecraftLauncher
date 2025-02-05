using System;
using Avalonia.Threading;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Classes.Models.Event;
using MinecraftLaunch.Components.Installer;
using MinecraftLaunch.Components.Resolver;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Models.Game.JavaEdtion.Install;

public class InstallGame
{
    public static bool DownloadGame(string Version,Action<object,ProgressChangedEventArgs> progressChanged)
    {
        GameResolver gameResolver = new(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);

        VanlliaInstaller vanillaInstaller = new(gameResolver, Version, DownloaderConfiguration.Default);

        vanillaInstaller.ProgressChanged += (_, __) => progressChanged(_, __);

        var result = vanillaInstaller.InstallAsync().Result;
        return result;
    }
}