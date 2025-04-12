using RMCLConfigServer.Modules;
using RMCLConfigServer.Modules.Classes;
using RMCLConfigServer.Modules.Classes.Network;
using Serilog;

namespace RMCLConfigServer;

public static class Program
{
    public static void Main(string[] args)
    {
        Init.InitServer();
        while (true) { }
    }
}

