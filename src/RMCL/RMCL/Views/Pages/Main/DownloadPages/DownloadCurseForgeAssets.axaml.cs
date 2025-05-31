using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OverrideLauncher.Core.Modules.Classes.Download.Assets.CurseForge;
using OverrideLauncher.Core.Modules.Entry.DownloadEntry.DownloadAssetsEntry;

namespace RMCL.Views.Pages.Main.DownloadPages;

public partial class DownloadCurseForgeAssets : UserControl
{
    public DownloadCurseForgeAssets()
    {
        InitializeComponent();
    }

    private void SearchBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var name = SearchBox.Text;

        Task.Run(() =>
        {
            var result = CurseForgeSearch.Search(new CurseForgeSearchInfo()
            {
                ApiKey = "$2a$10$Awb53b9gSOIJJkdV3Zrgp.CyFP.dI13QKbWn/4UZI4G4ff18WneB6",
                SearchName = name,
                GameVersion = "",
                Index = 0,
                PageSize = 2
            }).Result;
        });
    }
}