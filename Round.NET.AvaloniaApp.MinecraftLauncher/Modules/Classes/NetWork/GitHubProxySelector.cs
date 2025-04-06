using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public class GitHubProxySelector
{
    private List<string> proxyTemplates;

    public GitHubProxySelector()
    {
        proxyTemplates = new List<string>
        {
            "https://ghproxy.com/{url}",
            "https://gh.ddlc.top/{url}",
            "https://ghps.cc/{url}",
            "https://github.moeyy.xyz/{url}",
            "https://gh.idayer.com/{url}",
            "https://gh-proxy.com/{url}",
            "https://mirror.ghproxy.com/{url}",
            "https://gh.api.99988866.xyz/{url}",
            "https://ghproxy.net/{url}",
            "https://gh-proxy.net/{url}",
            "https://gh.b52m.cn/{url}"
        };
    }

    public string GetBestProxyUrl(string uri)
    {
        var proxyUrls = GenerateProxyUrls(uri);
        var pingResults = new List<(string Url, long PingTime)>();

        using (Ping ping = new Ping())
        {
            foreach (var url in proxyUrls)
            {
                try
                {
                    PingReply reply = ping.Send(url.Replace("https://", "").Replace("http://", "").Split('/')[0]);
                    if (reply.Status == IPStatus.Success)
                    {
                        pingResults.Add((url, reply.RoundtripTime));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ping {url.Replace("https://", "").Replace("http://", "").Split('/')[0]} failed: {ex.Message}");
                }
            }
        }

        if (pingResults.Count == 0)
        {
            throw new InvalidOperationException("没有找到可用的代理。");
        }

        var bestProxy = pingResults[0];
        foreach (var result in pingResults)
        {
            if (result.PingTime < bestProxy.PingTime)
            {
                bestProxy = result;
            }
        }

        return bestProxy.Url;
    }

    private List<string> GenerateProxyUrls(string uri)
    {
        var urls = new List<string>();
        foreach (var template in proxyTemplates)
        {
            try
            {
                urls.Add(string.Format(template, uri));
            }
            catch (FormatException)
            {
                urls.Add(template.Replace("{url}", uri));
            }
        }
        return urls;
    }
}