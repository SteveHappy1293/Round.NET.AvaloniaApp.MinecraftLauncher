using BedrockPlug.View.Pages;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;

namespace BedrockPlug;

public class Main
{
    public void InitPlug()
    {
        Core.API.RegisterMangePage(new()
        {
            Icon = IconType.Gift,
            Route = "Bedrock",
            Title = "基岩版",
            Page = new BedrockMange()
        });
    }
}