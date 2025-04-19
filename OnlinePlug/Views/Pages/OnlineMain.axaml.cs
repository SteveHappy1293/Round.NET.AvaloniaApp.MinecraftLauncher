using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnlinePlug.Views.Pages.SubPages;

namespace OnlinePlug.Views.Pages;

public partial class OnlineMain : UserControl
{
    public OnlineMain()
    {
        InitializeComponent();

        MainFrame.Content = new OnlineHome();
    }
}