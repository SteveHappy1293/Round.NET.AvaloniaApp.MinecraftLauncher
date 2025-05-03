using System;
using System.Threading.Tasks;
using OverrideLauncher.Core.Modules.Entry.AccountEntry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.UserLogin;

public class UserLogin
{
    public class MicrosoftLogin
    {
        public Action<string> GetedCode;
        public Action<AccountEntry> LoggedIn;

        public void Login()
        {
            MicrosoftAuthenticator authenticator = new("c06d4d68-7751-4a8a-a2ff-d1b46688f428");
            authenticator.Login = (sender) =>
            {
                GetedCode.Invoke(sender.Code);
            };
            
            var userProfile = authenticator.Authenticator().Result;
            LoggedIn(userProfile);
        }
    }
}