using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using RMCLPlugToolsEntry;
using Path = System.Windows.Shapes.Path;

namespace RMCLPlugTools;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    public PlugConfigEntry Config { get; set; } = new ();
    public bool IsEdit { get; set; } = false;
    public MainWindow()
    {
        InitializeComponent();
        TitleBarHeight = 32;
        IsEdit = true;
        
        string[] args = Environment.GetCommandLineArgs();

        // 注意：第一个参数(args[0])是程序本身的路径
        var isnowindow = args.Contains("-w");
        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-c":
                    var conpath = args[i+1];
                    string selectedFilePath = conpath.Replace("\"", "");
                    Config = JsonSerializer.Deserialize<PlugConfigEntry>(File.ReadAllText(selectedFilePath));

                    LoadConfig();
                    break;
                case "-o":
                    var outpath = args[i+1];
                    Classes.Packing.GoPacking(Config);
                    File.Copy("RMCL.Packing\\Plug.rplk", outpath, true);
                    if(!isnowindow) MessageBox.Show("文件打包成功！","调试插件",MessageBoxButton.OK,MessageBoxImage.Information);
                    Close();
                    break;
            }
        }
    }

    private void ConfigChanged(object sender, TextChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.PlugIcon = ChoseIconPath.Text;
            Config.PlugName = PlugName.Text;
            Config.PlugVersion = PlugVersion.Text;
            Config.PlugMainDll = ChoseMainPlugPath.Text;
            Config.PlugWriter = PlugWriter.Text;
            Config.PlugMainDir = ChoseMainDirPath.Text;
        }
    }

    private async void Packing_OnClick(object sender, RoutedEventArgs e)
    {
        // 2. 选择保存位置
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Title = "导出 RMCL 插件包",
            Filter = "RMCL 插件包 (*.rplk)|*.rplk",
            DefaultExt = ".rplk",
            FileName = $"{Config.PlugName}.rplk"
        };
    
        if (saveFileDialog.ShowDialog() != true)
        {
            return;
        }
    
        string zipPath = saveFileDialog.FileName;
        var controller = await this.ShowProgressAsync("打包中", "正在创建插件包文件...", true);
        controller.SetIndeterminate();
    
        try
        {
            await Task.Run(() => 
            {
                Classes.Packing.GoPacking(Config);
                File.Copy("RMCL.Packing\\Plug.rplk", zipPath, true);
            });
        
            await controller.CloseAsync();
            await this.ShowMessageAsync("完成", $"插件包已成功导出到: {zipPath}");
        }
        catch (Exception ex)
        {
            await controller.CloseAsync();
            await this.ShowMessageAsync("错误", $"导出错误: {ex.Message}");
        }
    }

    private void SaveThePlugInConfiguration_OnClick(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
    
        // 设置对话框属性
        saveFileDialog.Title = "保存文件";
        saveFileDialog.Filter = "json 文件 (*.json)|*.json";
        saveFileDialog.FilterIndex = 1;
        saveFileDialog.DefaultExt = ".json"; // 默认扩展名
        saveFileDialog.AddExtension = true; // 自动添加扩展名
    
        saveFileDialog.FileName = $"RMCL3 插件配置文件 - {Config.PlugName} - " + DateTime.Now.ToString("yyyyMMdd");
    
        if (saveFileDialog.ShowDialog() == true)
        {
            string saveFilePath = saveFileDialog.FileName;
            string jsresult = Regex.Unescape(JsonSerializer.Serialize(Config, new JsonSerializerOptions() { WriteIndented = true }).Replace("\\","\\\\")); //获取结果并转换成正确的格式
            File.WriteAllText(saveFilePath,jsresult);
        }
    }

    private void ImportThePlugInConfiguration_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
    
        openFileDialog.Title = "选择 RMCL3 插件包配置文件";
        openFileDialog.Filter = "JSON (*.json)|*.json";
        openFileDialog.FilterIndex = 2;
        openFileDialog.Multiselect = false;
    
        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            Config = JsonSerializer.Deserialize<PlugConfigEntry>(File.ReadAllText(selectedFilePath));

            LoadConfig();
        }
    }

    public void LoadConfig()
    {
        IsEdit = false;
        ChoseIconPath.Text = Config.PlugIcon;
        PlugName.Text = Config.PlugName;
        PlugVersion.Text = Config.PlugVersion;
        ChoseMainPlugPath.Text = Config.PlugMainDll;
        PlugWriter.Text = Config.PlugWriter;
        ChoseMainDirPath.Text = Config.PlugMainDir;
        IsEdit = true;
    }

    private void ChoseIcon_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
    
        openFileDialog.Title = "选择图标文件";
        openFileDialog.Filter = "png (*.png)|*.png|jpg (*.jpg)|*.jpg|ico (*.ico)|*.ico";
        openFileDialog.FilterIndex = 2;
        openFileDialog.Multiselect = false;
    
        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            Config.PlugIcon = selectedFilePath;

            LoadConfig();
        }
    }

    private void ChoseMainPlug_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
    
        openFileDialog.Title = "选择插件本体文件（dll）";
        openFileDialog.Filter = "dll (*.dll)|*.dll";
        openFileDialog.FilterIndex = 2;
        openFileDialog.Multiselect = false;
    
        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            Config.PlugMainDll = selectedFilePath;

            LoadConfig();
        }
    }

    private void ChoseMainDir_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true; // 设置为文件夹选择模式
        dialog.Title = "选择文件夹";
    
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            string selectedFolder = dialog.FileName;
            Config.PlugMainDir = selectedFolder;

            LoadConfig();
        }
    }
}