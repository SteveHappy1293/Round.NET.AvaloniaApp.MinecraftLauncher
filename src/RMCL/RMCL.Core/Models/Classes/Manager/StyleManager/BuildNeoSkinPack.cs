using System;
using System.IO;
using System.Text.Json;
using HarfBuzzSharp;
using RMCL.Base.Entry.Style;
using RMCL.Base.Enum;

namespace RMCL.Core.Models.Classes.Manager.StyleManager;

public class BuildNeoSkinPack
{
    public SkinStyleConfig Config { get; set; }
    public string ResultPath { get; set; }

    public void BuildPack()
    {
        var path = PathsDictionary.PathDictionary.SkinTempFolder;

        if (Directory.Exists(path)) Directory.Delete(path, true);
        Directory.CreateDirectory(path);

        if (Config.BackgroundModel != null &&
            Config.BackgroundModel.ChooseModel == BackgroundModelEnum.Image)
        {
            Directory.CreateDirectory(Path.Combine(path, "Image"));
            var ima = Config.BackgroundModel.ImageEntry.ImagePaths[Config.BackgroundModel.ImageEntry.ChooseIndex];
            File.Copy(ima,
                Path.Combine(path, "Image", Path.GetFileName(ima)));

            Config.Background = Path.Combine("Image", Path.GetFileName(ima));
            Config.Opacity = Config.BackgroundModel.ImageEntry.Opacity;
        }

        File.WriteAllText(Path.Combine(path, "Style.json"), JsonSerializer.Serialize(Config));

        try
        {
            string zipFile = Path.Combine(PathsDictionary.PathDictionary.SkinRootFolder, "Style.rskin");

            SystemHelper.SystemHelper.CreateZipFromFolder(path, zipFile);
            ResultPath = zipFile;
            Console.WriteLine($"创建ZIP文件成功");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建ZIP文件时出错: {ex.Message}");
        }
    }

    public void CollectConfig(bool background, bool color, bool button)
    {
        Config = new();
        Config.PackVersion = 2;
        Config.BackgroundModel = background ? RMCL.Config.Config.MainConfig.Background : null;
        Config.ThemeColors = color ? RMCL.Config.Config.MainConfig.ThemeColors : null;
        Config.ButtonStyle = button ? RMCL.Config.Config.MainConfig.ButtonStyle : null;

        Config.IsBackground = background;
        Config.IsButton = button;
        Config.IsColor = color;

        BuildPack();
    }
}