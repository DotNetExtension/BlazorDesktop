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
    /// Gets an <see cref="ConfigurationManager"/> that can be used to customize the application's
    /// configuration sources and read configuration attributes.
    /// </summary>
    public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

    /// <summary>
    /// Gets the collection of root component mappings configured for the application.
    /// </summary>
    public RootComponentMappingCollection RootComponents { get; }

    /// <summary>
    /// Gets the service collection.
    /// </summary>
    public IServiceCollection Services => _hostApplicationBuilder.Services;

    /// <summary>
    /// Gets information about the app's host environment.
    /// </summary>
    public IWebHostEnvironment HostEnvironment { get; }

    /// <summary>
    /// Gets the logging builder for configuring logging services.
    /// </summary>
    public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

    /// <summary>
    /// Exposes configuration options for the window.
    /// </summary>
    public ConfigureWindowBuilder Window { get; }

    private readonly HostApplicationBuilder _hostApplicationBuilder;

    private BlazorDesktopHostBuilder(string[]? args)
    {
        _hostApplicationBuilder = InitializeHostApplicationBuilder(args);

        InitializeDefaultServices();

        RootComponents = InitializeRootComponents();
        HostEnvironment = InitializeEnvironment();
        Window = InitializeWindowBuilder();
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
    /// Registers a <see cref="IServiceProviderFactory{TBuilder}" /> instance to be used to create the <see cref="IServiceProvider" />.
    /// </summary>
    /// <typeparam name="TBuilder">The type of builder provided by the <see cref="IServiceProviderFactory{TBuilder}" />.</typeparam>
    /// <param name="factory">The <see cref="IServiceProviderFactory{TBuilder}" />.</param>
    /// <param name="configure">
    /// A delegate used to configure the <typeparamref T="TBuilder" />. This can be used to configure services using
    /// APIs specific to the <see cref="IServiceProviderFactory{TBuilder}" /> implementation.
    /// </param>
    /// <exception cref="ArgumentNullException">Occurs when the <paramref name="factory"/> parameter is null.</exception>
    /// <remarks>
    /// <para>
    /// <see cref="ConfigureContainer{TBuilder}(IServiceProviderFactory{TBuilder}, Action{TBuilder})"/> is called by <see cref="Build"/>
    /// and so the delegate provided by <paramref name="configure"/> will run after all other services have been registered.
    /// </para>
    /// <para>
    /// Multiple calls to <see cref="ConfigureContainer{TBuilder}(IServiceProviderFactory{TBuilder}, Action{TBuilder})"/> will replace
    /// the previously stored <paramref name="factory"/> and <paramref name="configure"/> delegate.
    /// </para>
    /// </remarks>
    public void ConfigureContainer<TBuilder>(IServiceProviderFactory<TBuilder> factory, Action<TBuilder>? configure = null) where TBuilder : notnull
    {
        ArgumentNullException.ThrowIfNull(factory);

        _hostApplicationBuilder.ConfigureContainer(factory, configure);
    }

    /// <summary>
    /// Builds a <see cref="BlazorDesktopHost"/> instance based on the configuration of this builder.
    /// </summary>
    /// <returns>A <see cref="BlazorDesktopHost"/> object.</returns>
    public BlazorDesktopHost Build()
    {
        return new(_hostApplicationBuilder.Build());
    }

    private static HostApplicationBuilder InitializeHostApplicationBuilder(string[]? args)
    {
        var configuration = new ConfigurationManager();

        configuration.AddEnvironmentVariables("ASPNETCORE_");

        return new(new HostApplicationBuilderSettings
        {
            Args = args,
            Configuration = configuration
        });
    }

    private void InitializeDefaultServices()
    {
        Services.AddHttpClient();
        Services.AddWpfBlazorWebView();
        Services.AddSingleton<WebViewInstaller>();
        Services.AddSingleton<Application>();
        Services.AddSingleton<Window, BlazorDesktopWindow>();
        Services.AddHostedService<BlazorDesktopService>();
    }

    private RootComponentMappingCollection InitializeRootComponents()
    {
        var rootComponents = new RootComponentMappingCollection();

        Services.AddSingleton(rootComponents);

        return rootComponents;
    }

    private BlazorDesktopHostEnvironment InitializeEnvironment()
    {
        var hostEnvironment = new BlazorDesktopHostEnvironment(_hostApplicationBuilder.Environment, Configuration);

        Services.AddSingleton<IWebHostEnvironment>(hostEnvironment);

        return hostEnvironment;
    }

    private ConfigureWindowBuilder InitializeWindowBuilder()
    {
        return new ConfigureWindowBuilder(Configuration);
    }
}
