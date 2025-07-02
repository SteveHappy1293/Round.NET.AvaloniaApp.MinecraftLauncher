using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RMCL.Core.Views.Windows.Main.Client;

public partial class ClientLogViewWindow : Window
{
    public ClientLogViewWindow()
    {
        InitializeComponent();
    }
    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
    private void TitleBar_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}