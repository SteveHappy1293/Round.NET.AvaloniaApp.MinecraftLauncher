using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RMCL;

public partial class CloseWindow : Window
{
    public CloseWindow()
    {
        InitializeComponent();
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void WaitForTheTask_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ForceQuit_OnClick(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}