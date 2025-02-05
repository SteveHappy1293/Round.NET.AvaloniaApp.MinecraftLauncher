using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Flurl.Util;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Models.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Setting : UserControl
{
    public Setting()
    {
        InitializeComponent();
        Task.Run(() => {
            // 等待 FindJava 完成
            while (!FindJava.IsFinish) { }
            // 更新 UI
            Dispatcher.UIThread.Invoke(() =>
            {
                foreach (var java in Config.MainConfig.Javas)
                {
                    JavaComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = $"[Java {java.JavaSlugVersion}] {java.JavaPath}"
                    });
                }
                JavaComboBox.SelectedIndex = Config.MainConfig.SelectedJava;
            });  
        }); // 更新Java设置下拉框
    }

    private void JavaComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) //Java选择下拉框选择回调
    {
        Config.MainConfig.SelectedJava = JavaComboBox.SelectedIndex;
        Config.SaveConfig();
    }
}