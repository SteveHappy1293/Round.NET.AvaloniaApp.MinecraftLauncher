using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using RMCL.Controls.Container;
using RMCL.Controls.ControlHelper.Wizard;
using RMCL.Core.Views.Pages.WizardPages;

namespace RMCL.Core.Views.Windows.Initialize;

public partial class InitializeWindow : Window
{
    public InitializeWindow()
    {
        InitializeComponent();
        RenderOptions.SetTextRenderingMode(this, TextRenderingMode.SubpixelAntialias); // 字体渲染模式
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.MediumQuality); // 图片渲染模式
        RenderOptions.SetEdgeMode(this, EdgeMode.Antialias); // 形状渲染模式
        
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardWelcome(),
            Title = "欢迎"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardPlayer(),
            Title = "创建新账户"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardGameFolder(),
            Title = "新增游戏目录"
        });
        MainWizardFrame.RegistedWizard(new WizardPageEntry()
        {
            Page = new WizardJava(),
            Title = "全局 Java 设置"
        });
    }
        
    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
        
    private void TitleBar_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
        
    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}