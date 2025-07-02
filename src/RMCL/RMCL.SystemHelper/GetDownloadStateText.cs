using OverrideLauncher.Core.Modules.Enum.Download;

namespace RMCL.SystemHelper;

public class GetDownloadStateText
{
    public static string GetText(DownloadStateEnum type)
    {
        return type switch
        {
            DownloadStateEnum.DownloadJson => "获取版本 Json 文件",
            DownloadStateEnum.DownloadAssets => "下载资源文件",
            DownloadStateEnum.DownloadAssetsSuccess => "下载资源文件成功",
            DownloadStateEnum.DownloadLibrary => "下载依赖文件",
            DownloadStateEnum.DownloadLibrarySuccess => "下载依赖文件成功",
            DownloadStateEnum.DownloadClient => "下载游戏本体",
            DownloadStateEnum.DownloadSuccess => "下载完成",

            DownloadStateEnum.CompletionJar => "补全游戏依赖",
            DownloadStateEnum.CompletionAssets => "补全资源文件",
            DownloadStateEnum.CompletionSuccess => "补全文件完毕",

            DownloadStateEnum.Error => "错误",
        };
    }
}