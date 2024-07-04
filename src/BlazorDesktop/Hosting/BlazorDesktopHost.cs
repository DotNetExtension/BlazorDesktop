// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Hosting;

/// <summary>
/// A host object for Blazor running under WPF. Use <see cref="BlazorDesktopHostBuilder"/>
/// to initialize a <see cref="BlazorDesktopHost"/>.
/// </summary>
public sealed class BlazorDesktopHost : IHost, IAsyncDisposable
{
    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

    /// <summary>
    /// Gets the service provider associated with the application.
    /// </summary>
    public IServiceProvider Services => _host.Services;

    private readonly IHost _host;

    internal BlazorDesktopHost(IHost host)
    {
        _host = host;
    }

    /// <summary>
    /// Starts the host.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>A task that represents the startup of the <see cref="BlazorDesktopHost"/>.</returns>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _host.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Shuts down the host.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>A task that represents the shutdown of the <see cref="BlazorDesktopHost"/>.</returns>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _host.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Disposes the host.
    /// </summary>
    public void Dispose()
    {
        _host.Dispose();
    }

    /// <summary>
    /// Disposes the host asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> which respresents the completion of disposal.</returns>
    public ValueTask DisposeAsync()
    {
        return ((IAsyncDisposable)_host).DisposeAsync();
    }
}
