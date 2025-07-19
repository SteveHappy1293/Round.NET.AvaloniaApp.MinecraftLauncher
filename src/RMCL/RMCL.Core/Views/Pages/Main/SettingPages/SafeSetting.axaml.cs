using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RMCL.Core.Views.Pages.ChildFramePage.Safe;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class SafeSetting : UserControl
{
    public SafeSetting()
    {
        InitializeComponent();
    }

    private void OpenLogManagerBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.Models.Classes.Core.ChildFrame.Show(new ManageLogsChildPage());
    }

    private void OpenMistakManagerBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Core.Models.Classes.Core.ChildFrame.Show(new ManageExceptionChildPage());
    }
}