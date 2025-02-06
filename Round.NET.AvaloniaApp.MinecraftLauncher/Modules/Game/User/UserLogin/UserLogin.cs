using System;
using System.Threading.Tasks;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;

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
            await authenticator.DeviceFlowAuthAsync(dc => {
                //在获取到一次性代码后要执行的代码
                Console.WriteLine(dc.UserCode);
                Console.WriteLine(dc.VerificationUrl);
                GetedCode(dc.UserCode);
            });

            var userProfile = await authenticator.AuthenticateAsync();
            LoggedIn(userProfile);
        }
    }
}