using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using Flurl.Util;
using MinecraftLaunch.Utilities;
using OverrideLauncher.Core.Modules.Classes.Account;
using OverrideLauncher.Core.Modules.Classes.Launch;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.AccountEntry;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using OverrideLauncher.Core.Modules.Entry.JavaEntry;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Round.NET.VersionServerMange.Library.Modules;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;

public class LaunchJavaEdtion
{
    public static async void LaunchGame(string Dir,string VersionID,Action<string> LaunchingOutput,Action Exit,string Server = null)
    {
        //UpdateServers(VersionID);
        
        if (Config.Config.MainConfig.SetTheLanguageOnStartup) SetValueOnFirst.SetLanguage(VersionID);
        if (Config.Config.MainConfig.SetTheGammaOnStartup) SetValueOnFirst.SetGamma(VersionID);
        
        var ver = new VersionParse(new GameInstancesInfo()
        {
            GameCatalog = Dir,
            GameName = VersionID
        });

        LaunchRunner Runner = new LaunchRunner(new LaunchRunnerInfo()
        {
            GameInstances = ver,
            JavaInfo = new JavaInfo()
            {
                JavaPath = Java.FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath
            },
            Account = new AccountEntry()
            {
                AccountType = User.User.GetUser(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID).Type,
                UserName = User.User.GetUser(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID).Config
                    .UserName,
                UUID = User.User.GetUser(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID).Config.UUID
            },
            LauncherInfo = "RMCL",
            LauncherVersion = "114",
            JvmArgs = new List<string>() { Server }
        });
        Runner.LogsOutput = (string logs) => { LaunchingOutput(logs); };
        try
        {
            var process = Runner.GameProcess;
            process.Exited += (_, _) => Exit();
        }
        catch (Exception ex)
        {
            // 抛出包含堆栈信息的新异常
            throw ex;
        }
        Runner.Start();
    }
    public static async Task<string> GetLauncherCommand(string VersionID)
    {
        var dir =
            Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path;
        
        if (Config.Config.MainConfig.SetTheLanguageOnStartup) SetValueOnFirst.SetLanguage(VersionID);
        if (Config.Config.MainConfig.SetTheGammaOnStartup) SetValueOnFirst.SetGamma(VersionID);
        var account = User.User.GetAccount(User.User.Users[Config.Config.MainConfig.SelectedUser].UUID);
        
        
        var ver = new VersionParse(new GameInstancesInfo()
        {
            GameCatalog = dir,
            GameName = VersionID
        });
        
        LaunchRunner Runner = new LaunchRunner(new LaunchRunnerInfo()
        {
            GameInstances = ver,
            JavaInfo = new JavaInfo()
            {
                JavaPath = Java.FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath
            },
            Account = account,
            LauncherInfo = "RMCL",
            LauncherVersion = "114"
        });
        Runner.LogsOutput = (string logs) => {  };
        return $"@echo off\r\n\"{MinecraftLauncher.Modules.Java.FindJava.JavasList[Config.Config.MainConfig.SelectedJava].JavaPath.Replace("javaw.exe","java.exe")}\" {Runner.GameProcess.StartInfo.Arguments}";
    }
    public static bool ResourceCompletion(string VersionID)
    {
        // IGameResolver gameResolver = new GameResolver(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);
        // IChecker resourceChecker = new ResourceChecker(gameResolver.GetGameEntity(VersionID));
        // var result = resourceChecker.CheckAsync().Result;

        // false = 不全，true = 全
        return true;
    }
    /*public static void UpdateServers(string VersionID)
    {
        try
        {
            var path = Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path +
                       "/versions/" + VersionID + "/servers.dat";

            var se = new ServerMangeCore();
            se.Path = path;
            se.Load();
            se.Servers = ServerMange.Servers;
            se.Save();
        }
        catch (Exception ex)
        {
            Message.Message.Show("启动游戏","同步全局服务器配置出错",InfoBarSeverity.Error);
        }
    }
    */
}