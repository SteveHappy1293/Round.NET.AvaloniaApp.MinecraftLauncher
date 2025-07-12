using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RMCL.Base.Interface;
using RMCL.Controls.View;

namespace RMCL.Core.Views.Pages.ChildFramePage.Game.GameClientSettingSubPages;

public partial class ClientResourcePackSetting : ISetting ,IUISetting
{
    public async void UpdateUI()
    {
        var path = System.IO.Path.Combine(Path, PathsDictionary.PathDictionary.ClientResourcePackFolderName);
        if (!Directory.Exists(path)) return;
        
        var files = Directory.EnumerateFiles(path)
            .ToList();

        foreach (var file in files)
        {
            try
            {
                var bitmap = await Task.Run(() => Models.Classes.Core.ImageResourcePool.GetImage(file,210));
                
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                // 记录错误或忽略。
            }
        }
    }
    public string Path { get; set; }
    public ClientResourcePackSetting()
    {
        InitializeComponent();
    }
}