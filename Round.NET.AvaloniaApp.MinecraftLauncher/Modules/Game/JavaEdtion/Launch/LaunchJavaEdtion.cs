using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Event;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Checker;
using MinecraftLaunch.Components.Launcher;
using MinecraftLaunch.Components.Resolver;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;

public class LaunchJavaEdtion
{
    public static void LaunchGame(string VersionID,Action<Process> GetGameProcess,Action<object,LogReceivedEventArgs> LaunchingOutput,Action Exit)
    {
        var account = new OfflineAuthenticator(Config.Config.MainConfig.Users[Config.Config.MainConfig.SelectedUser].UserName).Authenticate();
        LaunchConfig config = new LaunchConfig {
            Account = account,
            LauncherName = "RMCL 3.0",
            JvmConfig = new JvmConfig(Config.Config.MainConfig.Javas[Config.Config.MainConfig.SelectedJava].JavaPath) {
                MaxMemory = 1024
            },
    
            IsEnableIndependencyCore = true  //是否启用版本隔离
        };
        bool launched = false;
        IGameResolver gameResolver = new GameResolver(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);
        Launcher launcher = new(gameResolver, config); 
        Task.Run(async () => {
            var gameProcessWatcher = await launcher.LaunchAsync(VersionID);
            GetGameProcess(gameProcessWatcher.Process);
            gameProcessWatcher.OutputLogReceived += (sender, args) => {
                LaunchingOutput(sender,args);
            };
    
            gameProcessWatcher.Exited += (sender, args) => {
                Exit();
            };
        });
    }

    public static bool ResourceCompletion(string VersionID)
    {
        IGameResolver gameResolver = new GameResolver(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);
        IChecker resourceChecker = new ResourceChecker(gameResolver.GetGameEntity(VersionID));
        var result = resourceChecker.CheckAsync().Result;

        // false = 不全，true = 全
        return result;
    }
}