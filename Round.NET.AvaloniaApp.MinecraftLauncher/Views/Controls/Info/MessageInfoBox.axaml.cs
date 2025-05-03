using Avalonia;
using Avalonia.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Info;

public partial class MessageInfoBox : UserControl
{
    public MessageInfoBox(string Message,string Title = "消息")
    {
        InitializeComponent();
        TitleBox.Text = Title;
        InfoBox.Text = Message;
    }
}