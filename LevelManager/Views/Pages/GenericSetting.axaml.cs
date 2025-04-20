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
using Config = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config.Config;
using LaunchJavaEdtion = Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch.LaunchJavaEdtion;

namespace LevelManager.Views.Pages;

public partial class GenericSetting : UserControl
{
    private NbtFile _nbt;
    public GenericSetting(string filepath,NbtFile nbt)
    {
        InitializeComponent();
        _nbt = nbt;
        NbtCompound root = nbt.RootTag;
        NbtCompound data = root.Get<NbtCompound>("Data");
        hardcore.IsChecked = data.Get<NbtByte>("hardcore").Value == 1;
        lockdiff.IsChecked = data.Get<NbtByte>("DifficultyLocked").Value == 1;
        diff.SelectedIndex = data.Get<NbtByte>("Difficulty").Value;
        time.Text = data.Get<NbtLong>("Time").Value.ToString();
        rain.IsChecked = data.Get<NbtByte>("raining").Value == 1;
        thunder.IsChecked = data.Get<NbtByte>("thundering").Value == 1;
    }
}