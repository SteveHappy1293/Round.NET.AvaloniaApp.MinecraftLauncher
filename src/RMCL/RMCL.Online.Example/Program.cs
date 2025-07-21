using RMCL.OnlineCore;
using RMCL.OnlineCore.Classes;
using RMCL.OnlineCore.Classes.OpenP2P;

Console.WriteLine("开始初始化环境...");
if(!InitializeEnvironment.Initialize("bin")) await InitializeEnvironment.OnDownloadCoreFile();
InitializeEnvironment.Token = 17190022896174664900;
InitializeEnvironment.User = "MinecraftYJQ_";
InitializeEnvironment.UUID = "test";
Console.WriteLine("环境初始化完成！");

OpenP2PClient openP2PClient = new OpenP2PClient();
