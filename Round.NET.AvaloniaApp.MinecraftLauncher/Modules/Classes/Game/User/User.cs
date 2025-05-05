using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls.Shapes;
using HarfBuzzSharp;
using OverrideLauncher.Core.Modules.Classes.Account;
using OverrideLauncher.Core.Modules.Entry.AccountEntry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Account;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User;

public class User
{
    public class UserConfig
    {
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = string.Empty;
        public string Skin  { get; set; } = string.Empty;
        public string Head  { get; set; } = string.Empty;
        public string Body  { get; set; } = string.Empty;
        public AccountEntry Config { get; set; }
    }
    public static readonly string ConfigPath = Path.GetFullPath("../RMCL/RMCL.Config/User");
    public static List<UserConfig> Users = new();
    public static void LoadUser()
    {
        if (Directory.Exists(ConfigPath))
        {
            Users.Clear();
            foreach (var us in Directory.GetFiles(ConfigPath))
            {
                Users.Add(JsonSerializer.Deserialize<UserConfig>(File.ReadAllText(Path.Combine(ConfigPath, us))));
            }
        }
        else
        {
            InitUser();
        }
    }

    public static void InitUser()
    {
        Directory.CreateDirectory(ConfigPath);

        var offlineAccount = new OffineAuthenticator("Steve");

        var user = new UserConfig()
        {
            Type = "Offline",
            Config = new AccountEntry()
            {
                UserName = offlineAccount.Authenticator().UserName,
                UUID = offlineAccount.Authenticator().UUID.ToString(),
                Token = offlineAccount.Authenticator().Token
            }
        };
        var json = Regex.Unescape(JsonSerializer.Serialize(user, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\"));
        File.WriteAllText(Path.Combine(ConfigPath,$"{user.UUID}.json"), json);
        Users.Add(user);
    }
    public static AccountEntry GetAccount(string uuid)
    {
        foreach (var us in Users)
        {
            if (us.UUID == uuid)
            {
                if (us.Type == "Offline")
                {
                    var of = new OffineAuthenticator(us.Config.UserName);
                    return of.Authenticator();
                }
                else
                {
                    var mi = new AccountEntry()
                    {
                        UserName = us.Config.UserName,
                        UUID = us.Config.UUID,
                        Token = us.Config.Token,
                        AccountType = "msa"
                    };
                    return mi;
                }
            }
        }
        return null;
    }
    public static void AddAccount(AccountEntry account)
    {
        if (account.AccountType == "msa")
        {
            var ac = new UserConfig()
            {
                Type = "Microsoft",
                Config = account
            };
            var json = Regex.Unescape(JsonSerializer.Serialize(ac, new JsonSerializerOptions() { WriteIndented = true }));
            File.WriteAllText(Path.Combine(ConfigPath,$"{ac.UUID}.json"),json);
        }
        else
        {
            var of = new UserConfig()
            {
                Type = "Offline",
                Config = account
            };
            var json = Regex.Unescape(JsonSerializer.Serialize(of, new JsonSerializerOptions() { WriteIndented = true }));
            File.WriteAllText(Path.Combine(ConfigPath,$"{of.UUID}.json"),json);
        }
        LoadUser();
    }

    public static void SaveUsers()
    {
        Users.ForEach((config =>
        {
            var json = Regex.Unescape(JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true }));
            File.WriteAllText(Path.Combine(ConfigPath,$"{config.UUID}.json"),json);
        }));
    }
    public static UserConfig GetUser(string uuid)
    {
        return Users.Find((x)=>x.UUID == uuid);
    }
    public static void SetUser(string uuid,UserConfig user)
    {
        var index = Users.FindIndex((x) => x.UUID == uuid);
        Users[index] = user;
        SaveUsers();
    }
}