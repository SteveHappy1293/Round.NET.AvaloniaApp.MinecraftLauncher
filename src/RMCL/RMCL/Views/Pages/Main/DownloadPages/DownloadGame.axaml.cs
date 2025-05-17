using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
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
    private List<OverrideLauncher.Core.Modules.Entry.DownloadEntry.VersionManifestEntry.Version> Versions = new List<VersionManifestEntry.Version>();
    public DownloadGame()
    {
        InitializeComponent();

        Task.Run(Update);
    }
    public void Update()
    {
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

        Dispatcher.UIThread.Invoke(UpdateUI);
        IsEdit = true;
    }

    public void UpdateUI()
    {
        VersionsList.Items.Clear();
        var tag = ((ComboBoxItem)VersionType.SelectedItem).Tag.ToString() ?? "*";
        Versions.ForEach(x =>
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
            else if(x.Type==tag)
            { 
                VersionsList.Items.Add(it);
            }
        });
    }

    private void VersionType_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if(IsEdit) UpdateUI();
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var tag = ((ComboBoxItem)VersionType.SelectedItem)?.Tag.ToString() ?? "*"; // 获取当前选择的版本类型
        if (sender is TextBox textBox)
        {
            string searchText = textBox.Text.Trim().ToLower(); // 获取搜索框的内容并转为小写
            if (string.IsNullOrEmpty(searchText))
            {
                // 如果搜索框为空，根据当前选择的版本类型显示所有版本
                UpdateUI();
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

                // 更新 UI，显示过滤后的版本
                Dispatcher.UIThread.Invoke(() =>
                {
                    VersionsList.Items.Clear();
                    filteredVersions.ForEach(version =>
                    {
                        var it = new DownloadGameItem(version);
                        it.OnDownload = ((id) =>
                        {
                            new DownloadClient(version).ShowDialog(Core.MainWindow);
                        });
                        VersionsList.Items.Add(it);
                    });
                });
            }
        }
    }
}