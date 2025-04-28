class Program
{
    static async Task Main(string[] args)
    {
        var config = new AppConfig
        {
            Network = new NetworkConfig
            {
                Token = 11602319472897248650,
                Node = "a946e1ab3e2e544e",
                ServerHost = "api.openp2p.cn",
                ServerPort = 27183
            }
        };

        using (var p2p = new P2PNetwork())
        {
            var result = await p2p.RequestPeerInfoAsync(config);
            if (result == ErrorType.None)
            {
                Console.WriteLine($"Successfully got peer info: {config.PeerIP}");
            }
            else
            {
                Console.WriteLine($"Error: {result}");
            }
        }
    }
}