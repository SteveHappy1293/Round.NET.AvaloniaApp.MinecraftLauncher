using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using OverrideLauncher.Core.Modules.Classes.Download.Assets.CurseForge;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;
using RMCL.Controls.Item;
using RMCL.Controls.View;
using RMCL.Models.Classes;
using RMCL.Views.Pages.Main.DownloadPages.DownloadAssets;

namespace RMCL.Views.Pages.Main.DownloadPages;

public partial class DownloadCurseForgeAssets : UserControl
{
    public CurseForgeSearchResponse NowResult { get; set; }
    public int PagesCount { get; set; }
    public int ShowedPagesCount { get; set; }
    public Button LoadMoreBtn { get; set; }
    public TextBlock CountShowBox { get; set; }

    private CurseForgeAssetsItem GetItem(ModInfo info)
    {
        var it = new CurseForgeAssetsItem(info);
        it.BtnOnClick = modInfo => Core.ChildFrame.Show(new DownloadAssetsDetailsPage(info));
        return it;
    }
    public DownloadCurseForgeAssets()
    {
        InitializeComponent();
        
        LoadingBox.IsVisible = true;
        AssetsList.Children.Clear();
        AssetsList.IsVisible = false;
        NullBox.IsVisible = false;
        SearchConfigBox.IsEnabled = false;
        Task.Run(() =>
        {
            try
            {
                var res = CurseForgeSearch.GetFeatured("$2a$10$Awb53b9gSOIJJkdV3Zrgp.CyFP.dI13QKbWn/4UZI4G4ff18WneB6")
                    .Result;

                res.Data.Featured.ForEach(x =>
                {
                    Dispatcher.UIThread.Invoke(() => { AssetsList.Children.Add(GetItem(x)); });
                });

                Dispatcher.UIThread.Invoke(() =>
                {
                    SearchConfigBox.IsEnabled = true;
                    AssetsList.IsVisible = true;
                    LoadingBox.IsVisible = false;
                    NullBox.IsVisible = false;
                });
            }
            catch
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    Core.MessageShowBox.AddInfoBar("无法搜索", "无法搜索 CurseForge 资源，可能您未连接互联网", InfoBarSeverity.Error);
                    SearchConfigBox.IsEnabled = true;
                    AssetsList.IsVisible = false;
                    LoadingBox.IsVisible = false;
                    NullBox.IsVisible = true;
                });
            }
        });
    }

    public void LoadMore(Loading load)
    {
        SearchConfigBox.IsEnabled = false;
        AssetsList.Children.Remove(CountShowBox);
        var name = SearchBox.Text;
        var ClassID = AssetsType.SelectedIndex switch
        {
            1 => CurseForgeSearchClassID.Mod,
            2 => CurseForgeSearchClassID.ResourcePacks,
            3 => CurseForgeSearchClassID.Modpacks,
            4 => CurseForgeSearchClassID.LightAndShadowPacks,
            _ => 0
        };

        Task.Run(() =>
        {
            var res = CurseForgeSearch.Search(new CurseForgeSearchInfo()
            {
                ApiKey = "$2a$10$Awb53b9gSOIJJkdV3Zrgp.CyFP.dI13QKbWn/4UZI4G4ff18WneB6",
                SearchName = name,
                Index = ShowedPagesCount + 1,
                PageSize = 50,
                ClassID = ClassID
            }).Result;
            
            res.Data.ForEach(x =>
            {
                NowResult.Data.Add(x);
                Dispatcher.UIThread.Invoke(() => AssetsList.Children.Add(GetItem(x)));
                ShowedPagesCount++;
                NowResult.Pagination.ResultCount ++;
            });

            Dispatcher.UIThread.Invoke(() =>
            {
                CountShowBox = new TextBlock()
                {
                    Text = $"共  {NowResult.Pagination.TotalCount} 条结果，已显示 {NowResult.Pagination.ResultCount} 条结果",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.Gray
                };
                AssetsList.Children.Add(CountShowBox);
                
                AssetsList.Children.Remove(load);
                if (NowResult.Pagination.ResultCount < NowResult.Pagination.TotalCount)
                {
                    AddLoadMoreBtn();
                }
                SearchConfigBox.IsEnabled = true;
            });
        });
    }
    private void SearchBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (AssetsList.Children.Contains(CountShowBox))
        {
            AssetsList.Children.Remove(CountShowBox);
        }
        var name = SearchBox.Text;
        LoadingBox.IsVisible = true;
        AssetsList.Children.Clear();
        AssetsList.IsVisible = false;
        NullBox.IsVisible = false;
        SearchConfigBox.IsEnabled = false;

        Task.Run(() =>
        {
            try
            {
                var ClassID = AssetsType.SelectedIndex switch
                {
                    1 => CurseForgeSearchClassID.Mod,
                    2 => CurseForgeSearchClassID.ResourcePacks,
                    3 => CurseForgeSearchClassID.Modpacks,
                    4 => CurseForgeSearchClassID.LightAndShadowPacks,
                    _ => 0
                };
                

                NowResult = CurseForgeSearch.Search(new CurseForgeSearchInfo()
                {
                    ApiKey = "$2a$10$Awb53b9gSOIJJkdV3Zrgp.CyFP.dI13QKbWn/4UZI4G4ff18WneB6",
                    SearchName = name,
                    Index = 0,
                    PageSize = 50,
                    ClassID = ClassID
                }).Result;

                Dispatcher.UIThread.Invoke(() =>
                {
                    NowResult.Data.ForEach(x => { AssetsList.Children.Add(GetItem(x)); });
                    CountShowBox = new TextBlock()
                    {
                        Text = $"共  {NowResult.Pagination.TotalCount} 条结果，已显示 {NowResult.Pagination.ResultCount} 条结果",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.Gray
                    };
                    AssetsList.Children.Add(CountShowBox);

                    if (NowResult.Pagination.TotalCount > 50)
                    {
                        PagesCount = NowResult.Pagination.TotalCount / 50;

                        AddLoadMoreBtn();
                    }

                    ShowedPagesCount = 0;

                    LoadingBox.IsVisible = false;
                    AssetsList.IsVisible = true;
                    SearchConfigBox.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Dispatcher.UIThread.Invoke(() =>
                {
                    LoadingBox.IsVisible = false;
                    NullBox.IsVisible = true;
                    SearchConfigBox.IsEnabled = true;
                });
            }
        });
    }

    public void AddLoadMoreBtn()
    {
        LoadMoreBtn = new Button()
        {
            Content = "加载更多",
            Margin = new Thickness(8),
            Classes = { "accent" },
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        LoadMoreBtn.Click += (sender, e) =>
        {
            var load = new Loading()
            {
                SmallTitle = "翻箱倒柜",
                BigTitle = "正在翻箱倒柜..."
            };
            AssetsList.Children.Add(load);
            AssetsList.Children.Remove(LoadMoreBtn);
            LoadMore(load);
        };
        AssetsList.Children.Add(LoadMoreBtn);
    }
}