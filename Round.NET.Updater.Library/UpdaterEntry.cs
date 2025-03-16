using System;
using System.Collections.Generic;

public class Author
{
    public string login { get; set; }
    public long id { get; set; }
    public string avatarUrl { get; set; }
    public string htmlUrl { get; set; }
}

public class Asset
{
    public string name { get; set; }
    public string browser_download_url { get; set; }
}

public class Release
{
    public string tagName { get; set; }
    public string name { get; set; }
    public List<Asset> assets { get; set; }
}

public class ResultEntry
{
    public string Name { get; set; } = String.Empty;
    public string URL { get; set; } = String.Empty;
    public string Version { get; set; } = String.Empty;
}