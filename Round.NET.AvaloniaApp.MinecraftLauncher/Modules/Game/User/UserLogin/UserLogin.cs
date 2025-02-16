using System;
using System.Threading.Tasks;
using MinecraftLaunch.Base.Models.Authentication;
using MinecraftLaunch.Components.Authenticator;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.UserLogin;

public class UserLogin
{
    public class MicrosoftLogin
    {
        public Action<string> GetedCode;
        public Action<Account> LoggedIn;

        public async Task Login()
        {
            MicrosoftAuthenticator authenticator = new("c06d4d68-7751-4a8a-a2ff-d1b46688f428");
            var oA = await authenticator.DeviceFlowAuthAsync(dc => {
                //在获取到一次性代码后要执行的代码
                RLogs.WriteLog(dc.UserCode);
                RLogs.WriteLog(dc.VerificationUrl);
                GetedCode(dc.UserCode);
            });

            var userProfile = await authenticator.AuthenticateAsync(oA);
            LoggedIn(userProfile);
        }
    }
}