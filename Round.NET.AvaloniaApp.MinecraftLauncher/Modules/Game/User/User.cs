using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Controls.Shapes;
using HarfBuzzSharp;
using MinecraftLaunch.Classes.Enums;
using MinecraftLaunch.Classes.Models.Auth;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.User;

public class User
{
    public class UserConfig
    {
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = string.Empty;
        public AccountConfig Config { get; set; }
    }
    public class AccountConfig
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UUID { get; set; }
        public DateTime RefreshTime { get; set; }
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
        
        var offlineAccount = new OfflineAccount();
        offlineAccount.Name = "Steve";

        var user = new UserConfig()
        {
            Type = "Offline",
            Config = new AccountConfig()
            {
                Username = offlineAccount.Name,
                UUID = offlineAccount.Uuid.ToString(),
                AccessToken = offlineAccount.AccessToken
            }
        };
        var json = Regex.Unescape(JsonSerializer.Serialize(user, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\"));
        File.WriteAllText(Path.Combine(ConfigPath,$"{user.UUID}.json"), json);
        Users.Add(user);
    }
    public static Account GetAccount(string uuid)
    {
        foreach (var us in Users)
        {
            if (us.UUID == uuid)
            {
                if (us.Type == "Offline")
                {
                    var of = new OfflineAccount();
                    of.Name = us.Config.Username;
                    of.Uuid = Guid.Parse(us.Config.UUID);
                    return of;
                }
                else
                {
                    var mi = new MicrosoftAccount();
                    mi.Name = us.Config.RefreshToken;
                    mi.AccessToken = us.Config.AccessToken;
                    mi.Name = us.Config.Username;
                    mi.LastRefreshTime = us.Config.RefreshTime;
                    mi.Uuid = Guid.Parse(us.Config.UUID);
                    return mi;
                }
            }
        }
        return null;
    }
    public static void AddAccount(Account account)
    {
        if (account.Type == AccountType.Microsoft)
        {
            var ac = new UserConfig()
            {
                Type = "Microsoft",
                Config = new AccountConfig()
                {
                    AccessToken = account.AccessToken,
                    RefreshTime = ((MicrosoftAccount)account).LastRefreshTime,
                    RefreshToken = ((MicrosoftAccount)account).RefreshToken,
                    Username = ((MicrosoftAccount)account).Name,
                    UUID = ((MicrosoftAccount)account).Uuid.ToString()
                }
            };
            var json = Regex.Unescape(JsonSerializer.Serialize(ac, new JsonSerializerOptions() { WriteIndented = true }));
            File.WriteAllText(Path.Combine(ConfigPath,$"{ac.UUID}.json"),json);
        }
        else
        {
            var of = new UserConfig()
            {
                Type = "Offline",
                Config = new AccountConfig()
                {
                    AccessToken = account.AccessToken,
                    Username = ((OfflineAccount)account).Name,
                    UUID = Guid.NewGuid().ToString()
                }
            };
            var json = Regex.Unescape(JsonSerializer.Serialize(of, new JsonSerializerOptions() { WriteIndented = true }));
            File.WriteAllText(Path.Combine(ConfigPath,$"{of.UUID}.json"),json);
        }
        LoadUser();
    }
}