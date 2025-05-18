using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Controls.ControlHelper;

namespace RMCL.Controls.Item;

public partial class GameDirectoryItem : UserControl
{
    public GameDirectoryItem(GameDirectoryItemInfo info)
    {
        InitializeComponent();
        if(!Directory.Exists(Path.GetFullPath(info.Path))) Directory.CreateDirectory(Path.GetFullPath(info.Path));
        
        DirName.Text = info.Name;
        DirPath.Text = info.Path;
    }
}