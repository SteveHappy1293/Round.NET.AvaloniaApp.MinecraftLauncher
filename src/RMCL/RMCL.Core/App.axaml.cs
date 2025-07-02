using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using HotAvalonia;
using NetworkService.SingleInstanceDetector;
using RMCL.Base.Entry.Style;
using RMCL.Config;
using RMCL.Core.Views;
using RMCL.Core.Views.Windows;

namespace RMCL.Core;

public partial class App : Application
{
    public override void Initialize()
    {
        Console.WriteLine("App Init...");
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        Console.WriteLine("On Framework Initialization Completed");
        DisableAvaloniaDataAnnotationValidation();
        this.Styles.Add(Core.Models.Classes.Core.FluentAvaloniaTheme);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        RequestedThemeVariant = Config.Config.MainConfig.ThemeColors.Theme switch
        {
            ThemeType.Dark => ThemeVariant.Dark,
            ThemeType.Light => ThemeVariant.Light
        };

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}