using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Classes.Account;
using OverrideLauncher.Core.Modules.Classes.Launch.Client;
using OverrideLauncher.Core.Modules.Classes.Version;
using OverrideLauncher.Core.Modules.Entry.GameEntry;
using OverrideLauncher.Core.Modules.Entry.JavaEntry;
using OverrideLauncher.Core.Modules.Entry.LaunchEntry;
using OverrideLauncher.Core.Modules.Enum.Launch;
using RMCL.Controls.Item;
using RMCL.Models.Classes;
using RMCL.Views.Windows.Main.ManageWindows;

namespace RMCL.Views.Pages.Main.ManagePages;

public partial class ManageGame : UserControl
{
    public bool IsEdit { get; set; } = false;
    public List<VersionParse> Versions = new();
    public ManageGame()
    {
        InitializeComponent();
        
        Refuse();
    }

    public void UpdateUI(string searchText = null)
    {
        IsEdit = false;
        VersionsList.Items.Clear();
        VersionsList.IsVisible = false;
        LoadingBox.IsVisible = true;
        Versions.Clear();
        
        NullBox.IsVisible = false;
        Task.Run(() =>
        {
            var path = Path.GetFullPath(Path.Combine(
                Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path, "versions"));

            var vers = Directory.GetDirectories(path);
            vers.ToList().ForEach(x => Versions.Add(new VersionParse(new ClientInstancesInfo()
            {
                GameCatalog =
                    Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path,
                GameName = Path.GetFileName(x)
            })));

            // 如果有搜索文本，则进行过滤
            if (!string.IsNullOrEmpty(searchText))
            {
                Versions = Versions.Where(version => version.GameJson.Id.ToLower().Contains(searchText.ToLower())).ToList();
            }

            foreach (var ver in Versions)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var item = new ManagerGameItem(ver);
                    item.OnLaunch = parse =>
                    {
                        ClientRunner Runner = new ClientRunner(new ClientRunnerInfo()
                        {
                            GameInstances = parse,
                            JavaInfo = new JavaInfo()
                            {
                                JavaPath = @"D:\MCLDownload\ext\jre-v64-220420\jdk17\bin\java.exe",
                                Version = "17.0.2",
                                Is64Bit = true
                            },
                            Account = new OffineAuthenticator("RoundStudio").Authenticator(),
                            LauncherInfo = "RMCL",
                            LauncherVersion = "114",
                            WindowInfo = ClientWindowSizeEnum.Fullscreen
                        });
                        Runner.LogsOutput = (string logs) => { Console.WriteLine(logs); };
                        Runner.Start();
                    };
                    VersionsList.Items.Add(item);
                });
            }

            Dispatcher.UIThread.Invoke(() =>
            {
                VersionsList.SelectedIndex = Config.Config.MainConfig
                    .GameFolders[Config.Config.MainConfig.SelectedGameFolder]
                    .SelectedGameIndex;
                VersionsList.IsVisible = true;
                LoadingBox.IsVisible = false;
                if (Versions.Count == 0)
                {
                    NullBox.IsVisible = true;
                }
            });

            IsEdit = true;
        });
    }

    public void Refuse()
    {
        IsEdit = false;
        VersionChoseBox.Items.Clear();
        Config.Config.MainConfig.GameFolders.ForEach(x =>
        {
            VersionChoseBox.Items.Add(new ComboBoxItem() { Content = x.Name });
        });
        VersionChoseBox.SelectedIndex = Config.Config.MainConfig.SelectedGameFolder;

        IsEdit = true;
        UpdateUI(SearchBox.Text);
    }
    private void RefuseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Refuse();
    }

    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (IsEdit)
        {
            // 获取搜索框的内容
            var searchText = SearchBox.Text;
            UpdateUI(searchText);
        }
    }

    private void ManageTheGameCatalog_OnClick(object? sender, RoutedEventArgs e)
    {
        new ManageGameDirectory().ShowDialog(Core.MainWindow);
        Refuse();
    }

    private void VersionChoseBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.SelectedGameFolder = VersionChoseBox.SelectedIndex;
            Config.Config.SaveConfig();
            
            UpdateUI(SearchBox.Text);
        }
    }

    private void OpenNowGameRoot_OnClick(object? sender, RoutedEventArgs e)
    {
        SystemHelper.FileExplorer.OpenFolder(Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].Path);
    }

    private void VersionsList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (IsEdit)
        {
            Config.Config.MainConfig.GameFolders[Config.Config.MainConfig.SelectedGameFolder].SelectedGameIndex =
                VersionsList.SelectedIndex;
            
            Config.Config.SaveConfig();
        }
    }
}