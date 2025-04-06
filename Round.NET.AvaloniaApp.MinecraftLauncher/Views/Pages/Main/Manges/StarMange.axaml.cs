using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Classes.Mange.StarMange;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Button;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges.StarManges;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;

public partial class StarMange : UserControl
{
    public StarMange()
    {
        InitializeComponent();
        this.Loaded += (sender, args) =>
        {
            Load();
        };
    }

    public void Load()
    {
        StarBox.Children.Clear();
        foreach (var star in StarGroup.StarGroups)
        {
            var btn = new StarGroupButton()
            {
                Title = star.GroupName,
                Count = star.Stars.Count,
                Image = new Bitmap(new MemoryStream(Convert.FromBase64String(star.ImageBase64String))),
                Margin = new Thickness(5),
                StarItems = star.Stars
            };
            btn.Click += (s, e) =>
            {
                var page = new StarGroupPage();
                page.GUID = star.GUID;
                ((MainView)Core.MainWindow.Content).CortentFrame.Content = page;
                ((MainView)Core.MainWindow.Content).CortentFrame.Opacity = 1;
                ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 0;
        
                Core.NavigationBar.Opacity = 0;
                page.Load();
            };
            StarBox.Children.Add(btn);
        }
    }
    private void NewStarGroup_OnClick(object? sender, RoutedEventArgs e)
    {
        var file = "";
        var name = "";
        var NameBox = new TextBox()
        {
            Width = 200,
            Watermark = "必填项..."
        };
        var ImagePathBox = new TextBox()
        {
            Width = 150,
            Watermark = "必填项..."
        };
        var ChoseImageBtn = new Button()
        {
            Content = "...",
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 40,
        };
        ChoseImageBtn.Click += async (_, __) =>
        {
            // 创建文件选择器
            var filePicker = Core.MainWindow.StorageProvider;
    
            // 设置文件类型过滤器，只显示图片文件
            var fileTypeFilter = new FilePickerFileType[]
            {
                new("Image Files")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" },
                    MimeTypes = new[] { "image/png", "image/jpeg", "image/bmp", "image/gif" }
                }
            };
    
            // 配置选择器选项
            var options = new FilePickerOpenOptions
            {
                Title = "选择图片文件",
                FileTypeFilter = fileTypeFilter,
                AllowMultiple = false // 只允许选择单个文件
            };
    
            // 打开文件选择对话框
            var files = await filePicker.OpenFilePickerAsync(options);
    
            // 返回第一个选择的文件路径（如果用户选择了文件）
            file = files.Count > 0 ? files[0].Path.AbsolutePath : null;
            ImagePathBox.Text = Path.GetFullPath(Uri.UnescapeDataString(file));
        };
        var con = new ContentDialog()
        {
            Title = "新建收藏夹",
            Content = new StackPanel()
            {
                Children =
                {
                    new DockPanel()
                    {
                        Children =
                        {
                            new Label(){Content = "收藏夹名称：",VerticalContentAlignment = VerticalAlignment.Center},
                            NameBox,
                        },
                        Margin = new Thickness(5),
                    },
                    new DockPanel()
                    {
                        Children =
                        {
                            new Label(){Content = "收藏夹封面：",VerticalContentAlignment = VerticalAlignment.Center},
                            ImagePathBox,
                            ChoseImageBtn
                        },
                        Margin = new Thickness(5),
                    }
                }
            },
            CloseButtonText = "确定",
            PrimaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Close
        };
        con.CloseButtonClick += (_, __) =>
        {
            name = NameBox.Text;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(Path.GetFullPath(Uri.UnescapeDataString(ImagePathBox.Text))))
            {
                StarGroup.RegisterStarGroup(new StarGroupEnrty()
                {
                    GroupName = name,
                    ImageBase64String = Convert.ToBase64String(File.ReadAllBytes(Path.GetFullPath(Uri.UnescapeDataString(ImagePathBox.Text)))),
                });
                StarGroup.SaveStars();
                Load();
            }
        };
        con.ShowAsync();
    }

    private void Refresh_OnClick(object? sender, RoutedEventArgs e)
    {
        Load();
    }
}