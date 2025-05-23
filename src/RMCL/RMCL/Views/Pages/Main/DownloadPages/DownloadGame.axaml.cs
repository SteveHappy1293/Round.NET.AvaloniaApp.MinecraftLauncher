using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry;
using RMCL.Controls.Item;
using RMCL.DownloadService;
using RMCL.Models.Classes;
using RMCL.Views.Windows.Main.DownloadWindows;

namespace RMCL.Views.Pages.Main.DownloadPages;

public partial class DownloadGame : UserControl
{
    private bool IsEdit = false;
    private bool IsLoad = false;
    private List<OverrideLauncher.Core.Modules.Entry.DownloadEntry.VersionManifestEntry.Version> Versions = new List<VersionManifestEntry.Version>();
    public DownloadGame()
    {
        InitializeComponent();

        Task.Run(Update);
    }
    public void Update()
    {
        IsEdit = false;
        Versions.Clear();
        Dispatcher.UIThread.Invoke(() =>
        {
            VersionType.IsEnabled = false;
            RefuseButton.IsEnabled = false;
            SearchBox.IsEnabled = false;
            LoadingBox.IsVisible = true;
            VersionsList.Items.Clear();
        });
        var resu = true;
        while (resu)
        {
            if (Versions.Count <= 0)
            {
                Versions =
                    UpdateMinecraftVersions.Load().ToList();
            }
            else
            {
                resu = false;
            }
        }
        Thread.Sleep(200); // 避免闪屏

        Dispatcher.UIThread.Invoke(UpdateUI);
        Dispatcher.UIThread.Invoke(() =>
        {
            VersionType.IsEnabled = true;
            RefuseButton.IsEnabled = true;
            SearchBox.IsEnabled = true;
            LoadingBox.IsVisible = false;
        });
        IsEdit = true;
        IsLoad = true;
    }

    public void UpdateUI()
    {
        var tag = ((ComboBoxItem)VersionType.SelectedItem)?.Tag.ToString() ?? "*"; // 获取当前选择的版本类型
        VersionsList.Items.Clear();
        VersionsList.IsVisible = false;
        string searchText = null;
        if (IsLoad) searchText = SearchBox.Text ?? null; // 获取搜索框的内容并转为小写
        NullBox.IsVisible = false;
        LoadingBox.IsVisible = true;
        Task.Run(() =>
        {
            if (string.IsNullOrEmpty(searchText))
            {
                Versions.ForEach(x =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        var it = new DownloadGameItem(x);
                        it.OnDownload = ((id) =>
                        {
                            new DownloadClient(x).ShowDialog(Core.MainWindow);
                        });


                        if (tag == "*")
                        {
                            VersionsList.Items.Add(it);
                        }
                        else if (x.Type == tag)
                        {
                            VersionsList.Items.Add(it);
                        }
                    });
                });
            }
            else
            {
                // 根据版本 ID 和当前选择的版本类型过滤版本列表
                var filteredVersions = Versions
                    .Where(version =>
                            version.Id.ToLower().Contains(searchText) && // 匹配版本 ID
                            (tag == "*" || version.Type == tag) // 匹配版本类型
                    )
                    .ToList();

                filteredVersions.ForEach(version =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        var it = new DownloadGameItem(version);
                        it.OnDownload = ((id) =>
                        {
                            new DownloadClient(version).ShowDialog(Core.MainWindow);
                        });
                        VersionsList.Items.Add(it);
                    });
                });
                // 更新 UI，显示过滤后的版本
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (filteredVersions.Count == 0)
                    {
                        NullBox.IsVisible = true;
                    }
                });
            }

            Dispatcher.UIThread.Invoke(() => LoadingBox.IsVisible = false);
            Dispatcher.UIThread.Invoke(() => VersionsList.IsVisible = true);
        });
    }

    private void VersionType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(IsEdit) UpdateUI();
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateUI();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Task.Run(Update);
    }
}