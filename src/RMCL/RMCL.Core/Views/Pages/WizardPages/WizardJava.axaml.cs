using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using RMCL.Controls.Item;

namespace RMCL.Core.Views.Pages.WizardPages;

public partial class WizardJava : UserControl
{
    public WizardJava()
    {
        InitializeComponent();
    }

    public void UpdateUI()
    {
        JavasBox.Children.Clear();
        ControlPanel.IsEnabled = false;
        ProgressBar.IsVisible = true;
        NullBox.IsVisible = true;
        JavaList.IsExpanded = true;

        Task.Run(() =>
        {
            var javas = JavaManager.JavaManager.SearchJavaAsync().Result;
            var list = javas.ToList();
            list.ForEach(x =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    JavasBox.Children.Add(
                        new ItemBox() { Content = x.JavaPath, Margin = new Thickness(5), Height = 20 });
                });
            });
            var searchJavaAsync = list;
            JavaManager.JavaManager.JavaRoot.Javas.AddRange(searchJavaAsync);
            JavaManager.JavaManager.SaveConfig();

            Dispatcher.UIThread.Invoke(() =>
            {
                JavaList.IsExpanded = true;
                NullBox.IsVisible = false;
                ProgressBar.IsVisible = false;
                ControlPanel.IsEnabled = true;
            });
        });
    }

    private void SearchJavaBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        UpdateUI();
    }
}