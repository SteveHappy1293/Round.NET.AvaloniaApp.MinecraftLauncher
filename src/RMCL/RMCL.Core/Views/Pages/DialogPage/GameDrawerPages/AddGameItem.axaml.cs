using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

public partial class AddGameItem : UserControl
{
    public string GameFolder { get; private set; }
    public string GameName { get; private set; }

    public AddGameItem()
    {
        InitializeComponent();

        Config.Config.MainConfig.GameFolders.ForEach(x =>
        {
            if (Directory.GetDirectories(Path.Combine(x.Path, "versions")).ToList().Count != 0)
            {
                ChosePathBox.Items.Add(x.Path);
            }
        });
        Directory.GetDirectories(Path.Combine(Config.Config.MainConfig.GameFolders[0].Path, "versions")).ToList()
            .ForEach(x => { ChoseClientBox.Items.Add(x); });
    }

    private void ChosePathBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GameFolder = ChosePathBox.SelectedItem.ToString();
        ChoseClientBox.Items.Clear();
        Directory.GetDirectories(Path.Combine(ChosePathBox.SelectedItem.ToString(), "versions")).ToList()
            .ForEach(x => { ChoseClientBox.Items.Add(Path.GetFileName(x)); });
    }

    private void ChoseClientBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GameName = ChoseClientBox.SelectionBoxItem.ToString();
    }
}