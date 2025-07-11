using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using RMCL.AssetsPool;
using RMCL.Base.Interface;
using RMCL.Controls.View;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientScreenshotSetting : ISetting ,IUISetting
{
    public async void UpdateUI()
    {
        try
        {
            var path = System.IO.Path.Combine(Path, PathsDictionary.PathDictionary.ClientScreenshotsFolderName);
            if (!Directory.Exists(path)) return;

            var files = Directory.EnumerateFiles(path)
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var bitmap = await Task.Run(() => Models.Classes.Core.ImageResourcePool.GetImage(file,210));
                
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var red = new ImageReader { ImageSource = bitmap };
                        red.OnOpen += (sender, args) => SystemHelper.SystemHelper.OpenFile(file); 
                        StackPanel.Children.Add(red);
                    });
                }
                catch (Exception ex)
                {
                    // 记录错误或忽略
                }
            }
        }
        catch (Exception ex)
        {
            // 处理目录访问错误
        }
    }

    public string Path { get; set; }
    public ClientScreenshotSetting()
    {
        InitializeComponent();
    }
}