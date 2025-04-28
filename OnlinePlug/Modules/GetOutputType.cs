using System.Text.RegularExpressions;

namespace OnlinePlug.Modules;

public class OutputTypeParser
{
    public static string ParseMessageType(string message)
    {
        if (message.Contains("autorunApp start"))
        {
            return "程序启动完毕，请耐心等待隧道连接";
        }

        if (message.Contains("autorunApp end"))
        {
            return "程序离线，请检查你的网络设置或查看网络连接是否正常";
        }

        if (message.Contains("LISTEN ON PORT"))
        {
            var portPattern = @"PORT\s+(\w+:\d+)";
            var portMatch = Regex.Match(message, portPattern);
            if (portMatch.Success)
            {
                var portInfo = portMatch.Groups[1].Value;
                if (message.Contains("START"))
                {
                    return "隧道本地端口为 " + portInfo + " 连接成功";
                }

                if (message.Contains("END"))
                {
                    return "隧道本地端口为 " + portInfo + " 断开连接";
                }
            }
        }

        if (message.Contains("ERROR P2PNetwork login error"))
        {
            return "请检查是否连接网络，或是程序是否拥有网络访问权限！";
        }

        if (message.Contains("Only one usage of each socket address"))
        {
            var socketPattern = @"(tcp|udp)\s*:\s*(\d+)";
            var socketMatch = Regex.Match(message, socketPattern);

            if (socketMatch.Success)
            {
                var protocol = socketMatch.Groups[1].Value;
                var port = socketMatch.Groups[2].Value;
                return "本地端口" + protocol + ":" + port + "被占用，请更换相关本地端口";
            }
        }

        if (message.Contains("no such host"))
        {
            return "请检查 DNS 是否正确，是否连接网络，或是程序是否拥有网络访问权限！";
        }

        if (message.Contains("it will auto reconnect when peer node online"))
        {
            var peerPattern = @"INFO\s+(\w+)\s+offline";
            var peerMatch = Regex.Match(message, peerPattern);
            if (peerMatch.Success)
            {
                var peerId = peerMatch.Groups[1].Value;
                return $"无法查找到 {peerId} 的房间，请确保对方已开启房间。";
            }
        }

        if (message.Contains("peer offline"))
        {
            return "无法查找到房间，请确保对方已开启房间。";
        }

        if (message.Contains("NAT type"))
        {
            var natPattern = @"NAT type:(\w+)";
            var natMatch = Regex.Match(message, natPattern);
            if (natMatch.Success)
            {
                var natType = natMatch.Groups[1].Value;
                if (natType == "2")
                    return "你的NAT类型为对称形 Symmetric NAT，连接可能受阻，或连接时间较长";
            }
        }

        if (message.Contains("login ok"))
        {
            var nodePattern = @"node=(\w+)";
            var nodeMatch = Regex.Match(message, nodePattern);
            if (nodeMatch.Success)
            {
                var nodeId = nodeMatch.Groups[1].Value;
                return "你的实际UID为" + nodeId;
            }
        }

        return message;
    }
}