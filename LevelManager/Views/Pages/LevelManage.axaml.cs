using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using DynamicData;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using fNbt;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;

namespace LevelManager.Views.Pages;

public partial class LevelManage : UserControl
{
    public LevelManage()
    {
        InitializeComponent();
        var all = new List<string>();
        foreach (var item in Config.MainConfig.GameFolders)
        {
            if (Directory.Exists($"{item.Path}\\versions"))
            {
                foreach (var VARIABLE in Directory.GetDirectories($"{item.Path}\\versions"))
                {
                    if (Directory.Exists($"{VARIABLE}\\saves"))
                    {
                        Box.Items.Add(new ComboBoxItem
                        {
                            Content = $"{new FileInfo(VARIABLE).Name}（{item.Name}）",
                            Tag = new string[] { $"{VARIABLE}\\saves" }
                        });
                        all.Add($"{VARIABLE}\\saves");
                    }
                }
            }
        } 
        
        Box.Items.Insert(0,new ComboBoxItem { Content = "所有已知的游戏存档" ,Tag = all.ToArray()});
        Box.SelectedIndex = 0;
    }

    private void UpdateSavesList(string[] paths)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            list.Items.Clear();
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {

                    foreach (var ver in Directory.GetDirectories(path))
                    {
                        var nbt = new NbtFile();
                        nbt.LoadFromFile($"{ver}\\level.dat");
                        NbtCompound root = nbt.RootTag;
                        NbtCompound data = root.Get<NbtCompound>("Data");
                        var versionName = data.Get<NbtString>("LevelName").Value;

                        var delete = new Button()
                        {
                            Content = new FluentIcon()
                            {
                                Icon = FluentIconSymbol.Delete20Regular,
                                Margin = new Thickness(-10),
                            },
                            Margin = new Thickness(5),
                            Height = 32,
                            Width = 32
                        };
                        delete.Click += (_, __) =>
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(ver, UIOption.AllDialogs,
                                RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
                            UpdateSavesList(paths);
                        };
                        var launch = new Button()
                        {
                            Content = $"启动游戏",
                            Margin = new Thickness(5),
                            Height = 32,
                            Width = 150
                        };
                        launch.Click += (_, __) =>
                        {
                            var dow = new LaunchJavaEdtion();
                            dow.Dir = Path.Combine(path, "..","..","..");
                            dow.Version = new DirectoryInfo(Path.Combine(path,"..")).Name;
                            dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                            dow.Launch();
                        };
                        var sett = new Button()
                        {
                            Content = new FluentIcon()
                            {
                                Icon = FluentIconSymbol.Settings20Regular,
                                Margin = new Thickness(-10),
                            },
                            Margin = new Thickness(5),
                            Height = 32,
                            Width = 32
                        };
                        sett.Click += (_, __) =>
                        {
                            var con = new ContentPageDialog()
                            {
                                Page = new LevelSetting { nbtfilepath = ver + "\\level.dat",nbt = nbt},
                                Title = $"存档设置 - {versionName}",
                            };
                            con.Show();
                        };
                        var title = new Label()
                        {
                            Content = versionName,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            VerticalContentAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(5),
                            FontSize = 20,
                        };
                        list.Items.Add(new ListBoxItem()
                        {
                            Content = new Grid()
                            {
                                Height = 65,
                                Children =
                                {
                                    new DockPanel()
                                    {
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        Children =
                                        {
                                            new Image()
                                            {
                                                Height = 48,
                                                Width = File.Exists($"{ver}\\icon.png")?48 : 0,
                                                Source = File.Exists($"{ver}\\icon.png")?new  Bitmap($"{ver}\\icon.png") : null,
                                                VerticalAlignment = VerticalAlignment.Center
                                            },
                                            new Grid()
                                            {
                                                Children =
                                                {
                                                    title,
                                                    new Label()
                                                    {
                                                        Content = $"创建时间：{new DirectoryInfo(ver).CreationTime}，难度：{data.Get<NbtInt>("GameType").Value}",
                                                        HorizontalContentAlignment = HorizontalAlignment.Left,
                                                        VerticalContentAlignment = VerticalAlignment.Bottom,
                                                        Margin = new Thickness(5),
                                                        FontSize = 15,
                                                        Foreground = Brushes.LightGray,
                                                    },
                                                }
                                            }
                                        }
                                    },

                                    new DockPanel()
                                    {
                                        HorizontalAlignment = HorizontalAlignment.Right,
                                        VerticalAlignment = VerticalAlignment.Center,
                                        Children =
                                        {
                                            launch,
                                            sett,
                                            delete
                                        }
                                    }
                                }
                            },
                        });
                    }

                    if (list.Items.Count > 0)
                    {
                        nothingtext.Opacity = 0;
                    }
                    else
                    {
                        nothingtext.Opacity = 1;
                    }
                }
            }
        });
    }

    private void Box_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateSavesList((Box.SelectedItem as ComboBoxItem).Tag as string[]);
    }
}