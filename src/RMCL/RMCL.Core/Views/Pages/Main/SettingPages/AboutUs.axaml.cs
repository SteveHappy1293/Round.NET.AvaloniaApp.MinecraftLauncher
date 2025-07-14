using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;

namespace RMCL.Core.Views.Pages.Main.SettingPages;

public partial class AboutUs : UserControl
{
    public AboutUs()
    {
        InitializeComponent();
        
        VersionBox.Text = $"{App.Current.GetType().Assembly.GetName().Version}";
    }

    private int num = 0;
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        num++;
        if (num == 10)
        {
            num = 0;
            new ContentDialog()
            {
                Title = "小彩蛋",
                Content = "其实 一角钱 经常把 YangSpring429 叫做 YangWei429 (阳痿429)，恭喜你发现了这个彩蛋",
                CloseButtonText = "确定"
            }.ShowAsync();
        }
    }
}