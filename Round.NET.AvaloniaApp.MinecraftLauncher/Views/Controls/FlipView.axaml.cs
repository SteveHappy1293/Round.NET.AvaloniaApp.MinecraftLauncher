using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Newtonsoft.Json;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls;

public partial class FlipView : UserControl
{
    private List<Control> Controls { get; set; } = new();
    private int index = 0;

    public void SetSource(List<Control> controls)
    {
        Controls = controls;
        index = 0; 
        UpdateButtons();
        Main.Content = controls[0];
    }
    public FlipView()
    {
        InitializeComponent();
        var root = JsonConvert.DeserializeObject<Root>(
            new WebClient().DownloadString("https://launchercontent.mojang.com/javaPatchNotes.json"));
        var temp = new List<Control>();
        foreach (var entry in root.Entries.Take(20))
        {
            temp.Add(EntryToGrid(entry));
        }
        SetSource(temp);
    }

    public static bool ContainsLinkFormat(string text)
    {
        string pattern = @".*<a.*</a>.*";

        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public static Control EntryToGrid(Entry source)
    {
        try
        {
            var backgroundImage = new Avalonia.Controls.Image
            {
                Stretch = Stretch.UniformToFill,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            LoadImageAsync(backgroundImage, $"https://launchercontent.mojang.com{source.Image.Url}");
            var imagestackPanel = new StackPanel
            {
                Margin = new Thickness(2), VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            imagestackPanel.Children.Add(new TextBlock
            {
                Text = source.Title, FontSize = 20, Margin = new Thickness(0, 2, 0, 0),
                FontWeight = Avalonia.Media.FontWeight.Bold
            });
            imagestackPanel.Children.Add(new TextBlock
            {
                Text = $"Minecraft {source.Version}", FontSize = 14, Margin = new Thickness(0, 2, 0, 8),
                FontWeight = Avalonia.Media.FontWeight.Light
            });
            var grid = new Grid();
            grid.Children.Add(backgroundImage);
            grid.Children.Add(imagestackPanel);
            StackPanel stackPanel = new();
            
            foreach (string line in source.Body.Split("\n"))
            {
                var color = new SolidColorBrush { Color = Colors.White };
                if (ContainsLinkFormat(line))
                {
                    color.Color = Colors.Blue;
                    Regex.Replace(line, @".*<a.*</a>.*", "");
                }

                var data = line.Replace("<code>", "").Replace("</code>", "");
                if (data.StartsWith("<p>"))
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = data.Replace("<p>", "").Replace(@"</p>", ""), FontSize = 12,
                        Margin = new Thickness(0, 2, 0, 0)
                    });
                if (data.StartsWith("<h1>"))
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = data.Replace("<h1>", "").Replace(@"</h1>", ""), FontSize = 18,
                        Margin = new Thickness(0, 2, 0, 0), FontWeight = Avalonia.Media.FontWeight.Bold
                    });
                if (data.StartsWith("<h2>"))
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = data.Replace("<h2>", "").Replace(@"</h2>", ""), FontSize = 16,
                        Margin = new Thickness(0, 2, 0, 0), FontWeight = Avalonia.Media.FontWeight.Bold
                    });
                if (data.StartsWith("<h3>"))
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = data.Replace("<h3>", "").Replace(@"</h3>", ""), FontSize = 14,
                        Margin = new Thickness(0, 2, 0, 0), FontWeight = Avalonia.Media.FontWeight.Bold
                    });
                if (data.StartsWith("<li>"))
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = $"   · {data.Replace("<li>", "").Replace(@"</li>", "")}", FontSize = 14,
                        Margin = new Thickness(0, 2, 0, 0)
                    });
            }

            return grid;
        }
        catch (Exception ex)
        {
            new Window { Content = ex.ToString() }.Show();
            return new Grid();
        }
    }

    public static async Task LoadImageAsync(Avalonia.Controls.Image imageControl, string imageUrl)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(imageUrl);

        if (response.IsSuccessStatusCode)
        {
            await using var stream = await response.Content.ReadAsStreamAsync();
            var bitmap = new Bitmap(stream);

            Dispatcher.UIThread.Post(() => { imageControl.Source = bitmap; });
        }
    }

    public class Root
    {
        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("entries")] public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("version")] public string Version { get; set; }

        [JsonProperty("image")] public Image Image { get; set; }

        [JsonProperty("body")] public string Body { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("contentPath")] public string ContentPath { get; set; }
    }

    public class Image
    {
        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("title")] public string Title { get; set; }
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        if (index > 0)
        {
            index--;
            Main.Opacity = 0;
            Main.Content = Controls[index];
            Main.Opacity = 1;
        }
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        Back.IsEnabled = index > 0;
        Back.Foreground =  index > 0 ? new SolidColorBrush { Color = Colors.White } : new SolidColorBrush { Color = Colors.Gray };
        Forward.IsEnabled = index < Controls.Count - 1;
        Forward.Foreground = index < Controls.Count - 1 ? new SolidColorBrush { Color = Colors.White } : new SolidColorBrush { Color = Colors.Gray };
    }
    private void Forward_OnClick(object? sender, RoutedEventArgs e)
    {
        if (index < Controls.Count - 1)
        {
            index++;
            Main.Opacity = 0;
            Main.Content = Controls[index];
            Main.Opacity = 1;
        }
        UpdateButtons();
    }
}