namespace RMCL.OnlineCore.Classes.OpenP2P;

public class OpenP2PClient
{
    public OpenP2PClient()
    {
        if(!InitializeEnvironment.IsInitialized) throw new Exception("未初始化环境");
        if(!File.Exists(InitializeEnvironment.OpenP2PCoreFile)) throw new Exception("未找到核心文件，请尝试重新初始化环境 或 关闭杀毒软件后重试");
        
        
    }
}