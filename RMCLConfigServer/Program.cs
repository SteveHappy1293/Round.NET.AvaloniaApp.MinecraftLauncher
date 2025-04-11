using RMCLConfigServer.Modules.Classes;
using RMCLConfigServer.Modules.Classes.Network;
using Serilog;

namespace RMCLConfigServer;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        Log.Information("Starting Config Server!");
        Config.Load();
        Log.Information("The configuration is loaded.");
        
        Log.Information($"Server Port: {Config.MainConfig.ServerPort}");
        Log.Information($"Server Name: {Config.MainConfig.ServerName}");
        var server = new Server(Config.MainConfig.ServerPort);
        server.Start();
        while (true) { }
    }
}

