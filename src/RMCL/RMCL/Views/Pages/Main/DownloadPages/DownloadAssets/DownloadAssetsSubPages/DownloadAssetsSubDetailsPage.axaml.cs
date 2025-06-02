using System;
using Avalonia;
using Avalonia.Controls;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;
using RMCL.Controls.View;

namespace RMCL.Views.Pages.Main.DownloadPages.DownloadAssets.DownloadAssetsSubPages;

public partial class DownloadAssetsSubDetailsPage : UserControl
{
    public DownloadAssetsSubDetailsPage()
    {
        InitializeComponent();
    }

    public void Update(ModInfo _modInfo)
    {
        _modInfo.Screenshots.ForEach(x =>
        {
            Console.WriteLine(x.Url);
            ScreenshotPreview.Children.Add(new WebImage() { ImageUrl = x.Url,Width = 200,Height = 200 });
        });
    }
}