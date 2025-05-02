using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SongResponse
{
    [JsonPropertyName("result")]
    public Result Result { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }
}

public class Result
{
    [JsonPropertyName("songs")] public List<Song> Songs { get; set; } = new ();

    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    [JsonPropertyName("songCount")]
    public int SongCount { get; set; }
}

public class Song
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; }

    [JsonPropertyName("album")]
    public Album Album { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("copyrightId")]
    public int CopyrightId { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("alias")]
    public List<string> Alias { get; set; }

    [JsonPropertyName("rtype")]
    public int Rtype { get; set; }

    [JsonPropertyName("ftype")]
    public int Ftype { get; set; }

    [JsonPropertyName("mvid")]
    public int Mvid { get; set; }

    [JsonPropertyName("fee")]
    public int Fee { get; set; }

    [JsonPropertyName("rUrl")]
    public string RUrl { get; set; }

    [JsonPropertyName("mark")]
    public long Mark { get; set; }
}

public class Artist
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("picUrl")]
    public string PicUrl { get; set; }

    [JsonPropertyName("alias")]
    public List<string> Alias { get; set; }

    [JsonPropertyName("albumSize")]
    public int AlbumSize { get; set; }

    [JsonPropertyName("picId")]
    public long PicId { get; set; }

    [JsonPropertyName("fansGroup")]
    public string FansGroup { get; set; }

    [JsonPropertyName("img1v1Url")]
    public string Img1v1Url { get; set; }

    [JsonPropertyName("img1v1")]
    public int Img1v1 { get; set; }

    [JsonPropertyName("trans")]
    public string Trans { get; set; }
}

public class Album
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("artist")]
    public Artist Artist { get; set; }

    [JsonPropertyName("publishTime")]
    public long PublishTime { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("copyrightId")]
    public int CopyrightId { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("picId")]
    public long PicId { get; set; }

    [JsonPropertyName("mark")]
    public int Mark { get; set; }
}