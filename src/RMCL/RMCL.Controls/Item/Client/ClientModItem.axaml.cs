using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using RMCL.PathsDictionary;

namespace RMCL.Controls.Item.Client;

public partial class ClientModItem : UserControl
{
    public string path { get; set; }
    public ClientModItem()
    {
        InitializeComponent();
    }

    public bool IsEdit { get; set; } = false;

    public async Task LoadInfoAsync(string filename)
    {
        IsEdit = false;
        path = filename;
        await Task.Run(() =>
        {
            var res = LoadModInfo.LoadJarMod.LoadJarModInfo(filename);

            if (res != null)
            {
                Console.WriteLine(res.IconPath);
                Dispatcher.UIThread.Invoke(() =>
                {
                    AssetsName.Text = res.FileName;
                    JarFileName.Text = res.Name;
                    EnableTheDisablingSetting.IsChecked = res.Enabled;
                    IsEdit = true;
                    try
                    {
                        var imagePath = Path.GetFullPath(res.IconPath); // 获取绝对路径
                        if (File.Exists(imagePath))
                        {
                            using (var stream = File.OpenRead(imagePath))
                            {
                                AssetsIcon.Background = new ImageBrush
                                {
                                    Source = new Bitmap(stream),
                                    Stretch = Stretch.UniformToFill // 可选：控制图片拉伸方式
                                };
                                AssetsIcon.Child = null;
                            }
                        }
                        else
                        {
                            Console.WriteLine("文件不存在: " + imagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("加载图片失败: " + ex.Message);
                    }
                });
            }
            else
            {
                Console.WriteLine("无法加载");
            }
        });
    }

    private void EnableTheDisablingSetting_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (IsEdit)
        {
            var end = EnableTheDisablingSetting.IsChecked;
            if (path.EndsWith(PathDictionary.ClientModEnablePostfix))
            {
                File.Move(path,
                    path.Replace(PathDictionary.ClientModEnablePostfix, PathDictionary.ClientModDisablePostfix));
            }
            else
            {
                File.Move(path,
                    path.Replace(PathDictionary.ClientModDisablePostfix, PathDictionary.ClientModEnablePostfix));
            }
        }
    }
}