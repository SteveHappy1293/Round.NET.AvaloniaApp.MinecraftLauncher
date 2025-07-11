using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Core.Views.Pages.DialogPage.GameDrawerPages;

public partial class AddGameItem : UserControl
{
    public string GameFolder { get;private set; }
    public string GameName { get;private set; }
    public AddGameItem()
    {
        InitializeComponent();
    }
}