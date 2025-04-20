using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OnlinePlug.Modules;

namespace OnlinePlug.Views.Pages.SubPages;

public partial class CreateRoom : UserControl
{
    public int Port { get; set; } = 0;
    public string RoomName { get; set; } = "";
    public CreateRoom()
    {
        InitializeComponent();
    }

    public void Start()
    {
        RoomNameT.Text = $"房间名称：{RoomName.Split(" - ")[1]}";
        RoomOwner.Text = $"房间管理：{RoomName.Split(" - ")[0]}";
        RoomPort.Text =  $"房间端口：{Port.ToString()}";
        BuildOnlineConfig.Save();

        var run = new Runner(Path.GetFullPath("..\\RMCL\\RMCL.OnlinePlug\\Console\\openp2p.exe"));
        run.OutputAction = (l) =>
        {
            Dispatcher.UIThread.Invoke(() => RoomStatus.Text = OutputTypeParser.ParseMessageType(l));
        };
        run.Run();
    }
}