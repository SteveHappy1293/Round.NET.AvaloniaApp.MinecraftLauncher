using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Flurl.Util;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Setting : UserControl
{
    public Setting()
    {
        InitializeComponent();
        Task.Run(() => {
            // 更新 UI
            Dispatcher.UIThread.Invoke(() =>
            {
                foreach (var java in FindJava.JavasList)
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