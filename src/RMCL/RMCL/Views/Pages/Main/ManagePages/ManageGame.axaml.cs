using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Models.Classes;
using RMCL.Views.Windows.Main.ManageWindows;

namespace RMCL.Views.Pages.Main.ManagePages;

public partial class ManageGame : UserControl
{
    public bool IsEdit { get; set; } = false;
    public ManageGame()
    {
        InitializeComponent();
        
        Refuse();
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
    }
    private void RefuseButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Refuse();
    }

    private void SearchBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        throw new System.NotImplementedException();
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
        }
    }
}