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

public partial class PlayerSetting : UserControl
{
    private NbtFile _nbt;
    public PlayerSetting(string filepath,NbtFile nbt)
    {
        InitializeComponent();
        try
        {
            _nbt = nbt;
            NbtCompound root = nbt.RootTag;
            NbtCompound data = root.Get<NbtCompound>("Data");
            var player = data.Get<NbtCompound>("Player");
            if (player != null)
            {
                uuid.Text = ConvertToUuid(player.Get<NbtIntArray>("UUID").Value);
                foreach (NbtCompound item in (player.Get<NbtList>("Inventory")))
                {
                     LoadItem(item);
                }
            }

            else
            {
                this.Content = "无法获取玩家信息";
            }
        }
        catch (Exception e)
        {
            new Window{Content = e.ToString()}.Show();
        }
    }
    public List<int> SlotsUsed = new List<int>();

    public void LoadItem(NbtCompound item)
    {
        int slotNumber = item.Get<NbtByte>("Slot").Value;
        if (SlotsUsed.Contains(slotNumber))
            return;
        var slotMap = new Dictionary<int, Slot>
        {
            { 0, s1 }, { 1, s2 }, { 2, s3 }, { 3, s4 }, { 4, s5 },
            { 5, s6 }, { 6, s7 }, { 7, s8 }, { 8, s9 }, { 9, s11 },
            { 10, s12 }, { 11, s13 }, { 12, s14 }, { 13, s15 }, { 14, s16 },
            { 15, s17 }, { 16, s18 }, { 17, s19 }, { 18, s21 }, { 19, s22 },
            { 20, s23 }, { 21, s24 }, { 22, s25 }, { 23, s26 }, { 24, s27 },
            { 25, s28 }, { 26, s29 }, { 27, s31 }, { 28, s32 }, { 29, s33 },
            { 30, s34 }, { 31, s35 }, { 32, s36 }, { 33, s37 }, { 34, s38 },
            { 35, s39 }, { 100, boots }, { 101, leggings }, { 102, body }, { 103, head },{-106,hand}
        };
        if (slotMap.TryGetValue(slotNumber, out var slot))
        {
            SlotsUsed.Add(slotNumber);
            slot.ItemID = item.Get<NbtString>("id").Value;
            slot.Count = item.Get<NbtByte>("Count").Value;
        }
    }

    public static string ConvertToUuid(int[] numbers)
    {
        string hexString = "";
        foreach (int num in numbers)
        {
            string hex = num.ToString("x8");
            hexString += hex;
        }
        
        return $"{hexString.Substring(0, 8)}-{hexString.Substring(8, 4)}-{hexString.Substring(12, 4)}-" +
               $"{hexString.Substring(16, 4)}-{hexString.Substring(20, 12)}";
    }
}