// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using BlazorDesktop.Services;
using BlazorDesktop.Wpf;

namespace BlazorDesktop.Hosting;

/// <summary>
/// A builder for configuring and creating a <see cref="BlazorDesktopHost"/>.
/// </summary>
public sealed class BlazorDesktopHostBuilder
{
    /// <summary>
    /// The host builder.
    /// </summary>
    private readonly IHostBuilder _hostBuilder;

    /// <summary>
    /// Gets an <see cref="ConfigurationManager"/> that can be used to customize the application's
    /// configuration sources and read configuration attributes.
    /// </summary>
    public ConfigurationManager Configuration { get; }

    /// <summary>
    /// Gets the collection of root component mappings configured for the application.
    /// </summary>
    public RootComponentMappingCollection RootComponents { get; }

    /// <summary>
    /// Gets the service collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets information about the app's host environment.
    /// </summary>
    public IWebHostEnvironment HostEnvironment { get; }

    /// <summary>
    /// Gets the logging builder for configuring logging services.
    /// </summary>
    public ILoggingBuilder Logging { get; }

    /// <summary>
    /// Exposes configuration options for the window.
    /// </summary>
    public ConfigureWindowBuilder Window { get; }

    /// <summary>
    /// Creates an instance of <see cref="BlazorDesktopHostBuilder"/> with the minimal configuration.
    /// </summary>
    /// <param name="args">The arguments passed to the application's main method.</param>
    internal BlazorDesktopHostBuilder(string[]? args)
    {
        _hostBuilder = Host.CreateDefaultBuilder(args);

        Configuration = new();
        RootComponents = new();
        Services = new ServiceCollection();
        Logging = new LoggingBuilder(Services);
        Window = new(Configuration);
        HostEnvironment = InitializeEnvironment(args);
    }

    /// <summary>
    /// Creates an instance of <see cref="BlazorDesktopHostBuilder"/> using the most common
    /// conventions and settings.
    /// </summary>
    /// <param name="args">The arguments passed to the application's main method.</param>
    /// <returns>A <see cref="BlazorDesktopHostBuilder"/>.</returns>
    public static BlazorDesktopHostBuilder CreateDefault(string[]? args = default)
    {
        return new(args);
    }

    /// <summary>
    /// Builds a <see cref="BlazorDesktopHost"/> instance based on the configuration of this builder.
    /// </summary>
    /// <returns>A <see cref="BlazorDesktopHost"/> object.</returns>
    public BlazorDesktopHost Build()
    {
        ((BlazorDesktopHostingEnvironment)HostEnvironment).SaveConfig(Configuration);

        var builder = _hostBuilder.ConfigureAppConfiguration(builder =>
        {
            builder.AddConfiguration(Configuration);
        })
        .ConfigureServices(services =>
        {
            services.AddWpfBlazorWebView();

            services.AddSingleton(HostEnvironment);
            services.AddSingleton(RootComponents);
            services.AddSingleton<Window, BlazorDesktopWindow>();
            services.AddSingleton<Application>();

            services.AddHostedService<BlazorDesktopService>();

            foreach (var service in Services)
            {
                services.Add(service);
            }
        });

        return new(builder.Build());
    }

    /// <summary>
    /// Adds Chromium dev tools to the Blazor Desktop application.
    /// </summary>
    /// <returns>The <see cref="BlazorDesktopHostBuilder"/>.</returns>
    public BlazorDesktopHostBuilder UseDeveloperTools()
    {
        Services.AddBlazorWebViewDeveloperTools();
        return this;
    }

    /// <summary>
    /// Initializes the environment.
    /// </summary>
    /// <param name="args">The arguments passed to the application's main method.</param>
    /// <returns>A <see cref="BlazorDesktopHostingEnvironment"/>.</returns>
    private static BlazorDesktopHostingEnvironment InitializeEnvironment(string[]? args)
    {
        var environmentHost = Host.CreateDefaultBuilder(args).Build();
        var environment = new BlazorDesktopHostingEnvironment();
        var config = environmentHost.Services.GetRequiredService<IConfiguration>();

        environment.LoadConfig(config);

        return environment;
    }
}
