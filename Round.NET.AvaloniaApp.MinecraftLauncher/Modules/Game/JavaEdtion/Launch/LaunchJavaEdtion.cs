using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using Flurl.Util;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Downloader;
using MinecraftLaunch.Components.Installer;
using MinecraftLaunch.Components.Parser;
using MinecraftLaunch.Launch;
using MinecraftLaunch.Utilities;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;

public class LaunchJavaEdtion
{
    public static async void LaunchGame(string VersionID,Action<Process> GetGameProcess,Action<object,LogReceivedEventArgs> LaunchingOutput,Action Exit,string Server = null)
    {
        var entry = VanillaInstaller.EnumerableMinecraftAsync()
            .FirstAsync(x => x.Id == VersionID);

        MinecraftParser minecraftParser =
            Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path;
        
        if (Config.Config.MainConfig.SetTheLanguageOnStartup) SetValueOnFirst.SetLanguage(VersionID);
        if (Config.Config.MainConfig.SetTheGammaOnStartup) SetValueOnFirst.SetGamma(VersionID);
        var account = User.User.GetAccount(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID);
        MinecraftRunner runner = new(new LaunchConfig {
            Account = User.User.GetAccount(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID),
            JavaPath = FindJava.JavasList[Config.Config.MainConfig.SelectedJava],
            MaxMemorySize = Config.Config.MainConfig.JavaUseMemory,
            MinMemorySize = 512,
            LauncherName = "\"RMCL 3.0\"",
        }, minecraftParser);
        var process = await runner.RunAsync(minecraftParser.GetMinecraft(VersionID));
        process.Started += (_, _) => GetGameProcess(process.Process);
        process.OutputLogReceived += (_, arg) => LaunchingOutput(process, arg);
        process.Exited += (_, _) => Exit();
    }

    public static async Task<string> GetLauncherCommand(string VersionID)
    {
        var entry = VanillaInstaller.EnumerableMinecraftAsync()
            .FirstAsync(x => x.Id == VersionID);

        MinecraftParser minecraftParser =
            Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path;
        
        if (Config.Config.MainConfig.SetTheLanguageOnStartup) SetValueOnFirst.SetLanguage(VersionID);
        if (Config.Config.MainConfig.SetTheGammaOnStartup) SetValueOnFirst.SetGamma(VersionID);
        var account = User.User.GetAccount(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID);
        MinecraftRunner runner = new(new LaunchConfig {
            Account = User.User.GetAccount(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID),
            JavaPath = FindJava.JavasList[Config.Config.MainConfig.SelectedJava],
            MaxMemorySize = Config.Config.MainConfig.JavaUseMemory,
            MinMemorySize = 512,
            LauncherName = "\"RMCL 3.0\"",
        }, minecraftParser);
        var process = await runner.RunAsync(minecraftParser.GetMinecraft(VersionID));
        process.Process.Kill(true);
        return $"@echo off\r\n\"{FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath.Replace("javaw.exe","java.exe")}\" {process.Process.StartInfo.Arguments}";
    }
    public static bool ResourceCompletion(string VersionID)
    {
        // IGameResolver gameResolver = new GameResolver(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);
        // IChecker resourceChecker = new ResourceChecker(gameResolver.GetGameEntity(VersionID));
        // var result = resourceChecker.CheckAsync().Result;

        // false = 不全，true = 全
        return true;
    }
}