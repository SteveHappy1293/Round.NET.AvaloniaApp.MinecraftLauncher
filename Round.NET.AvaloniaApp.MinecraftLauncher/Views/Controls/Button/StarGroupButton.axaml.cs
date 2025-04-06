using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Entry.Stars;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Button
{
    public class StarGroupButton : Avalonia.Controls.Button
    {
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<StarGroupButton, string>(nameof(Title), "");
        
        public static readonly StyledProperty<int> CountProperty =
            AvaloniaProperty.Register<StarGroupButton, int>(nameof(Count), 0);
        
        public static readonly StyledProperty<string> CountStrProperty =
            AvaloniaProperty.Register<StarGroupButton, string>(nameof(CountStr), "0");

        public static readonly StyledProperty<IImage> ImageProperty =
            AvaloniaProperty.Register<StarGroupButton, IImage>(
                nameof(Image));
        public List<StarItemEnrty> StarItems { get; set; } = new();
        static StarGroupButton()
        {
            CountProperty.Changed.AddClassHandler<StarGroupButton>((x, e) => 
                x.CountStr = $"{x.Count}");
        }

        public IImage Image
        {
            get => GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        
        public int Count
        {
            get => GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public string CountStr
        {
            get => GetValue(CountStrProperty);
            private set => SetValue(CountStrProperty, value);
        }
    }
}