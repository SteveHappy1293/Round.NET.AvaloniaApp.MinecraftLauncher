using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;

namespace BedrockPlug.View.Pages;

public partial class BedrockMange : UserControl
{
    public BedrockMange()
    {
        InitializeComponent();
        this.Loaded += (s, e) =>
        {
            Load();
        };
    }

    public void Load()
    {
        BedrockBox.Children.Clear();
        var dir = Directory.GetDirectories("../RMCL/RMCL.Bedrock/Games");
        foreach (var se in dir)
        {
            BedrockBox.Children.Add(new ListBoxItem()
            {
                Padding = new Thickness(10),
                Opacity = 0,
                Content = new DockPanel()
                {
                    Children =
                    {
                        new TextBlock()
                        {
                            Text = Path.GetFileName(se).Split('_')[1],
                        },
                        new DockPanel()
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Children =
                            {
                                new Button()
                                {
                                    Height = 32,
                                    Width = 32,
                                    Margin = new Thickness(5),
                                    Content = new FluentIcon()
                                    {
                                        Icon = FluentIconSymbol.Airplane20Regular
                                    }
                                }
                            }
                        }
                    }
                },
            });
        }
    }
}