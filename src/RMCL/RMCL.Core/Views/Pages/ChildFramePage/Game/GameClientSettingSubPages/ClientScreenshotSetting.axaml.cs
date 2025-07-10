using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RMCL.Base.Interface;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientScreenshotSetting : ISetting ,IUISetting
{
    public void UpdateUI()
    {
        var path = System.IO.Path.Combine(Path, PathsDictionary.PathDictionary.ClientScreenshotsFolderName);
        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path).ToList();
            
        }
    }
    public string Path { get; set; }
    public ClientScreenshotSetting()
    {
        InitializeComponent();
    }
}