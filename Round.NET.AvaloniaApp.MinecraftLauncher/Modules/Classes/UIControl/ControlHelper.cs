using System;
using Avalonia.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class ControlHelper
{
    public static MenuItem CreateMenuItem(string menuItemName,Action OnClick)
    {
        var item = new MenuItem();
        item.Header = menuItemName;
        item.Click += (sender, args) => { OnClick?.Invoke(); };
        return item;
    }
}