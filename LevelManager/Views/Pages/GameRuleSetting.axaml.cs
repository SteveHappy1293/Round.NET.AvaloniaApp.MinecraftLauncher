using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using fNbt;
using Newtonsoft.Json;

using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;
using Config = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config.Config;
using LaunchJavaEdtion = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch.LaunchJavaEdtion;

namespace LevelManager.Views.Pages;

public partial class GameRuleSetting : UserControl,IPage
{
    public void Open()
    {
        
    }
    private NbtFile _nbt;
    public GameRuleSetting(string filepath,NbtFile nbt)
    {
        InitializeComponent();
        _nbt = nbt;
        NbtCompound root = nbt.RootTag;
        NbtCompound data = root.Get<NbtCompound>("Data");
        foreach (NbtString VARIABLE in  data.Get<NbtCompound>("GameRules"))
        {
            list.Items.Add($"{VARIABLE.Name}: {VARIABLE.Value}");
        }
    }
}