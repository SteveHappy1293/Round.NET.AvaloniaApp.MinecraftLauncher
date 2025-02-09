using FluentAvalonia.UI.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

public class LoadingContentDialog
{
    public ContentDialog Show()
    {
        var dialog = new ContentDialog()
        {
            Title = "加载中...",
            Content = "请等一下，马上就好",
        };
        dialog.ShowAsync(Core.MainWindow);
        return dialog;
    }
}