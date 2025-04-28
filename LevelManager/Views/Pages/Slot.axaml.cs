using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views;

namespace LevelManager.Views.Pages;

public partial class Slot : UserControl
{
    private int _count;
    private string _id;
    public int Count
    {
        get
        {
            return _count;
        }
        set
        {
            _count = value;
                count.Content = value;
        }
    }

    public string ItemID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value; 
           if (_id != null )
            {
                try
                {
                    var asset = AssetLoader.Open(
                        new Uri($"avares://LevelManager/items/{_id.Replace("minecraft:", "")}.png"));
                    var bitmap = new Bitmap(asset);
                    Image.Source = bitmap;
                }
                catch
                {
                    var asset = AssetLoader.Open(
                        new Uri($"avares://LevelManager/items/barrier.png"));
                    var bitmap = new Bitmap(asset);
                    Image.Source = bitmap;
                }
            }
        }
    }
    public Slot()
    {
        InitializeComponent();
    }
}