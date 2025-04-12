using System;
using System.Collections.Generic;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Lssues;

public class LssuesItemEnrty
{
    public string url { get; set; }
    public string html_url { get; set; }
    public string title { get; set; }
    public List<LssuesLabelEntry> labels { get; set; }
    public int comments { get; set; } = 0;
    public string state { get; set; }
    public DateTime created_at { get; set; }
    public int number { get; set; }
    public LssuesUserEntry user { get; set; }
    public LssuesTypeEntry type { get; set; }
}