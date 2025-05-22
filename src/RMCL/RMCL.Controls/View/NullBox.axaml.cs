using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RMCL.Controls.View;

public partial class NullBox : UserControl
{
    public string BigTitle { get; set; } = "空";
    public string SmallTitle { get; set; } = "为什么这里什么都没有呢？";
    public NullBox()
    {
        InitializeComponent();

        this.Loaded += (sender, args) =>
        {
            BigTitleBox.Text = BigTitle;
            SmallTitleBox.Text = SmallTitle;
        };
    }
}