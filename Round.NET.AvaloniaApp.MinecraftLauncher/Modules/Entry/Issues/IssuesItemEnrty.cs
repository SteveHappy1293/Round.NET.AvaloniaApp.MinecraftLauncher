using System;
using System.Collections.Generic;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Issues;

public class IssuesItemEnrty
{
    public string url { get; set; }
    public string html_url { get; set; }
    public string title { get; set; }
    public List<IssuesLabelEntry> labels { get; set; }
    public int comments { get; set; } = 0;
    public string state { get; set; }
    public DateTime created_at { get; set; }
    public int number { get; set; }
    public IssuesUserEntry user { get; set; }
    public IssuesTypeEntry type { get; set; }
}